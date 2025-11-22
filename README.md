# University Management System (SMU)

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Status](https://img.shields.io/badge/status-development-orange)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-purple)

A comprehensive enterprise solution for managing university academic, administrative, and operational processes. Built with **.NET 8**, **Blazor WebAssembly**, **MudBlazor**, and **Supabase**.

## 📚 Documentation

- [**Product Requirements (PRD)**](Docs/prd_university_management.md): Detailed functional and non-functional requirements.
- [**Architecture**](ARCHITECTURE.md): High-level system design and technology stack.
- [**Analysis & Gaps**](Docs/ANALYSIS.md): Analysis of requirements and technical considerations.
- [**Roadmap**](ROADMAP.md): Development timeline and phases.
- [**Issues & Milestones**](ISSUES_AND_MILESTONES.md): Task tracking.

## 🚀 Features

- **Role-Based Access Control (RBAC)**: Secure access for Students, Professors, Deans, Rectors, and Admins.
- **Academic Management**: Courses, Grades, Attendance, and Curricula.
- **Administrative Tools**: Faculty structure, User management, and Reporting.
- **Modern UI**: Responsive and accessible interface built with MudBlazor.

## 🛠️ Tech Stack

- **Frontend**: Blazor WebAssembly, MudBlazor
- **Backend**: ASP.NET Core Web API
- **Database**: PostgreSQL (via Supabase)
- **Auth**: Supabase Auth (JWT)

## 📦 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Supabase Account](https://supabase.com/)

### Installation

1.  **Clone the repository**
    ```bash
    git clone https://github.com/mihaigoctavian24/Sistem-de-Management-Universitar.git
    cd Sistem-de-Management-Universitar
    ```

2.  **Configure Credentials**
    - Update `UniversityManagementSystem.API/appsettings.json` and `UniversityManagementSystem.Client/wwwroot/appsettings.json` with your Supabase URL and Keys.

3.  **Run the Application**
    ```bash
    # Run the API
    dotnet run --project UniversityManagementSystem.API

    # Run the Client (in a separate terminal)
    dotnet run --project UniversityManagementSystem.Client
    ```

## 🤝 Contributing

Please read [ISSUES_AND_MILESTONES.md](ISSUES_AND_MILESTONES.md) for current tasks.

## 📄 License

This project is licensed under the MIT License.
