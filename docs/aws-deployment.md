# AWS Deployment Guide

AnswerNow infrastructure is deployed using a mix of:

- **AWS SAM** for the serverless backend (API Gateway HTTP API + Lambda + IAM permissions)
- **CloudFormation (AWS CLI)** for supporting stacks (database/networking/operations)

This document captures the commands and workflow used for **DEV / QA / PROD**.

---

## Prerequisites

- AWS CLI installed
- AWS SAM CLI installed
- AWS credentials configured
- Route53 hosted zone for `answernowplace.com`

---

## Regional Architecture

The deployment intentionally uses two AWS regions.

### us-west-2 (Oregon) 

Backend infrastructure:

- API Gateway (HTTP API)
- Lambda (.NET 8)
- RDS PostgreSQL
- AWS Secrets Manager
- CloudWatch monitoring
- SAM deployments

### us-east-1 (N. Virginia)

Frontend edge infrastructure:

- ACM certificate for `answernowplace.com`
- CloudFront distribution

CloudFront requires ACM certificates to be created in **us-east-1**, which is why the frontend certificate resides there.

---

## Verification

Confirm the required tools and credentials are available.

aws --version
sam --version
aws sts get-caller-identity

---

# Validation

Before deployment, validate the SAM template.

sam validate -t template-prod.yaml

---

# SAM Deploy Example (DEV)

This deploys the backend serverless stack (API Gateway + Lambda).

sam deploy \
  --template-file template-dev.yaml \
  --stack-name answernow-backend-dev \
  --capabilities CAPABILITY_IAM \
  --region us-west-2 \
  --parameter-overrides \
    DbSecretArn="<DEV_DB_SECRET_ARN>" \
    JwtSecretArn="<DEV_JWT_SECRET_ARN>"

---

# CloudFormation Deploy Example (Supporting Stacks)

Supporting stacks such as database, networking, and operations monitoring are deployed using the AWS CLI.

aws cloudformation deploy \
  --template-file <stack-template>.yaml \
  --stack-name <stack-name> \
  --capabilities CAPABILITY_NAMED_IAM \
  --region us-west-2 \
  --parameter-overrides Key=Value AnotherKey=Value

---

# Deployment Notes

- Secrets are stored in AWS Secrets Manager (database connection string and JWT signing key).
- The Lambda execution role is granted least-privilege access to retrieve only the required secrets.
- CloudFormation templates pass Secret ARNs as parameters; Lambda retrieves the secret values at runtime.
- EF Core migrations run automatically in DEV (environment-controlled).
- Monitoring and budget alarms are deployed through the operations stack.



