-- 1. Seed Faculty
INSERT INTO public.faculties (name, created_at) 
VALUES ('Computer Science', NOW()) 
ON CONFLICT DO NOTHING;

-- 2. Seed Program (linked to Faculty)
DO $$
DECLARE
    v_faculty_id bigint;
    v_program_id bigint;
    v_group_id bigint;
    v_prof_id uuid;
    v_student_id uuid;
BEGIN
    -- Get Faculty ID
    SELECT id INTO v_faculty_id FROM public.faculties WHERE name = 'Computer Science' LIMIT 1;

    -- Insert Program
    INSERT INTO public.study_programs (name, faculty_id, created_at)
    VALUES ('Software Engineering', v_faculty_id, NOW())
    RETURNING id INTO v_program_id;

    -- 3. Seed Group
    INSERT INTO public.groups (name, program_id, created_at)
    VALUES ('101', v_program_id, NOW())
    RETURNING id INTO v_group_id;

    -- 4. Get an existing user to be the Professor (e.g., the Admin user)
    SELECT id INTO v_prof_id FROM auth.users LIMIT 1;

    -- 5. Seed Course
    INSERT INTO public.courses (name, program_id, professor_id, semester, credits, created_at)
    VALUES ('Algorithms', v_program_id, v_prof_id, 1, 5, NOW());

    INSERT INTO public.courses (name, program_id, professor_id, semester, credits, created_at)
    VALUES ('Data Structures', v_program_id, v_prof_id, 2, 6, NOW());

    -- 6. Seed Dummy Student Profile (for grading testing)
    -- Note: This student won't be able to login unless a corresponding auth.user exists,
    -- but they will appear in the Grade Entry list.
    v_student_id := gen_random_uuid();
    
    INSERT INTO public.profiles (id, email, first_name, last_name, role, created_at)
    VALUES (v_student_id, 'student.test@stud.rau.ro', 'Test', 'Student', 'student', NOW());
    
    -- Also insert into students table if it exists and is separate
    -- INSERT INTO public.students (id, group_id, registration_number) VALUES (v_student_id, v_group_id, 'REG123');

END $$;
