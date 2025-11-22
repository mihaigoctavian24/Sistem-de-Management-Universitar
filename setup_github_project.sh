#!/bin/bash

# University Management System - GitHub Project Setup Script
# This script uses the GitHub CLI (gh) to populate the repository with Milestones and Issues.

echo "🚀 Starting GitHub Project Setup..."

# Function to create a milestone if it doesn't exist
create_milestone() {
    TITLE=$1
    DESC=$2
    DUE_DATE=$3
    
    echo "📅 Creating Milestone: $TITLE..."
    gh api repos/:owner/:repo/milestones -f title="$TITLE" -f description="$DESC" -f due_on="$DUE_DATE" > /dev/null 2>&1 || echo "   (Milestone might already exist or error occurred)"
}

# Function to create an issue linked to a milestone
create_issue() {
    TITLE=$1
    BODY=$2
    MILESTONE=$3
    LABELS=$4
    
    echo "📝 Creating Issue: $TITLE..."
    gh issue create --title "$TITLE" --body "$BODY" --milestone "$MILESTONE" --label "$LABELS"
}

# --- 1. Create Milestones ---
# Dates are placeholders (approximate)

# Phase 2: Core Structure (Start: Now, Duration: ~2 weeks)
create_milestone "Phase 2: Core Structure" "Focus on University Structure (Faculties, Groups) and User Profiles." "$(date -v+14d +%Y-%m-%dT%H:%M:%SZ 2>/dev/null || date -d "+14 days" +%Y-%m-%dT%H:%M:%SZ)"

# Phase 3: Academic Operations (Start: +2 weeks, Duration: ~3 weeks)
create_milestone "Phase 3: Academic Operations" "Core academic loop: Courses, Grades, Attendance, Notifications." "$(date -v+35d +%Y-%m-%dT%H:%M:%SZ 2>/dev/null || date -d "+35 days" +%Y-%m-%dT%H:%M:%SZ)"

# Phase 4: Administrative & Reporting (Start: +5 weeks, Duration: ~2 weeks)
create_milestone "Phase 4: Administrative & Reporting" "Secretariat flows and Reporting dashboards." "$(date -v+49d +%Y-%m-%dT%H:%M:%SZ 2>/dev/null || date -d "+49 days" +%Y-%m-%dT%H:%M:%SZ)"

# Phase 5: Polish & Launch (Start: +7 weeks, Duration: ~1 week)
create_milestone "Phase 5: Polish & Launch" "UI Refinements, Testing, and Deployment." "$(date -v+56d +%Y-%m-%dT%H:%M:%SZ 2>/dev/null || date -d "+56 days" +%Y-%m-%dT%H:%M:%SZ)"


# --- 2. Create Issues for Phase 2 ---

create_issue "[FEAT] Faculty Management API" \
"**Description**
Implement the backend API for managing Faculties.

