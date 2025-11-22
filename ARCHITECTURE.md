# System Architecture

## Overview
The University Management System (SMU) is a modern, cloud-native web application designed for scalability and user experience. It follows a **Clean Architecture** approach, separating concerns between the User Interface, API, and Data layers.

## Technology Stack

### Frontend
- **Framework**: [Blazor WebAssembly (.NET 8)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- **UI Library**: [MudBlazor](https://mudblazor.com/) (Material Design components)
- **State Management**: In-memory state containers + Supabase Realtime
- **Authentication**: Custom `AuthenticationStateProvider` integrating with Supabase Auth

### Backend
- **Framework**: [ASP.NET Core Web API (.NET 8)](https://dotnet.microsoft.com/apps/aspnet/apis)
- **Authentication**: JWT Bearer (validated against Supabase Auth)
- **Authorization**: Policy-based RBAC (Role-Based Access Control)
- **Data Access**: [Supabase C# Client](https://github.com/supabase-community/supabase-csharp) / Postgrest

### Database & Infrastructure
- **Platform**: [Supabase](https://supabase.com/)
- **Database**: PostgreSQL
- **Auth**: Supabase GoTrue (JWT)
- **Storage**: Supabase Storage (for profile pictures, documents)
- **Realtime**: Supabase Realtime (WebSockets)

## Project Structure

```
Sistem-de-Management-Universitar/
├── UniversityManagementSystem.sln        # Solution file
├── UniversityManagementSystem.API/       # Backend Project
│   ├── Controllers/                      # API Endpoints
│   ├── Models/                           # Data Transfer Objects (DTOs) & Entities
│   ├── Services/                         # Business Logic
│   └── Program.cs                        # App Configuration & DI
├── UniversityManagementSystem.Client/    # Frontend Project
│   ├── Pages/                            # Razor Pages (Views)
│   ├── Shared/                           # Shared Components (Layouts, Nav)
│   ├── Auth/                             # Auth Providers & Logic
│   └── wwwroot/                          # Static Assets
├── Docs/                                 # Documentation
│   ├── prd_university_management.md      # Product Requirements
│   ├── schema.sql                        # Database Schema
│   └── ANALYSIS.md                       # Gap Analysis
└── .github/                              # GitHub Configuration (Workflows, Templates)
```

## Data Flow

1.  **User Interaction**: User interacts with MudBlazor components in the browser.
2.  **API Call**: The Client sends an HTTP request (with JWT) to the ASP.NET Core API.
3.  **Validation & Logic**: The API validates the request and applies business logic.
4.  **Data Access**: The API queries Supabase (PostgreSQL) via the Supabase Client.
5.  **Response**: Data is returned to the Client and rendered.
6.  **Realtime**: (Optional) Updates are pushed to connected clients via WebSockets.

## Security

-   **Authentication**: Handled by Supabase. Tokens are passed to the backend via `Authorization: Bearer <token>` header.
-   **Authorization**:
    -   **Frontend**: `AuthorizeView` and `[Authorize]` attribute to protect routes/UI.
    -   **Backend**: `[Authorize(Roles = ...)]` on Controllers/Actions.
    -   **Database**: Row Level Security (RLS) policies in PostgreSQL to ensure users can only access their own data (where applicable).
