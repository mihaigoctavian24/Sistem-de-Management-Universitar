# **PRD – Sistem de Management Universitar**

## 1. Introducere & Scop
Sistemul de Management Universitar (SMU) este o platformă enterprise destinată administrării complete a unei universități moderne. Soluția integrează managementul academic, administrativ și operațional într-un ecosistem digital unificat.

### Scopul sistemului
- Centralizarea datelor academice, administrative și operaționale.
- Automatizarea proceselor universitare precum: gestionarea studenților, profesorilor, facultăților, grupelor, seriilor, notelor, prezențelor și situațiilor școlare.
- Oferirea unei interfețe intuitive bazate pe **MudBlazor**.
- Asigurarea securității datelor prin roluri, permisiuni și audit.

## 2. Actori și Roluri
### 2.1 Student
#### Scopuri principale:
- Vizualizare note, absențe, orar.
- Înscriere la cursuri/seminarii.
- Completarea evaluărilor profesorilor.
- Comunicarea cu cadrul didactic.

#### Scenarii:
- Studentul se autentifică și vede dashboard-ul studentului.
- Studentul accesează catalogul personal.
- Studentul descarcă adeverințe (dacă modulul este activ).
- Studentul trimite cereri către secretariat.

### 2.2 Profesor
#### Scopuri:
- Gestionarea cursurilor și grupelor alocate.
- Introducerea notelor.
- Marcarea prezențelor.
- Comunicarea cu studenții.

#### Scenarii:
- Profesorul vede lista cursurilor.
- Profesorul deschide catalogul unei grupe și introduce note.
- Profesorul trimite anunțuri către studenți.

### 2.3 Decan
#### Scopuri:
- Supravegherea activității academice a facultății.
- Aprobare situații, note finale, exmatriculări.
- Gestionare profesori din facultate.

#### Scenarii:
- Decanul analizează rapoarte privind rata de promovabilitate.
- Decanul aprobă/respinge modificări administrative.

### 2.4 Rector
#### Scopuri:
- Supervizarea întregi universități.
- Vizualizare rapoarte generale.
- Gestionarea decanilor și politicilor universitare.

#### Scenarii:
- Rectorul aprobă schimbări majore în structura universității.
- Rectorul exportă rapoarte globale.

### 2.5 Secretariat (Modul dedicat)
#### Scopuri:
- Administrarea studenților, profesorilor și documentelor.
- Gestionarea cererilor studenților.
- Emitere adeverințe, situații școlare.

#### Scenarii:
- Secretara adaugă un nou student.
- Secretara modifică grupa unui student.

### 2.6 Admin
#### Scopuri:
- Gestionarea utilizatorilor și rolurilor.
- Crearea facultăților, seriilor, grupelor.
- Audit și monitorizare sistem.

#### Scenarii:
- Adminul creează o nouă facultate și îi atribuie un decan.
- Adminul gestionează permisiunile sistemului.

## 3. Arhitectură Sistem
- Frontend: Blazor WebAssembly + MudBlazor
- Backend: ASP.NET Core Web API
- Database & Auth: Supabase
- Storage: Supabase Storage
- Realtime: Supabase Realtime (sincronizare prezențe, note)

## 4. Module Funcționale
- Modul Studenți (adăugare, modificare, fișe matricole)
- Modul Profesori
- Modul Facultăți / Serii / Grupe
- Modul Note
- Modul Prezențe
- Modul Orar (opțional)
- Modul Secretariat
- Modul Admin
- Modul Rapoarte
- Modul Notificări (In-App)

## 5. Flow-uri UI/UX
Vor fi descrise prin wireframes și scenarii UI bazate pe MudBlazor.

## 6. Cerințe Non-Funcționale
- **Securitate**:
    - JWT + RBAC (Role Based Access Control).
    - Audit complet pentru acțiuni critice (POST, PUT, DELETE).
- **Performanță**:
    - Timp răspuns API < 200 ms.
    - Scalabilitate prin Supabase.
- **Validare Date**:
    - **Note**: Valori întregi 1-10.
    - **Prezențe**: Statusuri valide (Prezent, Absent, Învoit).
    - **Email**: Domenii obligatorii `@stud.rau.ro` (studenți) sau `@rau.ro` (profesori/staff).
    - **CNP/ID**: Format valid.
- **Gestiune Erori**:
    - Global Exception Handler în API (ProblemDetails).
    - Global Error Boundary în Frontend.
- **Notificări**:
    - MVP: Notificări In-App (tabel `Notifications` + Realtime).
    - Future: Email (SendGrid).
- **Strategie Date**:
    - Script de Seeding pentru date inițiale (Facultăți, Useri de test).

