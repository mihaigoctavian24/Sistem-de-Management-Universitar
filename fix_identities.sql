DO $$
DECLARE
    v_user_id uuid;
    v_email text;
BEGIN
    -- 1. Student
    v_email := 'student.test@stud.rau.ro';
    SELECT id INTO v_user_id FROM auth.users WHERE email = v_email;
    
    IF v_user_id IS NOT NULL THEN
        INSERT INTO auth.identities (id, user_id, provider_id, identity_data, provider, last_sign_in_at, created_at, updated_at)
        VALUES (
            gen_random_uuid(),
            v_user_id,
            v_user_id, -- provider_id is often user_id for email provider
            jsonb_build_object('sub', v_user_id, 'email', v_email, 'email_verified', true, 'phone_verified', false),
            'email',
            NOW(),
            NOW(),
            NOW()
        ) ON CONFLICT (provider, provider_id) DO NOTHING;
    END IF;

    -- 2. Professor
    v_email := 'professor.test@stud.rau.ro';
    SELECT id INTO v_user_id FROM auth.users WHERE email = v_email;
    
    IF v_user_id IS NOT NULL THEN
        INSERT INTO auth.identities (id, user_id, provider_id, identity_data, provider, last_sign_in_at, created_at, updated_at)
        VALUES (
            gen_random_uuid(),
            v_user_id,
            v_user_id,
            jsonb_build_object('sub', v_user_id, 'email', v_email, 'email_verified', true, 'phone_verified', false),
            'email',
            NOW(),
            NOW(),
            NOW()
        ) ON CONFLICT (provider, provider_id) DO NOTHING;
    END IF;

    -- 3. Dean
    v_email := 'dean.test@stud.rau.ro';
    SELECT id INTO v_user_id FROM auth.users WHERE email = v_email;
    
    IF v_user_id IS NOT NULL THEN
        INSERT INTO auth.identities (id, user_id, provider_id, identity_data, provider, last_sign_in_at, created_at, updated_at)
        VALUES (
            gen_random_uuid(),
            v_user_id,
            v_user_id,
            jsonb_build_object('sub', v_user_id, 'email', v_email, 'email_verified', true, 'phone_verified', false),
            'email',
            NOW(),
            NOW(),
            NOW()
        ) ON CONFLICT (provider, provider_id) DO NOTHING;
    END IF;

    -- 4. Secretary
    v_email := 'secretary.test@stud.rau.ro';
    SELECT id INTO v_user_id FROM auth.users WHERE email = v_email;
    
    IF v_user_id IS NOT NULL THEN
        INSERT INTO auth.identities (id, user_id, provider_id, identity_data, provider, last_sign_in_at, created_at, updated_at)
        VALUES (
            gen_random_uuid(),
            v_user_id,
            v_user_id,
            jsonb_build_object('sub', v_user_id, 'email', v_email, 'email_verified', true, 'phone_verified', false),
            'email',
            NOW(),
            NOW(),
            NOW()
        ) ON CONFLICT (provider, provider_id) DO NOTHING;
    END IF;

    -- 5. Rector
    v_email := 'rector.test@stud.rau.ro';
    SELECT id INTO v_user_id FROM auth.users WHERE email = v_email;
    
    IF v_user_id IS NOT NULL THEN
        INSERT INTO auth.identities (id, user_id, provider_id, identity_data, provider, last_sign_in_at, created_at, updated_at)
        VALUES (
            gen_random_uuid(),
            v_user_id,
            v_user_id,
            jsonb_build_object('sub', v_user_id, 'email', v_email, 'email_verified', true, 'phone_verified', false),
            'email',
            NOW(),
            NOW(),
            NOW()
        ) ON CONFLICT (provider, provider_id) DO NOTHING;
    END IF;

END $$;
