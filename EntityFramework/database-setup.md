# Database Setup Guide

## PostgreSQL with Docker

This project now uses PostgreSQL instead of SQLite for a more production-ready experience.

## Quick Start

### 1. Start PostgreSQL with Docker Compose
```bash
docker-compose up -d
```

### 2. Verify PostgreSQL is running
```bash
docker ps
```
You should see the `ef-learning-postgres` container running.

### 3. Run the API
```bash
cd EntityFrameworkLearning.Api
dotnet run
```

The database will be automatically created with seed data when you first run the application.

## Database Management

### Connect to PostgreSQL
```bash
# Using Docker
docker exec -it ef-learning-postgres psql -U postgres -d EntityFrameworkLearning

# Using psql (if installed locally)
psql -h localhost -p 5432 -U postgres -d EntityFrameworkLearning
```

### Stop PostgreSQL
```bash
docker-compose down
```

### Stop and remove all data
```bash
docker-compose down -v
```

### View logs
```bash
docker-compose logs postgres
```

## Connection String Details

- **Host**: localhost
- **Port**: 5432
- **Database**: EntityFrameworkLearning
- **Username**: postgres
- **Password**: postgres

## Benefits of PostgreSQL over SQLite

1. **Production Ready**: Used in real-world applications
2. **Better Performance**: Optimized for concurrent access
3. **Advanced Features**: JSON support, full-text search, etc.
4. **Scalability**: Can handle large datasets
5. **ACID Compliance**: Full transaction support
6. **Multi-user**: Supports multiple concurrent connections

## Troubleshooting

### Port already in use
If port 5432 is already in use, change the port in `docker-compose.yml`:
```yaml
ports:
  - "5433:5432"  # Use 5433 instead of 5432
```

Then update `appsettings.json`:
```json
"DefaultConnection": "Host=localhost;Port=5433;Database=EntityFrameworkLearning;Username=postgres;Password=postgres"
```

### Connection refused
Make sure Docker is running and the container is started:
```bash
docker-compose up -d
docker ps
```

### Database doesn't exist
The database will be created automatically when you run the application for the first time. 