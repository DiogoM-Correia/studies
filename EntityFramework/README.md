# Entity Framework Learning API

A comprehensive .NET 9.0 Web API demonstrating Entity Framework Core concepts including CRUD operations, relationships, and DTOs.

## Features

- **Entity Framework Core** with PostgreSQL
- **Minimal API** endpoints with clean architecture
- **Swagger/OpenAPI** documentation
- **Docker** support for easy deployment
- **DTOs** for data transfer
- **Extension methods** for clean service configuration

## Prerequisites

- Docker Desktop
- .NET 9.0 SDK (for local development)

## Quick Start with Docker

### 1. Clone the repository
```bash
git clone <your-repo-url>
cd EntityFramework
```

### 2. Run with Docker Compose
```bash
docker-compose up --build
```

This will:
- Start PostgreSQL database on port 5433
- Build and start the API on port 5000
- Create the database automatically
- Set up networking between services

### 3. Access the application
- **API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **Database**: localhost:5433 (postgres/postgres)

## API Endpoints

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

### Categories
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `GET /api/categories/{id}/products` - Get products by category
- `POST /api/categories` - Create new category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category

## Development

### Local Development (without Docker)
1. Install .NET 9.0 SDK
2. Install PostgreSQL
3. Update connection string in `appsettings.json`
4. Run the application:
   ```bash
   cd EntityFrameworkLearning.Api
   dotnet run
   ```

### Docker Commands

```bash
# Build and start all services
docker-compose up --build

# Start in background
docker-compose up -d

# View logs
docker-compose logs -f api

# Stop all services
docker-compose down

# Stop and remove volumes
docker-compose down -v

# Rebuild specific service
docker-compose build api
```

## Project Structure

```
EntityFramework/
├── EntityFrameworkLearning.Api/     # .NET API project
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── DTOs/
│   │   ├── CategoryDto.cs
│   │   ├── ProductDto.cs
│   │   └── ...
│   ├── Endpoints/
│   │   ├── CategoryEndpoints.cs
│   │   └── ProductEndpoints.cs
│   ├── Extensions/
│   │   ├── ServiceCollectionExtensions.cs
│   │   └── WebApplicationExtensions.cs
│   ├── Models/
│   │   ├── Category.cs
│   │   └── Product.cs
│   └── ...
├── Dockerfile                       # Root Dockerfile
├── .dockerignore                    # Root .dockerignore
├── docker-compose.yml              # Docker Compose configuration
└── README.md
```

## Database Schema

### Categories
- Id (int, primary key)
- Name (string)
- Description (string, nullable)

### Products
- Id (int, primary key)
- Name (string)
- Description (string, nullable)
- Price (decimal)
- CategoryId (int, foreign key)
- Category (navigation property)

## Troubleshooting

### Common Issues

1. **Database connection fails**
   - Ensure PostgreSQL container is running: `docker-compose ps`
   - Check logs: `docker-compose logs postgres`

2. **API won't start**
   - Check if database is ready: `docker-compose logs api`
   - Verify connection string in docker-compose.yml

3. **Port conflicts**
   - Change ports in docker-compose.yml if 5000 or 5433 are in use

### Useful Commands

```bash
# Check container status
docker-compose ps

# View all logs
docker-compose logs

# Access PostgreSQL directly
docker exec -it ef-learning-postgres psql -U postgres -d EntityFrameworkLearning

# Restart specific service
docker-compose restart api
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test with Docker
5. Submit a pull request

## License

This project is for educational purposes. 