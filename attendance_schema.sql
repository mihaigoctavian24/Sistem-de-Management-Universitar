CREATE TABLE IF NOT EXISTS public.attendance (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    student_id UUID NOT NULL REFERENCES public.students(id) ON DELETE CASCADE,
    course_id BIGINT NOT NULL REFERENCES public.courses(id) ON DELETE CASCADE,
    date DATE NOT NULL,
    status TEXT NOT NULL CHECK (status IN ('Present', 'Absent', 'Excused')),
    created_at TIMESTAMPTZ DEFAULT NOW(),
    UNIQUE(student_id, course_id, date)
);

-- Enable RLS
ALTER TABLE public.attendance ENABLE ROW LEVEL SECURITY;

-- Policies
-- Professors can view attendance for their courses
CREATE POLICY "Professors can view attendance for their courses" ON public.attendance
    FOR SELECT
    USING (
        EXISTS (
            SELECT 1 FROM public.courses c
            WHERE c.id = attendance.course_id
            AND c.professor_id = auth.uid()
        )
    );

-- Professors can insert/update attendance for their courses
CREATE POLICY "Professors can manage attendance for their courses" ON public.attendance
    FOR ALL
    USING (
        EXISTS (
            SELECT 1 FROM public.courses c
            WHERE c.id = attendance.course_id
            AND c.professor_id = auth.uid()
        )
    );

-- Students can view their own attendance
CREATE POLICY "Students can view their own attendance" ON public.attendance
    FOR SELECT
    USING (
        student_id = auth.uid()
    );

-- Admins/Deans/Secretaries can view all
CREATE POLICY "Staff can view all attendance" ON public.attendance
    FOR SELECT
    USING (
        EXISTS (
            SELECT 1 FROM public.profiles
            WHERE id = auth.uid()
            AND role IN ('admin', 'dean', 'secretary', 'rector')
        )
    );
