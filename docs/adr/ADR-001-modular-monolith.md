# ADR-001: Use a Modular Monolith

- Status: Accepted
- Date: 2026-08-21

## Context

Homelab Manager needs to integrate several infrastructure systems such as
Docker, Proxmox, PostgreSQL and later dedicated monitoring agents.

A microservice architecture could separate these responsibilities into
multiple independently deployed services.

However, the initial system is primarily intended for a single homelab
environment and does not currently require independent scaling or
independent deployment of individual business capabilities.

Operating multiple microservices would introduce additional complexity
including:

- Service discovery
- Multiple deployments
- Inter-service authentication
- Distributed logging
- Distributed tracing
- Network failure handling
- Multiple container lifecycles
- Increased operational overhead

## Decision

Homelab Manager will initially be implemented as a modular monolith.

The application will contain clearly separated logical modules and
architectural layers while remaining relatively simple to deploy.

The main boundaries are:

- Web
- API
- Core
- Infrastructure

External infrastructure integrations will be encapsulated behind
interfaces.

A dedicated agent may be introduced later when direct access to an
infrastructure system is undesirable or unavailable.

## Consequences

### Positive

- Simple deployment
- Simple local development
- Lower operational overhead
- Clear architectural boundaries
- Easy debugging
- Suitable for a single homelab
- Components can be extracted later if required

### Negative

- Modules are deployed together
- Independent scaling is not possible
- Architectural discipline is required to prevent unwanted coupling

## Alternatives Considered

### Microservices

Rejected for the initial implementation because the operational
complexity is not justified by the current requirements.

### Single-layer application

Rejected because it would make the system harder to maintain and would
create strong coupling between the UI, business logic, persistence and
external integrations.
