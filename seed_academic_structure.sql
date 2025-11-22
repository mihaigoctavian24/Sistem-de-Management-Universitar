-- Seed Faculties
INSERT INTO public.faculties (name, created_at) VALUES
('Faculty of Computer Science', NOW()),
('Faculty of Economics', NOW()),
('Faculty of Law', NOW());

-- Seed Programs
-- Assuming IDs 1, 2, 3 for the faculties above. In production, we should select by name.
INSERT INTO public.programs (name, faculty_id, study_type, duration, created_at) VALUES
('Computer Science', (SELECT id FROM public.faculties WHERE name = 'Faculty of Computer Science' LIMIT 1), 'Bachelor', 3, NOW()),
('Information Technology', (SELECT id FROM public.faculties WHERE name = 'Faculty of Computer Science' LIMIT 1), 'Bachelor', 4, NOW()),
('Software Engineering', (SELECT id FROM public.faculties WHERE name = 'Faculty of Computer Science' LIMIT 1), 'Master', 2, NOW()),
('Business Administration', (SELECT id FROM public.faculties WHERE name = 'Faculty of Economics' LIMIT 1), 'Bachelor', 3, NOW()),
('International Law', (SELECT id FROM public.faculties WHERE name = 'Faculty of Law' LIMIT 1), 'Bachelor', 4, NOW());

-- Seed Groups
INSERT INTO public.groups (name, program_id, year, created_at) VALUES
('CS-101', (SELECT id FROM public.programs WHERE name = 'Computer Science' LIMIT 1), 1, NOW()),
('CS-201', (SELECT id FROM public.programs WHERE name = 'Computer Science' LIMIT 1), 2, NOW()),
('IT-101', (SELECT id FROM public.programs WHERE name = 'Information Technology' LIMIT 1), 1, NOW()),
('SE-M1', (SELECT id FROM public.programs WHERE name = 'Software Engineering' LIMIT 1), 1, NOW()),
('BA-101', (SELECT id FROM public.programs WHERE name = 'Business Administration' LIMIT 1), 1, NOW()),
('LAW-101', (SELECT id FROM public.programs WHERE name = 'International Law' LIMIT 1), 1, NOW());
