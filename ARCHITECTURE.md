# Architecture Documentation

## System Architecture

### High-Level Architecture
```
┌─────────────┐
│   Client    │
└──────┬──────┘
       │
       ▼
┌─────────────────────────────────────────────────────────┐
│              API Gateway (Yarp)                         │
│              Port: 5000                                 │
└─────────────────────────────────────────────────────────┘
       │
       ├──────────────────┬──────────────────┬─────────────┐
       ▼                  ▼                  ▼             ▼
┌──────────────┐   ┌──────────────┐   ┌──────────────┐   ...
│Search Hotel  │   │  Room Rate   │   │   Booking    │
│   Service    │   │   Service    │   │   Service    │
└──────────────┘   └──────────────┘   └──────┬───────┘
                                              │
                                              ▼
                                    ┌──────────────────┐
                                    │  Dapr Pub/Sub    │
                                    │     (Redis)      │
                                    └──────────────────┘
                                              │
                    ┌─────────────────────────┼─────────────────────┐
                    ▼                         ▼                     ▼
            ┌──────────────┐         ┌──────────────┐      ┌──────────────┐
            │   Payment    │         │    Email     │      │    Fraud     │
            │   Service    │         │Notification  │      │  Detection   │
            └──────────────┘         └──────────────┘      └──────────────┘
```

## Clean Architecture Layers

### 1. Domain Layer (Core)
- **Entities**: Business objects with identity
- **Value Objects**: Immutable objects without identity
- **Domain Events**: Events that occur in the domain
- **Interfaces**: Repository contracts

**Example**: `BookingEntity`, `Hotel`, `PaymentEntity`

### 2. Application Layer
- **Commands**: Write operations (CQRS)
- **Queries**: Read operations (CQRS)
- **Handlers**: Business logic execution
- **DTOs**: Data transfer objects

**Example**: `CreateBookingCommand`, `SearchHotelsQuery`

### 3. Infrastructure Layer
- **Dapr Integration**: Event bus, state store
- **External Services**: Third-party APIs
- **Persistence**: Database implementations

**Example**: `DaprEventBus`, `DaprStateStore`

### 4. Presentation Layer (API)
- **Controllers**: HTTP endpoints
- **Middleware**: Cross-cutting concerns
- **Configuration**: Service setup

## Design Patterns

### 1. CQRS (Command Query Responsibility Segregation)
```csharp
// Command - Write
public record CreateBookingCommand(...)
public class CreateBookingHandler { }

// Query - Read
public record SearchHotelsQuery(...)
public class SearchHotelsHandler { }
```

### 2. Repository Pattern
```csharp
public interface IRepository<T>
{
    Task<T> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
}
```

### 3. Result Pattern
```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }
}
```

### 4. Event-Driven Architecture
```csharp
// Publish event
await _daprClient.PublishEventAsync("pubsub", "booking-created", bookingEvent);

// Subscribe to event
[Topic("pubsub", "booking-created")]
public async Task OnBookingCreated(BookingEvent event) { }
```

### 5. API Gateway Pattern
- Single entry point for all clients
- Request routing to microservices
- Cross-cutting concerns (auth, logging)

### 6. Saga Pattern (Distributed Transactions)
```
Booking Created → Payment Processing → Email Notification
     ↓                    ↓                    ↓
  Success            Success/Fail         Confirmation
     ↓                    ↓                    ↓
  Confirm            Rollback if fail     Update Loyalty
```

## Dapr Building Blocks

### 1. Service Invocation
```csharp
var response = await _daprClient.InvokeMethodAsync<Response>(
    "payment-service", 
    "process-payment", 
    request);
```

### 2. State Management
```csharp
// Save state
await _daprClient.SaveStateAsync("statestore", "key", value);

// Get state
var value = await _daprClient.GetStateAsync<T>("statestore", "key");
```

### 3. Pub/Sub Messaging
```csharp
// Publish
await _daprClient.PublishEventAsync("pubsub", "topic", data);

// Subscribe
[Topic("pubsub", "topic")]
public async Task HandleEvent(EventData data) { }
```

### 4. Observability
- Automatic distributed tracing with Zipkin
- Metrics collection
- Logging integration

## Service Communication

### Synchronous (Service Invocation)
- API Gateway → Services
- Service → Service (when immediate response needed)

### Asynchronous (Pub/Sub)
- Booking → Email Notification
- Booking → Fraud Detection
- Booking → Loyalty Service
- Payment → Email Notification

## Data Management

### State Store (Redis)
- Booking data
- Payment records
- Loyalty points
- Refund information

### Event Store (Pub/Sub)
- booking-created
- payment-completed
- refund-processed
- booking-cancelled

## Security Considerations

### Current Implementation (POC)
- No authentication/authorization
- No encryption
- No rate limiting

### Production Requirements
1. **Authentication**: JWT tokens, OAuth2
2. **Authorization**: Role-based access control
3. **Encryption**: TLS/SSL, data encryption at rest
4. **Rate Limiting**: API throttling
5. **API Keys**: Service-to-service authentication
6. **Secrets Management**: Dapr secrets API

## Scalability

### Horizontal Scaling
- Each service can scale independently
- Stateless services (state in Redis)
- Load balancing via API Gateway

### Performance Optimization
- Caching with Redis
- Async processing with Pub/Sub
- Database read replicas
- CDN for static content

## Monitoring & Observability

### Distributed Tracing (Zipkin)
- Request flow across services
- Performance bottlenecks
- Error tracking

### Metrics
- Request count
- Response time
- Error rate
- Resource utilization

### Logging
- Structured logging
- Centralized log aggregation
- Log correlation with trace IDs

## Resilience Patterns

### Circuit Breaker (Dapr)
```yaml
apiVersion: dapr.io/v1alpha1
kind: Configuration
metadata:
  name: resiliency
spec:
  policies:
    circuitBreaker:
      maxRequests: 1
      timeout: 30s
```

### Retry Policy
```yaml
retries:
  maxAttempts: 3
  backoff:
    initialInterval: 1s
    maxInterval: 10s
```

### Timeout
```yaml
timeout: 5s
```

## Testing Strategy

### Unit Tests
- Domain logic
- Handlers
- Services

### Integration Tests
- API endpoints
- Dapr components
- Database operations

### End-to-End Tests
- Complete booking flow
- Event propagation
- Error scenarios

## Deployment

### Local Development
- Docker Compose for infrastructure
- Dapr CLI for services
- Hot reload with dotnet watch

### Production (Kubernetes)
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: booking-service
spec:
  replicas: 3
  template:
    metadata:
      annotations:
        dapr.io/enabled: "true"
        dapr.io/app-id: "booking"
```

## Future Enhancements

1. **Authentication & Authorization**
2. **API Versioning**
3. **GraphQL Gateway**
4. **Service Mesh (Istio)**
5. **Event Sourcing**
6. **CQRS with separate read/write databases**
7. **Real-time notifications (SignalR)**
8. **Advanced fraud detection (ML)**
9. **Multi-tenancy support**
10. **Internationalization**
