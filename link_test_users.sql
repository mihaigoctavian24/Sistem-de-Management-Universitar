-- 1. Confirm Emails
UPDATE auth.users
SET email_confirmed_at = NOW()
WHERE email LIKE '%test@stud.rau.ro';

-- 2. Link Student to Group
DO $$
DECLARE
    v_student_id uuid;
    v_group_id bigint;
BEGIN
    SELECT id INTO v_student_id FROM auth.users WHERE email = 'student.test@stud.rau.ro';
    SELECT id INTO v_group_id FROM public.groups WHERE name = '101' LIMIT 1;

    IF v_student_id IS NOT NULL THEN
        -- Ensure Profile Exists
        INSERT INTO public.profiles (id, email, first_name, last_name, role)
        VALUES (v_student_id, 'student.test@stud.rau.ro', 'Test', 'Student', 'student')
        ON CONFLICT (id) DO NOTHING;

        -- Link to Group
        IF v_group_id IS NOT NULL THEN
            INSERT INTO public.students (id, group_id, registration_number)
            VALUES (v_student_id, v_group_id, 'REG001')
            ON CONFLICT (id) DO NOTHING;
        END IF;
    END IF;
END $$;

-- 3. Link Professor to Faculty
DO $$
DECLARE
    v_prof_id uuid;
    v_faculty_id bigint;
BEGIN
    SELECT id INTO v_prof_id FROM auth.users WHERE email = 'professor.test@stud.rau.ro';
    SELECT id INTO v_faculty_id FROM public.faculties WHERE name = 'Computer Science' LIMIT 1;

    IF v_prof_id IS NOT NULL THEN
        -- Ensure Profile Exists
        INSERT INTO public.profiles (id, email, first_name, last_name, role)
        VALUES (v_prof_id, 'professor.test@stud.rau.ro', 'Test', 'Professor', 'professor')
        ON CONFLICT (id) DO NOTHING;

        -- Link to Faculty
        IF v_faculty_id IS NOT NULL THEN
            INSERT INTO public.professors (id, faculty_id)
            VALUES (v_prof_id, v_faculty_id)
            ON CONFLICT (id) DO NOTHING;
        END IF;
    END IF;
END $$;

-- 4. Link Dean to Faculty
DO $$
DECLARE
    v_dean_id uuid;
    v_faculty_id bigint;
BEGIN
    SELECT id INTO v_dean_id FROM auth.users WHERE email = 'dean.test@stud.rau.ro';
    SELECT id INTO v_faculty_id FROM public.faculties WHERE name = 'Computer Science' LIMIT 1;

    IF v_dean_id IS NOT NULL THEN
        -- Ensure Profile Exists
        INSERT INTO public.profiles (id, email, first_name, last_name, role)
        VALUES (v_dean_id, 'dean.test@stud.rau.ro', 'Test', 'Dean', 'dean')
        ON CONFLICT (id) DO NOTHING;

        -- Link to Faculty
        IF v_faculty_id IS NOT NULL THEN
            UPDATE public.faculties SET dean_id = v_dean_id WHERE id = v_faculty_id;
        END IF;
    END IF;
END $$;

-- 5. Ensure Profiles for Secretary and Rector
DO $$
DECLARE
    v_sec_id uuid;
    v_rec_id uuid;
BEGIN
    SELECT id INTO v_sec_id FROM auth.users WHERE email = 'secretary.test@stud.rau.ro';
    SELECT id INTO v_rec_id FROM auth.users WHERE email = 'rector.test@stud.rau.ro';

    IF v_sec_id IS NOT NULL THEN
        INSERT INTO public.profiles (id, email, first_name, last_name, role)
        VALUES (v_sec_id, 'secretary.test@stud.rau.ro', 'Test', 'Secretary', 'secretary')
        ON CONFLICT (id) DO NOTHING;
    END IF;

    IF v_rec_id IS NOT NULL THEN
        INSERT INTO public.profiles (id, email, first_name, last_name, role)
        VALUES (v_rec_id, 'rector.test@stud.rau.ro', 'Test', 'Rector', 'rector')
        ON CONFLICT (id) DO NOTHING;
    END IF;
END $$;
