# Asset Management System

A full-stack asset management application built with **ASP.NET Core** (backend) and **Angular** (frontend), backed by **SQL Server**.

---

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running
- Docker Compose v2 (bundled with Docker Desktop)

---

## Project Structure

```
FullStackApplications/
├── docker-compose.yml               # Root compose file (run from here)
├── AssetManagementSystem/           # ASP.NET Core Web API
│   └── Dockerfile
└── AssetManagementSystem-Client/    # Angular frontend
    └── Dockerfile
```

---

## Configuration

Before running, update the SA password in `docker-compose.yml` to a secure value:

```yaml
SA_PASSWORD: "YourStrong!Passw0rd"   # Change this
```

Make sure the same password is used in both places:
- `db` service → `SA_PASSWORD`
- `db` service → healthcheck `-P` argument
- `backend` service → `ConnectionStrings__AssetManagementDBCOnnection`

---

## Running the Application

### 1. Build and start all services

From the `FullStackApplications/` root directory:

```bash
docker compose up --build
```

This will:
1. Build the **backend** image from `AssetManagementSystem/Dockerfile`
2. Build the **frontend** image from `AssetManagementSystem-Client/Dockerfile`
3. Pull the **SQL Server 2022** image
4. Start all three containers in dependency order (db → backend → frontend)

### 2. Run in detached (background) mode

```bash
docker compose up --build -d
```

### 3. Access the application

| Service  | URL                        |
|----------|----------------------------|
| Frontend | http://localhost:4200       |
| Backend API | http://localhost:5000    |
| SQL Server  | localhost,1433           |

---

## Useful Commands

### View running containers
```bash
docker compose ps
```

### View logs for all services
```bash
docker compose logs -f
```

### View logs for a specific service
```bash
docker compose logs -f backend
docker compose logs -f frontend
docker compose logs -f db
```

### Stop all services
```bash
docker compose down
```

### Stop and remove volumes (deletes database data)
```bash
docker compose down -v
```

### Rebuild a single service without cache
```bash
docker compose build --no-cache backend
docker compose build --no-cache frontend
```

### Restart a single service
```bash
docker compose restart backend
```

---

## Database Migrations

Entity Framework migrations run automatically on application startup. If you need to apply them manually, exec into the backend container:

```bash
docker exec -it asset-management-system-backend sh
```

---

## Troubleshooting

### Backend fails to connect to the database
The SQL Server container needs time to initialize. The `healthcheck` in `docker-compose.yml` ensures the backend waits until the DB is ready. If the backend still fails, increase `start_period` in the `db` healthcheck:

```yaml
healthcheck:
  start_period: 60s   # increase if needed
```

### Port conflicts
If ports `4200`, `5000`, or `1433` are already in use, change the host-side port in `docker-compose.yml`:

```yaml
ports:
  - "4201:80"    # change 4200 to any free port
```

### Viewing SQL Server data locally
Connect with any SQL client (e.g., Azure Data Studio, SSMS) using:
- **Server:** `localhost,1433`
- **Authentication:** SQL Login
- **Username:** `sa`
- **Password:** *(your configured SA password)*
