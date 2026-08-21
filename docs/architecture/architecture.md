# Homelab Manager Architecture

## 1. Purpose

Homelab Manager is a self-hosted infrastructure monitoring and management
platform designed for small and medium-sized homelab environments.

The primary goal is to provide a centralized view of infrastructure,
services, health status, security findings and backup status.

The initial version follows a read-only approach.

## 2. Goals

The system should provide:

- Infrastructure inventory
- Docker host and container discovery
- Proxmox integration
- Service health monitoring
- Historical health data
- Security and exposure monitoring
- Backup monitoring
- Web-based dashboard
- REST API
- Automated testing
- Containerized deployment
- CI/CD

## 3. Non-Goals of the Initial MVP

The initial MVP must not perform destructive or administrative actions.

Examples:

- Starting or stopping containers
- Updating packages
- Updating Docker images
- Modifying Proxmox resources
- Changing firewall rules
- Executing arbitrary remote commands
- Deleting files
- Triggering backups

Control functionality may be introduced in later versions after
appropriate authentication, authorization and auditing mechanisms have
been implemented.

## 4. Architecture Style

The initial system is implemented as a modular monolith.

The application is divided into logical modules with clear boundaries,
but is deployed as a small number of independently deployable components.

Microservices are intentionally avoided in the initial implementation
because the additional operational complexity is not justified by the
current requirements.

## 5. Components

### Web

The web frontend provides the user interface and communicates with the
backend through HTTP APIs.

Technology:

- Blazor
- ASP.NET Core

### API

The API provides the application boundary for the frontend and external
integrations.

Responsibilities include:

- Authentication
- Authorization
- Infrastructure queries
- Health status
- Inventory
- Security findings

Technology:

- ASP.NET Core
- REST
- OpenAPI

### Core

The Core layer contains domain models and business rules.

It must not depend on infrastructure-specific implementations.

### Infrastructure

The Infrastructure layer provides implementations for external systems
and persistence.

Initial integrations include:

- PostgreSQL
- Proxmox API
- Docker API

### Agent

A lightweight agent is planned for a later development phase.

The agent will collect information from systems where direct remote API
access is undesirable or unavailable.

The agent will initially be read-only.

## 6. Data Storage

PostgreSQL is used as the primary relational database.

The database stores:

- Hosts
- Services
- Containers
- Virtual machines
- LXC containers
- Health check results
- Security findings
- Backup information

## 7. Communication

The browser communicates with the API over HTTPS.

The API communicates with infrastructure systems through their respective
APIs.

Future agents will communicate with the central API through authenticated
and encrypted connections.

## 8. Security Principles

The application follows these principles:

- Least privilege
- Read-only access by default
- No secrets stored in source control
- Explicit authentication
- Role-based authorization
- TLS for network communication
- Auditing of administrative operations
- No arbitrary command execution through the web application

## 9. Deployment

The application is intended to run in containers.

The initial deployment consists of:

- Homelab Manager
- PostgreSQL

Docker Compose is used for local development and initial deployment.

## 10. Observability

The application itself should expose:

- Health endpoints
- Readiness information
- Structured logs
- Metrics

The monitoring stack may later integrate with Prometheus and Grafana.

## 11. Future Extensions

Potential future capabilities include:

- Controlled administrative actions
- Backup verification
- Certificate monitoring
- Update management
- Advanced security scanning
- Notification integrations
- Role-based access control
- Audit logging
- Infrastructure topology visualization