**Requirements**
- [ ] CRUD Endpoints: \`GET\`, \`POST\`, \`PUT\`, \`DELETE\` /api/faculties
- [ ] RBAC: Only \`Admin\` and \`Rector\` can modify.
- [ ] Validation: Name required, unique.

**Technical Details**
- Controller: \`FacultiesController\`
- Service: \`FacultyService\`" \
"Phase 2: Core Structure" "backend,enhancement"

create_issue "[FEAT] Faculty Management UI" \
"**Description**
Implement the frontend UI for managing Faculties.

**Requirements**
- [ ] Page: \`/admin/faculties\`
- [ ] Component: \`MudTable\` with inline editing or Dialog.
- [ ] Actions: Add, Edit, Delete (with confirmation).

**Technical Details**
- Page: \`Faculties.razor\`" \
"Phase 2: Core Structure" "frontend,enhancement"

create_issue "[FEAT] Group & Series Management" \
"**Description**
Implement management for Study Programs, Series, and Groups.

**Requirements**
- [ ] API: CRUD for Programs, Series, Groups.
- [ ] UI: Hierarchical view or filtered tables.
- [ ] Relationship: Faculty -> Program -> Series -> Group.

**Technical Details**
- Entities: \`Program\`, \`Series\`, \`Group\`" \
"Phase 2: Core Structure" "fullstack,enhancement"

create_issue "[FEAT] User Profile Management" \
"**Description**
Allow Admins to manage user profiles and assign them to specific Faculties/Groups.

**Requirements**
- [ ] Admin Page: List all users.
- [ ] Edit Profile: Assign Role, Faculty, Group.
- [ ] Sync with Supabase Auth metadata." \
"Phase 2: Core Structure" "fullstack,enhancement"

create_issue "[CHORE] Data Seeding Script" \
"**Description**
Create a script or tool to populate the database with initial data for testing.

**Requirements**
- [ ] Seed Faculties, Programs, Groups.
- [ ] Seed Test Users (Student, Prof, Admin)." \
"Phase 2: Core Structure" "backend,chore"

create_issue "[CHORE] Global Error Handling & Logging" \
"**Description**
Implement robust error handling and audit logging.

**Requirements**
- [ ] API: Global Exception Handler (ProblemDetails).
- [ ] Client: Error Boundary.
- [ ] Audit: Log critical actions (POST/PUT/DELETE) to \`AuditLogs\` table." \
"Phase 2: Core Structure" "backend,chore"


# --- 3. Create Issues for Phase 3 ---

create_issue "[FEAT] Course Management" \
"**Description**
Manage Courses and Professor assignments.

**Requirements**
- [ ] Create Courses linked to Programs/Semesters.
- [ ] Assign Professors to Courses." \
"Phase 3: Academic Operations" "fullstack,enhancement"

create_issue "[FEAT] Grading System API" \
"**Description**
Backend logic for grading.

**Requirements**
- [ ] Endpoint: \`POST /api/grades\`
- [ ] Validation: Grade 1-10.
- [ ] Batch processing support." \
"Phase 3: Academic Operations" "backend,enhancement"

create_issue "[FEAT] Professor Gradebook UI" \
"**Description**
Interface for professors to grade students.

**Requirements**
- [ ] Grid view of students in a group.
- [ ] Input fields for grades.
- [ ] Save/Publish buttons." \
"Phase 3: Academic Operations" "frontend,enhancement"

create_issue "[FEAT] Student Grade View" \
"**Description**
Read-only view for students to see their grades.

**Requirements**
- [ ] Filter by Semester/Year.
- [ ] Calculate averages (if applicable)." \
"Phase 3: Academic Operations" "frontend,enhancement"

create_issue "[FEAT] Attendance Module" \
"**Description**
Track student attendance.

**Requirements**
- [ ] Professor: Mark present/absent.
- [ ] Student: View attendance history." \
"Phase 3: Academic Operations" "fullstack,enhancement"

create_issue "[FEAT] In-App Notification System" \
"**Description**
Realtime notifications for users.

**Requirements**
- [ ] \`Notifications\` table.
- [ ] Supabase Realtime subscription.
- [ ] UI Badge/Dropdown." \
"Phase 3: Academic Operations" "fullstack,enhancement"


# --- 4. Create Issues for Phase 4 ---

create_issue "[FEAT] Secretariat Module" \
"**Description**
Administrative tools for secretaries.

**Requirements**
- [ ] Student Enrollment flows.
- [ ] Document generation." \
"Phase 4: Administrative & Reporting" "fullstack,enhancement"

create_issue "[FEAT] Reports Module" \
"**Description**
Visual reports and statistics.

**Requirements**
- [ ] Charts for grades distribution.
- [ ] Attendance stats." \
"Phase 4: Administrative & Reporting" "frontend,enhancement"


# --- 5. Create Issues for Phase 5 ---

create_issue "[UI] UI/UX Refinements" \
"**Description**
Polish the application interface.

**Requirements**
- [ ] Responsive check.
- [ ] Animations.
- [ ] Theme consistency." \
"Phase 5: Polish & Launch" "frontend,design"

create_issue "[TEST] Unit & E2E Testing" \
"**Description**
Ensure system stability.

**Requirements**
- [ ] Unit tests for critical business logic.
- [ ] E2E tests for main flows." \
"Phase 5: Polish & Launch" "testing"

echo "✅ GitHub Project Setup Complete!"
