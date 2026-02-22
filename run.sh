#!/bin/bash

echo "Starting Hotel Booking System with Dapr..."

# Start infrastructure
docker-compose up -d

# Wait for Redis
sleep 5

# Start services with Dapr
dapr run --app-id searchhotel --app-port 5001 --dapr-http-port 3501 --components-path ./components -- dotnet run --project src/Services/SearchHotel/SearchHotel.API &

dapr run --app-id roomrate --app-port 5002 --dapr-http-port 3502 --components-path ./components -- dotnet run --project src/Services/RoomRate/RoomRate.API &

dapr run --app-id booking --app-port 5003 --dapr-http-port 3503 --components-path ./components -- dotnet run --project src/Services/Booking/Booking.API &

dapr run --app-id payment --app-port 5004 --dapr-http-port 3504 --components-path ./components -- dotnet run --project src/Services/Payment/Payment.API &

dapr run --app-id emailnotification --app-port 5005 --dapr-http-port 3505 --components-path ./components -- dotnet run --project src/Services/EmailNotification/EmailNotification.API &

dapr run --app-id frauddetection --app-port 5006 --dapr-http-port 3506 --components-path ./components -- dotnet run --project src/Services/FraudDetection/FraudDetection.API &

dapr run --app-id loyalty --app-port 5007 --dapr-http-port 3507 --components-path ./components -- dotnet run --project src/Services/Loyalty/Loyalty.API &

dapr run --app-id refund --app-port 5008 --dapr-http-port 3508 --components-path ./components -- dotnet run --project src/Services/Refund/Refund.API &

# Start API Gateway
dotnet run --project src/Services/ApiGateway/ApiGateway &

echo "All services started!"
echo "API Gateway: http://localhost:5000"
echo "Zipkin Dashboard: http://localhost:9411"

wait
