# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["EntityFrameworkLearning.Api/EntityFrameworkLearning.Api.csproj", "EntityFrameworkLearning.Api/"]
RUN dotnet restore "EntityFrameworkLearning.Api/EntityFrameworkLearning.Api.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "EntityFrameworkLearning.Api/EntityFrameworkLearning.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "EntityFrameworkLearning.Api/EntityFrameworkLearning.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "EntityFrameworkLearning.Api.dll"]
