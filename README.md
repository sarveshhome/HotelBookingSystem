# Hotel Booking System - Microservices with Dapr

A production-ready Hotel Booking System POC built with .NET Core, Dapr, and Clean Architecture principles.

## Architecture

### Microservices
1. **Search Hotel Service** - Hotel search and availability
2. **Room Rate Service** - Room pricing and rate calculation
3. **Booking Service** - Booking management (CQRS pattern)
4. **Payment Service** - Payment processing
5. **Email Notification Service** - Event-driven notifications
6. **Fraud Detection Service** - Transaction fraud analysis
7. **Loyalty Service** - Customer loyalty points
8. **Refund Service** - Refund processing
9. **API Gateway** - Yarp reverse proxy

### Patterns & Practices
- **Clean Architecture** - Domain, Application, Infrastructure layers
- **CQRS** - Command Query Responsibility Segregation
- **Event-Driven** - Pub/Sub with Dapr
- **Repository Pattern** - Data access abstraction
- **Result Pattern** - Error handling
- **API Gateway Pattern** - Single entry point with Yarp

### Dapr Building Blocks
- **Service Invocation** - Service-to-service calls
- **State Management** - Redis state store
- **Pub/Sub** - Event-driven messaging
- **Observability** - Zipkin tracing

## Technology Stack
- .NET 10
- Dapr 1.14
- Redis (State Store & Pub/Sub)
- Yarp (API Gateway)
- Zipkin (Distributed Tracing)

## Project Structure
```
HotelBookingSystem/
├── src/
│   ├── BuildingBlocks/
│   │   ├── Common.Domain/          # Domain entities, Result pattern
│   │   ├── Common.Application/     # CQRS interfaces, Repository
│   │   └── Common.Infrastructure/  # Dapr implementations
│   └── Services/
│       ├── SearchHotel/
│       ├── RoomRate/
│       ├── Booking/
│       ├── Payment/
│       ├── EmailNotification/
│       ├── FraudDetection/
│       ├── Loyalty/
│       ├── Refund/
│       └── ApiGateway/
├── components/                      # Dapr components
│   ├── pubsub.yaml
│   └── statestore.yaml
├── docker-compose.yml
└── run.sh
```

## Prerequisites
- .NET 10 SDK
- Dapr CLI
- Docker & Docker Compose
- Redis

## Setup & Run

### 1. Install Dapr
```bash
wget -q https://raw.githubusercontent.com/dapr/cli/master/install/install.sh -O - | /bin/bash
dapr init
```

### 2. Start Infrastructure
```bash
docker-compose up -d
```

### 3. Run All Services
```bash
./run.sh
```

### 4. Access Services
- **API Gateway**: http://localhost:5000
- **Zipkin Dashboard**: http://localhost:9411
- **Search Hotel**: http://localhost:5001
- **Room Rate**: http://localhost:5002
- **Booking**: http://localhost:5003
- **Payment**: http://localhost:5004

## API Examples

### Search Hotels
```bash
curl "http://localhost:5000/api/hotels/search?city=Mumbai&checkIn=2024-12-01&checkOut=2024-12-05&guests=2"
```

### Get Room Rates
```bash
curl "http://localhost:5000/api/rates/{hotelId}?checkIn=2024-12-01&checkOut=2024-12-05"
```

### Create Booking
```bash
curl -X POST http://localhost:5000/api/bookings \
  -H "Content-Type: application/json" \
  -d '{
    "hotelId": "guid",
    "guestName": "John Doe",
    "guestEmail": "john@example.com",
    "checkIn": "2024-12-01",
    "checkOut": "2024-12-05",
    "guests": 2,
    "totalAmount": 500
  }'
```

### Process Payment
```bash
curl -X POST http://localhost:5000/api/payments \
  -H "Content-Type: application/json" \
  -d '{
    "bookingId": "guid",
    "amount": 500,
    "paymentMethod": "CreditCard"
  }'
```

### Check Loyalty Points
```bash
curl "http://localhost:5000/api/loyalty/john@example.com/points"
```

### Process Refund
```bash
curl -X POST http://localhost:5000/api/refunds \
  -H "Content-Type: application/json" \
  -d '{
    "bookingId": "guid",
    "amount": 500
  }'
```

## Event Flow

1. **Booking Created** → Triggers:
   - Email Notification (confirmation)
   - Fraud Detection (validation)
   - Loyalty Points (accumulation)

2. **Payment Completed** → Triggers:
   - Email Notification (receipt)
   - Booking Confirmation

3. **Refund Processed** → Triggers:
   - Email Notification (refund confirmation)
   - Loyalty Points (adjustment)

## Monitoring

### Zipkin Tracing
Access distributed traces at: http://localhost:9411

### Dapr Dashboard
```bash
dapr dashboard
```

## Clean Architecture Layers

### Domain Layer
- Entities
- Domain Events
- Value Objects

### Application Layer
- Commands & Queries (CQRS)
- Handlers
- Interfaces

### Infrastructure Layer
- Dapr Event Bus
- Dapr State Store
- External Services

## Design Patterns

1. **Repository Pattern** - Data access abstraction
2. **CQRS** - Separate read/write operations
3. **Event Sourcing** - Event-driven state changes
4. **Circuit Breaker** - Dapr resiliency
5. **Saga Pattern** - Distributed transactions
6. **API Gateway** - Single entry point

## Common Services

- **State Management** - Dapr state store with Redis
- **Event Bus** - Pub/Sub messaging
- **Service Discovery** - Dapr service invocation
- **Distributed Tracing** - Zipkin integration
- **Secrets Management** - Dapr secrets API

## Development

### Add New Service
1. Create service project
2. Add Dapr.AspNetCore package
3. Configure in run.sh
4. Add route in API Gateway

### Testing
```bash
dotnet test
```

## Production Considerations

- Add authentication/authorization
- Implement rate limiting
- Add API versioning
- Configure health checks
- Set up CI/CD pipeline
- Add comprehensive logging
- Implement caching strategy
- Configure auto-scaling

## License
MIT
