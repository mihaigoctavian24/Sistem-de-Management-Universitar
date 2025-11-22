-- Create Secretary Test User
-- This script updates an existing user to have the secretary role
-- You'll need to replace the email with an actual user email from your system

-- Option 1: Update an existing test user to secretary role
UPDATE public.profiles 
SET role = 'secretary' 
WHERE email = 'secretary.test@rau.ro';

-- Option 2: If you need to check existing users first
-- Run this query to see available users:
-- SELECT id, email, role FROM public.profiles WHERE role = 'student' LIMIT 5;

-- Then update one of them:
-- UPDATE public.profiles SET role = 'secretary' WHERE email = 'REPLACE_WITH_ACTUAL_EMAIL';

-- Verify the update
SELECT id, email, first_name, last_name, role 
FROM public.profiles 
WHERE role = 'secretary';
