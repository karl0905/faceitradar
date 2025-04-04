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
- `PGADMIN_DEFAULT_EMAIL=pgadmin_email`
- `PGADMIN_DEFAULT_PASSWORD=pgadmin_password`
- `PGADMIN_PORT=pgadmin_port`

### Backend Directory

Create `.env` file in `/backend` with:

- `ASPNETCORE_ENVIRONMENT=Development`
- `FACEIT_API_KEY=your_faceit_api_key_here`
- `LOG_LEVEL=Debug`
- `DB_PORT=5432`
- `DB_NAME=your_database_name`
- `DB_USER=your_db_username`
- `DB_PASSWORD=your_db_password`

Note: The application automatically determines the database host based on the environment:

- In Docker: Uses "postgres" (container name) - Docker sets `DOTNET_RUNNING_IN_CONTAINER=true` automatically
- Locally: Uses "localhost" for development

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
