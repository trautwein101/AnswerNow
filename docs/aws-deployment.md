# AWS Deployment Guide

AnswerNow infrastructure is deployed using a mix of:
- **AWS SAM** (serverless backend)
- **CloudFormation via AWS CLI** (supporting stacks)

This doc captures the commands and deployment workflow used for DEV/QA/PROD.

---

# Prerequisites

- AWS CLI installed
- AWS SAM CLI installed
- AWS credentials configured
- Route53 hosted zone
- ACM certificate
- Region set to `us-west-2` for backend deployments

---

## Verifaction Tools:

```bash
aws --version
sam --version

#Verify Credentials:

-> aws sts get-caller-identity

#Validate Templates:


-> sam validate -t template-prod.yaml


##  SAM Deploy Example (DEV)

-> sam deploy \
  --template-file template-dev.yaml \
  --stack-name answernow-backend-dev \
  --capabilities CAPABILITY_IAM

##  CloudFormation via AWS CLI (DEV)

-> aws cloudformation deploy \
  --template-file ---deprecated.yaml \
  --stack-name ---deprecated \
  --capabilities CAPABILITY_NAMED_IAM---deprecated \
  --region us-west-2 \
  --parameter-overrides Key=Value AnotherKey=Value

## Deployment Notes
- JWT secrets are injected via environment variables.
- Database connection strings are injected via environment variables (not included in repo).
- EF Core migrations run automatically in DEV.
- Monitoring and budget alarms are deployed through the operations stack.


