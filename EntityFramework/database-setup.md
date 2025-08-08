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
