# Docker commands
start:
	docker-compose up -d

stop:
	docker-compose down

logs:
	docker-compose logs -f api

rebuild:
	docker-compose build api
	docker-compose up -d api

reset:
	docker-compose down -v && docker-compose up -d

# Build and Test commands
restore:
	@echo "🔄 Restoring packages..."
	@dotnet restore --no-cache

clean:
	@echo "🧹 Cleaning solution..."
	@dotnet clean
	@find . -name "bin" -type d -exec rm -rf {} + 2>/dev/null || true
	@find . -name "obj" -type d -exec rm -rf {} + 2>/dev/null || true

# Build individual projects with proper dependency order
build-projects:
	@echo "🔨 Building projects in dependency order..."
	@dotnet build AnalyticsDashboard.Core/AnalyticsDashboard.Core.csproj --no-restore --configuration Release
	@dotnet build AnalyticsDashboard.Infrastructure/AnalyticsDashboard.Infrastructure.csproj --no-restore --configuration Release
	@dotnet build AnalyticsDashboard.Api/AnalyticsDashboard.Api.csproj --no-restore --configuration Release

# Build test projects with proper dependency order
build-tests:
	@echo "🔨 Building test projects in dependency order..."
	@dotnet build AnalyticsDashboard.Core.Tests/AnalyticsDashboard.Core.Tests.csproj --no-restore --configuration Release
	@dotnet build AnalyticsDashboard.Infrastructure.Tests/AnalyticsDashboard.Infrastructure.Tests.csproj --no-restore --configuration Release
	@dotnet build AnalyticsDashboard.Api.Tests/AnalyticsDashboard.Api.Tests.csproj --no-restore --configuration Release

# Run tests with proper failure detection
test:
	@echo "🧪 Running tests..."
	@echo "  → Core tests..."
	@dotnet test AnalyticsDashboard.Core.Tests/AnalyticsDashboard.Core.Tests.csproj --no-build --configuration Release --verbosity minimal
	@echo "  → Infrastructure tests..."
	@dotnet test AnalyticsDashboard.Infrastructure.Tests/AnalyticsDashboard.Infrastructure.Tests.csproj --no-build --configuration Release --verbosity minimal
	@echo "  → API tests..."
	@dotnet test AnalyticsDashboard.Api.Tests/AnalyticsDashboard.Api.Tests.csproj --no-build --configuration Release --verbosity minimal
	@echo "✅ All tests passed!"

# Run tests with coverage
test-coverage:
	@echo "🧪 Running tests with coverage..."
	@dotnet test --configuration Release --collect:"XPlat Code Coverage" --results-directory ./TestResults

# Full CI pipeline
ci: clean restore build-projects build-tests test
	@echo "✅ All builds and tests completed successfully!"

# Pre-push validation (what you want!)
pre-push: ci
	@echo "🚀 Pre-push validation completed - ready to push!"

# Watch for changes and run tests
watch:
	@dotnet watch test --project AnalyticsDashboard.Core.Tests

# Quick validation (faster for development)
quick-check:
	@echo "⚡ Quick validation..."
	@dotnet build --configuration Debug --verbosity minimal
	@dotnet test --configuration Debug --verbosity minimal --no-build


.PHONY: start stop logs rebuild reset restore clean build-projects build-tests test test-coverage ci pre-push watch quick-check
