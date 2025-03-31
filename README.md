# FaceItRadar Environment Setup

## Setup Steps

- Run `dotnet restore` in the backend directory to ensure environment files work with the backend

## Required Environment Files

### Root Directory

Create `.env` file in project root with:

- `ENV=development`
- `POSTGRES_USER=your_db_username`
- `POSTGRES_PASSWORD=your_db_password`
- `POSTGRES_DB=your_database_name`
- `POSTGRES_PORT=port_number`
- `BACKEND_PORT=port_number`

### Backend Directory

Create `.env` file in `/backend` with:

- `ASPNETCORE_ENVIRONMENT=Development`
- `FACEIT_API_KEY=your_faceit_api_key_here`
- `LOG_LEVEL=Debug`

### DB Directory

Create `.env` file in `/db` with:

- `PGDATA=/var/lib/postgresql/data/pgdata`

## Important Notes

- Don't rename files - must be exactly `.env`
- Don't commit sensitive data to git
- Debugging: run `docker compose config`

## Database Access

```bash
# Connect to PostgreSQL in Docker container
docker exec -it faceitradar-db psql -U <db_username> -d <database_name>

# Or use this one-liner to run a query
docker exec -it faceitradar-db psql -U <db_username> -d <database_name> -c "SELECT * FROM table_name;"
```

## pgAdmin Access

Access pgAdmin at http://localhost:5050 after starting containers.

1. Login with credentials from root `.env` file
2. Navigate database structure:
   - Servers → YourServer → Databases → YourDB → Schemas → public → Tables
3. View table data: Right-click table → View/Edit Data → All Rows
4. Run SQL queries: Tools → Query Tool

## Database Migrations

This project uses Entity Framework Core for database migrations with helper scripts to avoid hardcoding connection strings.

### Using Migration Scripts

Two bash scripts are available in the `backend/Scripts` directory:

1. **Create a new migration:**

   ```bash
   ./add-migration.sh YourMigrationName
   ```

2. **Apply migrations to the database:**
   ```bash
   ./apply-migration.sh
   ```

Make sure your `.env` files are properly configured before running these scripts.
