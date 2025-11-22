-- Password for all users: F32e891_!!_Admin
-- Hash: $2a$10$h929CMJvob95S.6gp295LuXul.2I//ScJlZYk.7V7h5zaGe51/tVe

DO $$
DECLARE
    v_student_id uuid;
    v_professor_id uuid;
    v_dean_id uuid;
    v_secretary_id uuid;
    v_rector_id uuid;
    v_group_id bigint;
    v_faculty_id bigint;
    v_program_id bigint;
BEGIN
    -- Get existing IDs
    SELECT id INTO v_faculty_id FROM public.faculties WHERE name = 'Computer Science' LIMIT 1;
    SELECT id INTO v_program_id FROM public.programs WHERE name = 'Software Engineering' LIMIT 1;
    SELECT id INTO v_group_id FROM public.groups WHERE name = '101' LIMIT 1;

    -- 1. Create Student
    SELECT id INTO v_student_id FROM auth.users WHERE email = 'student.test@stud.rau.ro';
    IF v_student_id IS NULL THEN
        v_student_id := gen_random_uuid();
        INSERT INTO auth.users (
            instance_id, id, aud, role, email, encrypted_password, email_confirmed_at, raw_user_meta_data, created_at, updated_at
        ) VALUES (
            '00000000-0000-0000-0000-000000000000',
            v_student_id,
            'authenticated',
            'authenticated',
            'student.test@stud.rau.ro',
            '$2a$10$h929CMJvob95S.6gp295LuXul.2I//ScJlZYk.7V7h5zaGe51/tVe',
            NOW(),
            '{"first_name": "Test", "last_name": "Student", "role": "student"}'::jsonb,
            NOW(),
            NOW()
        );
    END IF;

    -- Link Student to Group
    INSERT INTO public.students (id, group_id, registration_number)
    VALUES (v_student_id, v_group_id, 'REG001')
    ON CONFLICT (id) DO NOTHING;


    -- 2. Create Professor
    SELECT id INTO v_professor_id FROM auth.users WHERE email = 'professor.test@stud.rau.ro';
    IF v_professor_id IS NULL THEN
        v_professor_id := gen_random_uuid();
        INSERT INTO auth.users (
            instance_id, id, aud, role, email, encrypted_password, email_confirmed_at, raw_user_meta_data, created_at, updated_at
        ) VALUES (
            '00000000-0000-0000-0000-000000000000',
            v_professor_id,
            'authenticated',
            'authenticated',
            'professor.test@stud.rau.ro',
            '$2a$10$h929CMJvob95S.6gp295LuXul.2I//ScJlZYk.7V7h5zaGe51/tVe',
            NOW(),
            '{"first_name": "Test", "last_name": "Professor", "role": "professor"}'::jsonb,
            NOW(),
            NOW()
        );
    END IF;

    -- Link Professor to Faculty
    INSERT INTO public.professors (id, faculty_id)
    VALUES (v_professor_id, v_faculty_id)
    ON CONFLICT (id) DO NOTHING;


    -- 3. Create Dean
    SELECT id INTO v_dean_id FROM auth.users WHERE email = 'dean.test@stud.rau.ro';
    IF v_dean_id IS NULL THEN
        v_dean_id := gen_random_uuid();
        INSERT INTO auth.users (
            instance_id, id, aud, role, email, encrypted_password, email_confirmed_at, raw_user_meta_data, created_at, updated_at
        ) VALUES (
            '00000000-0000-0000-0000-000000000000',
            v_dean_id,
            'authenticated',
            'authenticated',
            'dean.test@stud.rau.ro',
            '$2a$10$h929CMJvob95S.6gp295LuXul.2I//ScJlZYk.7V7h5zaGe51/tVe',
            NOW(),
            '{"first_name": "Test", "last_name": "Dean", "role": "dean"}'::jsonb,
            NOW(),
            NOW()
        );
    END IF;
    
    -- Update Faculty to set Dean
    UPDATE public.faculties SET dean_id = v_dean_id WHERE id = v_faculty_id;


    -- 4. Create Secretary
    SELECT id INTO v_secretary_id FROM auth.users WHERE email = 'secretary.test@stud.rau.ro';
    IF v_secretary_id IS NULL THEN
        v_secretary_id := gen_random_uuid();
        INSERT INTO auth.users (
            instance_id, id, aud, role, email, encrypted_password, email_confirmed_at, raw_user_meta_data, created_at, updated_at
        ) VALUES (
            '00000000-0000-0000-0000-000000000000',
            v_secretary_id,
            'authenticated',
            'authenticated',
            'secretary.test@stud.rau.ro',
            '$2a$10$h929CMJvob95S.6gp295LuXul.2I//ScJlZYk.7V7h5zaGe51/tVe',
            NOW(),
            '{"first_name": "Test", "last_name": "Secretary", "role": "secretary"}'::jsonb,
            NOW(),
            NOW()
        );
    END IF;


    -- 5. Create Rector
    SELECT id INTO v_rector_id FROM auth.users WHERE email = 'rector.test@stud.rau.ro';
    IF v_rector_id IS NULL THEN
        v_rector_id := gen_random_uuid();
        INSERT INTO auth.users (
            instance_id, id, aud, role, email, encrypted_password, email_confirmed_at, raw_user_meta_data, created_at, updated_at
        ) VALUES (
            '00000000-0000-0000-0000-000000000000',
            v_rector_id,
            'authenticated',
            'authenticated',
            'rector.test@stud.rau.ro',
            '$2a$10$h929CMJvob95S.6gp295LuXul.2I//ScJlZYk.7V7h5zaGe51/tVe',
            NOW(),
            '{"first_name": "Test", "last_name": "Rector", "role": "rector"}'::jsonb,
            NOW(),
            NOW()
        );
    END IF;

END $$;
