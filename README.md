
The project follows a layered architecture separating API, business logic, and data access concerns.

---

## Architecture Overview

### Local Development
Angular SPA → .NET API → PostgreSQL (Docker)

### AWS Production Deployment
CloudFront → API Gateway (HTTP API) → Lambda (.NET 8) → RDS PostgreSQL

The AWS environment includes:

- Custom domain (Route53 + ACM)
- CloudWatch alarms
- SNS notifications (email)
- Budget monitoring
- Environment-based configuration (DEV / QA / PROD)

For a full technical breakdown, see:

➡️ **docs/architecture.md**

---

## Key Features

- Layered clean architecture
- JWT-based authentication
- Environment-specific configuration
- Automatic EF Core migrations (DEV + cloud environments)
- Health check endpoints
- Serverless AWS deployment
- Cost-conscious cloud design decisions
