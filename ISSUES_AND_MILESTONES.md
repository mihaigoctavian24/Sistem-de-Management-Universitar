# GitHub Issues & Milestones

This document simulates a GitHub Project board. Use these items to track progress.

## Milestone 1: Core Structure (v0.2)
**Due Date**: TBD
**Focus**: Faculties, Groups, and User Profiles.

### Issues
- [ ] **[FEAT] Implement Faculty Management API**
    - Create `FacultiesController`
    - Implement `GET`, `POST`, `PUT`, `DELETE` endpoints
    - Add RBAC (Admin/Rector only)
- [ ] **[FEAT] Implement Faculty Management UI**
    - Create `FacultiesPage.razor`
    - Add `MudTable` with inline editing
- [ ] **[FEAT] Implement Group Management**
    - API for Groups (linked to Programs)
    - UI for managing Groups
- [ ] **[FEAT] User Profile Management**
    - Allow Admins to edit user profiles (assign specific Faculty/Group)

## Milestone 2: Academic Loop (v0.3)
**Focus**: Courses, Grades, Attendance.

### Issues
- [ ] **[FEAT] Course Management**
    - Assign Professors to Courses
- [ ] **[FEAT] Grading System API**
    - `POST /grades` with validation
- [ ] **[FEAT] Professor Gradebook UI**
    - Interface for Professors to input grades for a specific group
- [ ] **[FEAT] Student Grade View**
    - Read-only view for Students to see their grades

## Milestone 3: Polish & Release (v1.0)
**Focus**: Testing, UI Polish, Documentation.

### Issues
- [ ] **[TEST] Unit Tests for Grading Logic**
- [ ] **[DOCS] API Documentation (Swagger)**
- [ ] **[UI] Dark Mode Toggle & Theme Customization**
