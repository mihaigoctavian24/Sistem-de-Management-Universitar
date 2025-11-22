-- Data Seeding Script for Reports Module
-- This script populates grades and attendance for existing students to enable report testing.

DO $$
DECLARE
    student_rec RECORD;
    course_rec RECORD;
    grade_val INT;
    attendance_status TEXT;
    i INT;
BEGIN
    -- Loop through all students
    FOR student_rec IN SELECT id, group_id FROM public.students LOOP
        
        -- 1. Seed Grades
        -- For each course, assign a random grade (5-10) or sometimes fail (1-4)
        FOR course_rec IN SELECT id, credits FROM public.courses LOOP
            -- 80% chance to have a grade for a course
            IF (floor(random() * 10 + 1)::int) > 2 THEN
                grade_val := floor(random() * 10 + 1)::int;
                
                -- Insert grade if not exists
                INSERT INTO public.grades (student_id, course_id, value, date)
                VALUES (student_rec.id, course_rec.id, grade_val, NOW() - (random() * 30 || ' days')::interval)
                ON CONFLICT DO NOTHING; -- Avoid duplicates if run multiple times
            END IF;
        END LOOP;

        -- 2. Seed Attendance
        -- Generate 20 attendance records per student across random courses
        FOR i IN 1..20 LOOP
            -- Pick a random course
            SELECT id INTO course_rec FROM public.courses ORDER BY random() LIMIT 1;
            
            -- Determine status (70% Present, 20% Absent, 10% Excused)
            IF (random() < 0.7) THEN
                attendance_status := 'Present';
            ELSIF (random() < 0.9) THEN
                attendance_status := 'Absent';
            ELSE
                attendance_status := 'Excused';
            END IF;

            INSERT INTO public.attendance (student_id, course_id, date, status)
            VALUES (
                student_rec.id, 
                course_rec.id, 
                NOW() - (random() * 60 || ' days')::interval, 
                attendance_status
            );
        END LOOP;
        
    END LOOP;
END $$;
