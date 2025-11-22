DO $$
DECLARE
    v_student_id uuid;
BEGIN
    SELECT id INTO v_student_id FROM auth.users WHERE email = 'student.test@stud.rau.ro';

    IF v_student_id IS NOT NULL THEN
        -- Grade for Algorithms (Course ID 4)
        INSERT INTO public.grades (student_id, course_id, value, date, created_at)
        VALUES (v_student_id, 4, 9, NOW(), NOW());

        -- Grade for Data Structures (Course ID 5)
        INSERT INTO public.grades (student_id, course_id, value, date, created_at)
        VALUES (v_student_id, 5, 10, NOW() - INTERVAL '1 day', NOW());
    END IF;
END $$;
