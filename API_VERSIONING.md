# API Versioning

API versioning has been implemented across all microservices using URL path versioning.

## Configuration

- **Default Version**: v1.0
- **Versioning Strategy**: URL Path (`/api/v{version}/...`)
- **Package**: Asp.Versioning.Http 8.1.0

## Versioned Endpoints

### Search Hotel Service
```
GET /api/v1/hotels/search?city=Mumbai&checkIn=2024-12-01&checkOut=2024-12-05&guests=2
```

### Booking Service
```
POST /api/v1/bookings
GET  /api/v1/bookings/{id}
```

### Payment Service
```
POST /api/v1/payments
```

### API Gateway Routes
All routes support versioning:
```
/api/v1/hotels/{**catch-all}
/api/v1/bookings/{**catch-all}
/api/v1/payments/{**catch-all}
/api/v1/rates/{**catch-all}
/api/v1/loyalty/{**catch-all}
/api/v1/refunds/{**catch-all}
```

## Features

- **Default Version**: Requests without version use v1.0
- **Version Reporting**: API version included in response headers
- **Swagger Support**: API Explorer integration for versioned documentation

## Usage Examples

```bash
# Using v1 explicitly
curl "http://localhost:5000/api/v1/hotels/search?city=Mumbai&checkIn=2024-12-01&checkOut=2024-12-05&guests=2"

# Create booking with v1
curl -X POST http://localhost:5000/api/v1/bookings \
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

## Adding New Versions

To add v2:

1. **Controller Level**:
```csharp
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BookingsV2Controller : ControllerBase
{
    // v2 implementation
}
```

2. **Action Level**:
```csharp
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BookingsController : ControllerBase
{
    [HttpGet, MapToApiVersion("1.0")]
    public IActionResult GetV1() { }
    
    [HttpGet, MapToApiVersion("2.0")]
    public IActionResult GetV2() { }
}
```
