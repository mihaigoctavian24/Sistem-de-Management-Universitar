# Project Roadmap

This document outlines the development phases for the University Management System.

## Phase 1: Foundation & Authentication (Completed)
- [x] Project Initialization (Solution, API, Client)
- [x] Database Schema Design (Supabase)
- [x] Authentication Setup (JWT, Supabase Auth)
- [x] Basic UI Layout (MudBlazor)
- [x] Role-Based Access Control (RBAC) Foundation

## Phase 2: Core Structure (Current Focus)
**Goal**: Establish the university structure (Faculties, Programs, Groups) to allow student enrollment.
- [ ] **Module: Faculties & Departments**
    - CRUD for Faculties
    - CRUD for Study Programs
    - CRUD for Series/Groups
- [ ] **Module: Users & Profiles**
    - Extended Profile Management (Students, Professors)
    - Admin User Management (Assign Roles)
- [ ] **Technical Foundation**
    - [ ] Data Seeding Script (Faculties, Test Users)
    - [ ] Global Error Handling (API & Client)
    - [ ] Audit Logging (Action Filters)
    - [ ] Validation Rules (FluentValidation, Email Domains)

## Phase 3: Academic Operations
**Goal**: Enable the core academic loop (Courses, Grades, Attendance).
- [ ] **Module: Courses**
    - Course creation and assignment to Professors/Groups
- [ ] **Module: Grades**
    - Gradebook UI for Professors
    - Grade viewing for Students
    - Grade approval flow (Dean)
- [ ] **Module: Attendance**
    - Daily attendance tracking
    - Attendance reports
- [ ] **Module: Notifications**
    - In-App Notification System (Realtime)

## Phase 4: Administrative & Reporting
**Goal**: Advanced management and insights.
- [ ] **Module: Secretariat**
    - Student enrollment flows
    - Document generation (Certificates)
- [ ] **Module: Reports**
    - Academic performance charts
    - Attendance statistics

## Phase 5: Polish & Launch
- [ ] **UI/UX Refinements** (Animations, Responsive adjustments)
- [ ] **Testing** (Unit Tests, E2E Tests)
- [ ] **Deployment** (CI/CD Pipelines)
