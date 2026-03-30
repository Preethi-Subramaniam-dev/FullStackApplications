Asset Management System
📌 Overview
This project is a full-stack Asset Management System designed to manage corporate hardware (e.g., laptops, monitors) and associated software licenses.
It is built with a stateless and scalable architecture using ASP.NET Core and Angular, and fully containerized with Docker.

🚀 Features
Asset management (CRUD operations)
Employee management with asset assignment
One-to-one Warranty Card per Asset
Many-to-many Software License management
Asset status tracking (Available, Assigned, Retired)
Filtering support for efficient data retrieval
RESTful APIs using DTOs to avoid circular references
Swagger API documentation

🛠️ Tech Stack
Backend
ASP.NET Core Web API 
Entity Framework Core ( provide your sql connection details to connect to the db in appSettings.json file)
SQL Server (Populate the database with the provided AssetManagementDB.bak file committed in the repository)
Swagger (OpenAPI)
Logging (Seri Log) —> Log files added to Log Folder
Frontend
Angular
RxJS
🧩 Architecture
Stateless REST API design
Frontend communicates via HTTP APIs
No server-side session storage
Scalable via Docker containers

🗄️ Database Design
Relationships
Asset → WarrantyCard: One-to-One
Employee → Assets: One-to-Many
Assets ↔ SoftwareLicenses: Many-to-Many
Asset → Status: Many-to-One
Entity Diagram (Conceptual)
Employee (1) ────< Assets >──── (M) SoftwareLicenses
                  │
                  │ (1:1)
                  ▼
             WarrantyCard

Assets ──── (M:1) ──── Status
Status Values
Available
Assigned
Retired

🔌 API Endpoints
Method
Endpoint
Description


GET
/api/all
Get all assets (with filtering)


GET
/api/{id}
Get asset by ID


POST
/api/add
Create new asset


PUT
/api/edit/{id}
Update asset


PUT
/api/assign/{assetId}
Assign asset to employee



















































Full API documentation available via Swagger.

⚙️ Setup Instructions
1. Clone Repository
git clone https://github.com/Preethi-Subramaniam-dev/FullStackApplications.git
cd asset-management-system

2. Run with Docker
docker-compose up --build
Services Started:
Backend API
Angular Frontend
SQL Server Database

3. Apply EF Core Migrations
dotnet ef database update (use the already available migration file of name 20260329202927_InitialDbSetup.cs in the Migration folder)

📘 API Documentation
Swagger UI available at:
https://localhost:<port>/swagger
All endpoints include XML documentation (///)

💻 Frontend
Built with Angular
Uses RxJS for async data handling
JSDoc added for services
Features:
Search & filter assets
View details
Edit forms

📦 Dockerization
Fully containerized system
Uses docker-compose for orchestration
Ensures consistent environment across development and deployment

🧪 Assumptions
Each asset must have exactly one warranty card
Software licenses can be shared across assets
Employees can have multiple assets
Status values are predefined
Started building application with assumption data

📈 Future Improvements
Role-based access control (RBAC)
Pagination support
Advanced filtering & search
Logging & monitoring
Unit and integration tests

📌 Notes
This solution was implemented independently based on the provided requirements.
Where requirements were ambiguous, reasonable assumptions were made.

👤 Author
Preethi Subramaniam



