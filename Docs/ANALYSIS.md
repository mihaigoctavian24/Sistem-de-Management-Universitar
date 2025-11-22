# Project Analysis & Gap Analysis

## Overview
This document provides an analysis of the University Management System (SMU) based on the Product Requirements Document (PRD). It identifies key features, potential gaps, and technical considerations for the implementation phase.

## Key Features
- **Role-Based Access Control (RBAC)**: Granular permissions for Students, Professors, Deans, Rectors, Secretaries, and Admins.
- **Core Modules**:
    - **Students**: Management of student profiles, enrollment, and academic history.
    - **Professors**: Course assignment and grading.
    - **Faculties/Groups**: Structural organization of the university.
    - **Grades & Attendance**: Academic record keeping.
- **Tech Stack**: Blazor WebAssembly (MudBlazor) + ASP.NET Core Web API + Supabase (Auth, DB, Realtime).

## Gap Analysis & Recommendations

### 1. Validation Rules
**Observation**: The PRD mentions validation (e.g., "validator MudForm rulează") but lacks specific rules.
**Recommendation**: Define strict validation rules for:
- **Grades**: Must be integers between 1 and 10.
- **Attendance**: Valid statuses (Present, Absent, Excused).
- **Email**: Must match university domain (e.g., `@stud.rau.ro` or `@rau.ro`).
- **CNP/ID**: Format validation for student identification.

### 2. Notification System
**Observation**: The PRD mentions "Decanul primește notificare" (The Dean receives a notification).
**Recommendation**: Clarify the delivery mechanism.
- **MVP**: In-app notifications using a `Notifications` table and Supabase Realtime.
- **Future**: Email notifications using an external provider (e.g., SendGrid) triggered by Supabase Edge Functions.

### 3. Data Seeding
**Observation**: No explicit strategy for initial data population.
**Recommendation**: Create a "Seeding" script or Admin tool to populate:
- Initial Faculties and Departments.
- Test users for each role (Student, Professor, Admin, etc.) to facilitate testing.

### 4. Error Handling
**Observation**: "Timp răspuns API < 200 ms" is a non-functional requirement.
**Recommendation**: Implement:
- **Global Exception Handler** in API to return standardized error responses (ProblemDetails).
- **Global Error Boundary** in Blazor to catch unhandled UI exceptions and display user-friendly messages.

### 5. Audit Logging
**Observation**: "Audit complet" is required.
**Recommendation**:
- Implement an `AuditLogs` table.
- Use an Action Filter in ASP.NET Core to automatically log all state-changing requests (POST, PUT, DELETE) with `UserId`, `Timestamp`, `Action`, and `Payload`.

## Technical Considerations
- **Supabase Realtime**: Ensure RLS policies allow realtime subscriptions only for authorized users.
- **State Management**: Use a simple state container or Flux pattern for complex flows (like multi-step registration).
- **Performance**: Use pagination for large lists (Students, Grades) from the start.

## Conclusion
The PRD is comprehensive. The immediate focus should be on establishing the **Core Modules** with the recommended validation and error handling patterns in place.