## 7. User Stories & Acceptance Criteria
- US01: Student vizualizează note
- US02: Profesor introduce note
- US03: Secretara gestionează studenții
- US04: Admin gestionează roluri

## 8. RACI Matrix
| Activitate | Student | Profesor | Decan | Rector | Secretariat | Admin |
|-----------|---------|----------|-------|--------|-------------|-------|
| Gestionare Studenți | I | I | A | I | R | C |
| Note | I | R | A | I | C | I |

## 9. Permission Matrix
Va include permisiuni pentru fiecare endpoint / modul.

## 10. Wireframes
Vor fi definite în secțiunile următoare, cu layout MudBlazor.


---

# **11. Detaliere Extinsă – Permisiuni pe Endpoint, Fluxuri, Diagrame**

## **11.1 Permission Matrix Extinsă (Endpoint-Level RBAC)**
Permisiunile sunt definite granular, pornind de la fiecare modul și endpoint API.

### **11.1.1 Modul Studenți**
| Endpoint | Student | Profesor | Decan | Rector | Secretariat | Admin |
|---------|---------|----------|-------|--------|-------------|-------|
| GET /students | ❌ | ✔️ | ✔️ | ✔️ | ✔️ | ✔️ |
| GET /students/{id} (self) | ✔️ | ❌ | ❌ | ❌ | ❌ | ❌ |
| GET /students/{id} (others) | ❌ | ✔️ (doar grupa) | ✔️ (facultate) | ✔️ | ✔️ | ✔️ |
| POST /students | ❌ | ❌ | ❌ | ❌ | ✔️ | ✔️ |
| PUT /students/{id} | ❌ | ❌ | ❌ | ❌ | ✔️ | ✔️ |
| DELETE /students/{id} | ❌ | ❌ | ❌ | ✔️ | ❌ | ✔️ |


### **11.1.2 Modul Note**
| Endpoint | Student | Profesor | Decan | Rector | Secretariat | Admin |
|---------|---------|----------|-------|--------|-------------|-------|
| GET /grades/my | ✔️ | ❌ | ❌ | ❌ | ❌ | ❌ |
| GET /grades/group/{groupId} | ❌ | ✔️ | ✔️ | ✔️ | ✔️ | ✔️ |
| POST /grades | ❌ | ✔️ | ❌ | ❌ | ❌ | ✔️ |
| PUT /grades/{id} | ❌ | ✔️ | ✔️ (final approval) | ❌ | ❌ | ✔️ |
| DELETE /grades/{id} | ❌ | ❌ | ✔️ | ✔️ | ❌ | ✔️ |


### **11.1.3 Modul Prezențe**
| Endpoint | Student | Profesor | Decan | Rector | Secretariat | Admin |
|---------|---------|----------|-------|--------|-------------|-------|
| GET /attendance/my | ✔️ | ❌ | ❌ | ❌ | ❌ | ❌ |
| GET /attendance/group/{id} | ❌ | ✔️ | ✔️ | ✔️ | ✔️ | ✔️ |
| POST /attendance | ❌ | ✔️ | ❌ | ❌ | ❌ | ✔️ |
| PUT /attendance/{id} | ❌ | ✔️ | ✔️ | ❌ | ❌ | ✔️ |


### **11.1.4 Modul Facultăți / Serii / Grupe**
| Endpoint | Student | Profesor | Decan | Rector | Secretariat | Admin |
|---------|---------|----------|-------|--------|-------------|-------|
| GET /faculties | ✔️ | ✔️ | ✔️ | ✔️ | ✔️ | ✔️ |
| POST /faculties | ❌ | ❌ | ❌ | ✔️ | ❌ | ✔️ |
| PUT /faculties/{id} | ❌ | ❌ | ✔️ | ✔️ | ❌ | ✔️ |
| DELETE /faculties/{id} | ❌ | ❌ | ❌ | ✔️ | ❌ | ✔️ |
| GET /groups | ✔️ | ✔️ | ✔️ | ✔️ | ✔️ | ✔️ |
| POST /groups | ❌ | ❌ | ✔️ | ✔️ | ✔️ | ✔️ |
| PUT /groups/{id} | ❌ | ❌ | ✔️ | ✔️ | ✔️ | ✔️ |


---

# **11.2. Fluxuri Complete (Business-Level & UI)**

## **11.2.1 Flux – Înregistrare Note de către Profesor**
**Actor:** Profesor  
**Nivel:** Business + UI (MudBlazor)

### Pași:
1. Profesorul se autentifică în platformă.
2. Accesează meniul *„Cursurile mele”*.
3. Selectează cursul → selectează grupa.
4. Se deschide componenta MudBlazor:
   - `MudTable<StudentWithGrades>`
   - coloane: Student, Nota, Data, Tip evaluare
