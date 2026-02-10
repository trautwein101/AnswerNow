Write-Host "Starting local AnswerNow environment..." -ForegroundColor Green

# Start Postgres (docker compose)
docker compose up -d

# Apply EF Core migrations (Data project, using Api as startup)
dotnet ef database update `
  --project .\AnswerNow.Data\AnswerNow.Data.csproj `
  --startup-project .\AnswerNow.Api\AnswerNow.Api.csproj

# Run the API
dotnet run --project .\AnswerNow.Api --launch-profile https
