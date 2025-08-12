# Entity Framework Learning - Unit Tests

This project contains comprehensive unit tests for the Entity Framework Learning API.

## Test Structure

### Models Tests
- **CategoryTests.cs** - Tests for the Category entity model
- **ProductTests.cs** - Tests for the Product entity model

### DTOs Tests
- **DtoTests.cs** - Tests for all DTO classes (CategoryDto, ProductDto, etc.)
- **EntityExtensionsTests.cs** - Tests for DTO conversion extension methods

### Data Tests
- **ApplicationDbContextTests.cs** - Tests for database context, seeding, and relationships

### Endpoints Tests
- **ProductEndpointsTests.cs** - Tests for all product CRUD operations
- **CategoryEndpointsTests.cs** - Tests for all category CRUD operations

### Test Configuration
- **TestConfiguration.cs** - Common test utilities and configuration

## Test Features

### ✅ Test Isolation
- Each test uses a unique in-memory database (GUID-based names)
- No test data interference between tests
- Clean state for each test execution

### ✅ Comprehensive Coverage
- Model validation attributes
- Entity relationships
- CRUD operations
- Error handling
- Edge cases
- Data integrity

### ✅ Modern Testing Stack
- **xUnit** - Testing framework
- **FluentAssertions** - Readable assertions
- **Entity Framework In-Memory** - Fast, isolated database testing
- **No Moq** - Avoids security vulnerabilities

## Running Tests

### Prerequisites
- .NET 9.0 SDK
- Entity Framework Core tools

### Run All Tests
```bash
# From the solution root
dotnet test

# From the test project directory
cd EntityFrameworkLearning.Tests
dotnet test
```

### Run Tests with Verbose Output
```bash
dotnet test --verbosity normal
```

### Run Specific Test Class
```bash
dotnet test --filter "FullyQualifiedName~CategoryTests"
```

### Run Tests with Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Test Categories

### Unit Tests
- **Fast execution** (< 1 second total)
- **No external dependencies** (in-memory database)
- **Isolated** (each test is independent)
- **Deterministic** (same result every time)

### Integration Tests
- **Database operations** (CRUD, relationships)
- **Entity Framework features** (seeding, migrations)
- **Data validation** (constraints, foreign keys)

## Test Data

### Seed Data
Tests use the same seed data as the main application:
- **4 Categories**: Electronics, Books, Clothing, Home & Garden
- **5 Products**: Laptop, Smartphone, Programming Book, T-Shirt, Garden Tool Set

### Test Data Isolation
- Each test creates its own in-memory database
- Seed data is recreated for each test
- No data persists between tests

## Best Practices Used

### 1. Arrange-Act-Assert Pattern
```csharp
[Fact]
public async Task TestName_ShouldDoSomething_WhenCondition()
{
    // Arrange
    var context = await CreateContextWithSeedDataAsync();
    
    // Act
    var result = await context.Entities.ToListAsync();
    
    // Assert
    result.Should().HaveCount(5);
}
```

### 2. Descriptive Test Names
- Format: `MethodName_ShouldReturnResult_WhenCondition`
- Clear about what is being tested
- Indicates expected behavior and conditions

### 3. Test Data Management
- Unique database names prevent conflicts
- Proper disposal of database contexts
- Clean state for each test

### 4. Comprehensive Assertions
- Use FluentAssertions for readable assertions
- Test both positive and negative cases
- Verify edge cases and error conditions

## Adding New Tests

### 1. Follow Naming Convention
```csharp
public async Task MethodName_ShouldReturnResult_WhenCondition()
```

### 2. Use Test Utilities
```csharp
// For tests needing seed data
using var context = await TestConfiguration.CreateContextWithSeedDataAsync();

// For tests without seed data
using var context = TestConfiguration.CreateContextWithoutSeedData();
```

### 3. Test Both Success and Failure Cases
```csharp
[Fact]
public async Task GetProduct_ShouldReturnProduct_WhenProductExists()
[Fact]
public async Task GetProduct_ShouldReturnNull_WhenProductDoesNotExist()
```

### 4. Use Theory for Parameterized Tests
```csharp
[Theory]
[InlineData(0.0)]
[InlineData(100.0)]
[InlineData(999.99)]
public void Product_ShouldAcceptValidPriceRange(decimal price)
```

## Troubleshooting

### Common Issues

#### 1. Test Database Conflicts
- Ensure each test uses unique database names
- Use `Guid.NewGuid().ToString()` for database names

#### 2. Async Test Issues
- Always use `async Task` for async tests
- Use `await` for async operations
- Don't use `async void`

#### 3. Test Isolation Problems
- Check that each test creates its own context
- Verify proper disposal of resources
- Use `using` statements for context management

### Debugging Tests
```bash
# Run tests in debug mode
dotnet test --logger "console;verbosity=detailed"

# Run specific test with debug output
dotnet test --filter "FullyQualifiedName~SpecificTestName" --verbosity detailed
```

## Performance

### Test Execution Time
- **Total test suite**: < 2 seconds
- **Individual tests**: < 100ms each
- **Database operations**: < 50ms each

### Memory Usage
- **In-memory databases**: Lightweight and fast
- **No disk I/O**: All operations in memory
- **Automatic cleanup**: Resources disposed after each test

## Continuous Integration

### GitHub Actions
Tests are automatically run on:
- Pull requests
- Push to main branch
- Scheduled runs

### Build Pipeline
```yaml
- name: Run Tests
  run: dotnet test --verbosity normal --collect:"XPlat Code Coverage"
```

## Contributing

When adding new tests:
1. Follow existing naming conventions
2. Use the provided test utilities
3. Ensure test isolation
4. Add comprehensive assertions
5. Test both success and failure scenarios
6. Update this README if adding new test categories