5. Profesorul introduce notele în câmpuri inline (MudTextField + MudSelect).
6. La salvare:
   - validator MudForm rulează
   - trimite către endpointul POST /grades în batch
7. Dacă decan approval este activ:
   - status = „pending approval”
   - decanul primește notificare
8. Notele aprobate devin vizibile studenților.

### Diagrama (descriere textuală UML sequence)
- Profesor → UI: open grade table
- UI → API: GET grades
- Profesor → UI: add grade
- UI → API: POST grade
- API → Decan: notify (if enabled)
- Decan → API: approve grade
- API → DB: update status
- Student → UI: sees grade


## **11.2.2 Flux – Gestionare Student de către Secretariat**
1. Secretara accesează meniul „Studenți”.
2. Filtrează după facultate / serie / grupă.
3. Click „Adaugă student” → formular MudDialog.
4. Completează datele + atribuirea la o grupă.
5. Sistemul creează automat contul studentului.
6. Studentul primește email cu credentiale.


## **11.2.3 Flux – Rector aprobă crearea unei noi facultăți**
1. Admin creează facultatea (pending).
2. Rector primește notificare.
3. Rector accesează secțiunea „Aprobări structură”.
4. Poate:
   - aproba
   - respinge
   - cere modificări
5. După aprobare → facultatea devine activă.


---

# **11.3 Diagrame Arhitecturale (Descrise textual pentru a fi convertite ulterior în plantUML sau draw.io)**

## **11.3.1 Component Diagram**
- **Frontend (Blazor WebAssembly)**
  - Components MudBlazor
  - Auth Service
  - API Client
  - State Containers

- **Backend (ASP.NET Core Web API)**
  - Controllers
  - Application Services
  - Domain Models
  - Mappers

- **Supabase**
  - Auth (JWT)
  - Postgres DB
  - Storage
  - Realtime Channels

## **11.3.2 Sequence Diagram – Autentificare**
1. User → UI: Enter credentials
2. UI → Supabase Auth API: Login
3. Supabase → UI: Return JWT
4. UI → Local Storage: Save JWT
5. UI → Backend API: Call endpoint with JWT
6. Backend → Supabase: Verify token
7. Backend → UI: Return data


## **11.3.3 ERD (textual)**
- Faculties (id, name, deanId)
- Programs (id, facultyId, name)
- Groups (id, programId, name)
- Students (id, groupId, email, firstName, lastName, status)
- Professors (id, facultyId, email, name)
- Courses (id, professorId, programId, name)
- Grades (id, studentId, courseId, value, date, status)
- Attendance (id, studentId, courseId, date, presence)


---

# **11.4 Detaliere UI – Componente MudBlazor utilizate**

## **11.4.1 Liste și cataloage**
- `MudTable<T>` cu:
  - Sortare
  - Pagination
  - Inline editing
  - Column templates

## **11.4.2 Formulare administrative**
- `MudForm`
- `MudTextField`
- `MudSelect`
- `MudAutocomplete`
- `MudDatePicker`
- `MudDialog`

## **11.4.3 Dashboards**
- `MudCard`
- `MudChart`
- `MudGrid`
- `MudPaper`

## **11.4.4 Modale & Flow Controls**
- `MudDialog` pentru:
  - Add student
  - Add grade
  - Approval flows


---

# **11.5 Cerințe Non-Funcționale Extinse**

## **11.5.1 Securitate**
- JWT + RBAC
- Rate limiting per IP
- Audit pentru acțiuni critice
- Access policies pe endpoint
- 2FA opțional folosind Supabase Auth

## **11.5.2 Sustenabilitate și Scalabilitate**
- Auto-scaling Supabase
- Caching cu Redis (opțional)
- API stateless

## **11.5.3 Observabilitate**
- Logging structurat (Serilog)
- Metrics (Prometheus)
- Health checks API


---

# **11.6 User Stories – Extinse**

### **US01 – Student vizualizează notele**
**Given:** Student autentificat  
**When:** Accesează „Catalogul meu”  
**Then:** Sistemul afișează notele cu filtre și detalii.

### **US02 – Profesor introduce note**
Same flow extended: validation + batch save + pending approval.

### **US03 – Secretariat modifică grupa**
Must trigger cascade updates (orar, situație școlară).

### **US10 – Rector validează departamente**
Includes audit trail.


---

# **11.7 RACI extins**

| Activitate | Student | Profesor | Decan | Rector | Secretariat | Admin |
|------------|---------|----------|-------|--------|-------------|-------|
| Admitere student | I | I | I | I | R | A |
| Adăugare note | I | R | A | I | C | C |
| Aprobare note finale | I | C | R | A | I | I |
| Modificare structură universitate | I | I | C | R | I | A |

