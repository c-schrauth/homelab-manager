# ADR-002: Service Health Monitoring

- Status: Accepted
- Date: 2026-08-22

## Context

Homelab Manager needs to determine whether services running in the homelab are reachable and healthy.

The application will eventually monitor defferent types of services, including HTTP services, TCP endpoints and services running on Docker or Proxmos hosts.

Directly coupling the application to a specific monitoring mechanism would make the system difficult to extend and test.

## Decision

Health checking is implemented behind an abstraction.

The Core layer defines the domain model and interfaces required for health checks.

Infrastructure implementations provide the actual mechanisms used to perform health checks.

The initial implementation will support HTTP health checks.

Additional mechanisms such as TCP checks can be added later without changing the domain model.

## Consequences

### Positive

- Health checking can be tested independently of network infrastructure.
- Additional check mechanisms can be added later.
- Core remains independent from HTTP and networking implementations.
- Infrastructure-specific concerns remain outside the Core layer.

### Negative

- The initial implementation requires additional abstractions.
- The architecture is slightly more complex than directly performing HTTP requests.

## Alternatives Considered

### Direct HTTP requests from the API

Rejected because this would couple the API directly to the health-check implementation and make testing more difficult.

### Third-party monitoring system

Rejected for the initial implementation because Homelab Manager is intended to provide its own homelab-specific managment and monitoring capabilities.

