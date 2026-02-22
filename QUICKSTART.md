# Quick Start Guide

## 1. Prerequisites Check
```bash
# Check .NET version
dotnet --version  # Should be 10.0 or higher

# Check Dapr
dapr --version

# Check Docker
docker --version
```

## 2. Install Dapr (if not installed)
```bash
wget -q https://raw.githubusercontent.com/dapr/cli/master/install/install.sh -O - | /bin/bash
dapr init
```

## 3. Start the System
```bash
cd /Users/sarveshkumar/Practice/NetCore/HotelBookingSystem

# Start Redis and Zipkin
docker-compose up -d

# Wait 5 seconds for Redis to be ready
sleep 5

# Start all microservices
./run.sh
```

## 4. Test the System

### Test 1: Search Hotels
```bash
curl "http://localhost:5000/api/hotels/search?city=Mumbai&checkIn=2024-12-01&checkOut=2024-12-05&guests=2"
```

### Test 2: Get Room Rates
```bash
# Replace {hotelId} with ID from search results
curl "http://localhost:5000/api/rates/{hotelId}?checkIn=2024-12-01&checkOut=2024-12-05"
```

### Test 3: Create Booking (triggers events)
```bash
curl -X POST http://localhost:5000/api/bookings \
  -H "Content-Type: application/json" \
  -d '{
    "hotelId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "guestName": "John Doe",
    "guestEmail": "john@example.com",
    "checkIn": "2024-12-01T00:00:00Z",
    "checkOut": "2024-12-05T00:00:00Z",
    "guests": 2,
    "totalAmount": 500
  }'
```

### Test 4: Check Loyalty Points
```bash
curl "http://localhost:5000/api/loyalty/john@example.com/points"
```

## 5. Monitor the System

### View Distributed Traces
Open browser: http://localhost:9411

### View Dapr Dashboard
```bash
dapr dashboard
```
Open browser: http://localhost:8080

## 6. Stop the System
```bash
# Stop all Dapr services
dapr stop --app-id searchhotel
dapr stop --app-id roomrate
dapr stop --app-id booking
dapr stop --app-id payment
dapr stop --app-id emailnotification
dapr stop --app-id frauddetection
dapr stop --app-id loyalty
dapr stop --app-id refund

# Stop Docker containers
docker-compose down
```

## Troubleshooting

### Port Already in Use
```bash
# Find and kill process using port
lsof -ti:5000 | xargs kill -9
```

### Redis Connection Issues
```bash
# Restart Redis
docker-compose restart redis
```

### Dapr Not Working
```bash
# Reinitialize Dapr
dapr uninstall
dapr init
```

## Service Ports

| Service | App Port | Dapr Port |
|---------|----------|-----------|
| Search Hotel | 5001 | 3501 |
| Room Rate | 5002 | 3502 |
| Booking | 5003 | 3503 |
| Payment | 5004 | 3504 |
| Email Notification | 5005 | 3505 |
| Fraud Detection | 5006 | 3506 |
| Loyalty | 5007 | 3507 |
| Refund | 5008 | 3508 |
| API Gateway | 5000 | - |

## Event Flow Example

When you create a booking:
1. Booking service saves state
2. Publishes "booking-created" event
3. Email service sends confirmation
4. Fraud service validates transaction
5. Loyalty service adds points
6. All traced in Zipkin
