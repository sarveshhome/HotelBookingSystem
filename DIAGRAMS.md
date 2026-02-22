# Hotel Booking System - Visual Architecture

## System Overview Diagram

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              CLIENT LAYER                                    │
│                    (Web, Mobile, Desktop Applications)                       │
└────────────────────────────────┬────────────────────────────────────────────┘
                                 │
                                 │ HTTP/HTTPS
                                 ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                          API GATEWAY (Yarp)                                  │
│                            Port: 5000                                        │
│  ┌──────────────────────────────────────────────────────────────────────┐  │
│  │  Routes: /api/hotels, /api/rates, /api/bookings, /api/payments      │  │
│  │  Features: Load Balancing, Routing, CORS                             │  │
│  └──────────────────────────────────────────────────────────────────────┘  │
└────────────────────────────────┬────────────────────────────────────────────┘
                                 │
        ┌────────────────────────┼────────────────────────┐
        │                        │                        │
        ▼                        ▼                        ▼
┌──────────────┐        ┌──────────────┐        ┌──────────────┐
│ Search Hotel │        │  Room Rate   │        │   Booking    │
│   Service    │        │   Service    │        │   Service    │
│  Port: 5001  │        │  Port: 5002  │        │  Port: 5003  │
│              │        │              │        │              │
│ - Search     │        │ - Get Rates  │        │ - Create     │
│ - Filter     │        │ - Calculate  │        │ - Get        │
│ - Availability│       │ - Pricing    │        │ - Update     │
└──────────────┘        └──────────────┘        └──────┬───────┘
                                                       │
                                                       │ Publish Events
                                                       ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                        DAPR PUB/SUB (Redis)                                  │
│                                                                              │
│  Topics:                                                                     │
│  • booking-created                                                           │
│  • payment-completed                                                         │
│  • refund-processed                                                          │
│  • booking-cancelled                                                         │
└────────────────────────────────┬────────────────────────────────────────────┘
                                 │
        ┌────────────────────────┼────────────────────────┐
        │                        │                        │
        ▼                        ▼                        ▼
┌──────────────┐        ┌──────────────┐        ┌──────────────┐
│   Payment    │        │    Email     │        │    Fraud     │
│   Service    │        │Notification  │        │  Detection   │
│  Port: 5004  │        │   Service    │        │   Service    │
│              │        │  Port: 5005  │        │  Port: 5006  │
│ - Process    │        │              │        │              │
│ - Validate   │        │ - Send Email │        │ - Validate   │
│ - Confirm    │        │ - Templates  │        │ - Score      │
└──────────────┘        └──────────────┘        └──────────────┘

        ┌────────────────────────┼────────────────────────┐
        │                        │                        │
        ▼                        ▼                        ▼
┌──────────────┐        ┌──────────────┐        ┌──────────────┐
│   Loyalty    │        │    Refund    │        │              │
│   Service    │        │   Service    │        │              │
│  Port: 5007  │        │  Port: 5008  │        │              │
│              │        │              │        │              │
│ - Points     │        │ - Process    │        │              │
│ - Rewards    │        │ - Validate   │        │              │
│ - Tiers      │        │ - Confirm    │        │              │
└──────────────┘        └──────────────┘        └──────────────┘

                                 │
                                 ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                      DAPR STATE STORE (Redis)                                │
│                                                                              │
│  Stores:                                                                     │
│  • Bookings                                                                  │
│  • Payments                                                                  │
│  • Loyalty Points                                                            │
│  • Refunds                                                                   │
└─────────────────────────────────────────────────────────────────────────────┘

                                 │
                                 ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                    MONITORING & OBSERVABILITY                                │
│                                                                              │
│  ┌──────────────────┐              ┌──────────────────┐                    │
│  │  Zipkin Tracing  │              │  Dapr Dashboard  │                    │
│  │  Port: 9411      │              │  Port: 8080      │                    │
│  └──────────────────┘              └──────────────────┘                    │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Clean Architecture Layers

```
┌─────────────────────────────────────────────────────────────────┐
│                      PRESENTATION LAYER                          │
│                    (Controllers, API Endpoints)                  │
│                                                                  │
│  • HotelsController                                             │
│  • BookingsController                                           │
│  • PaymentsController                                           │
└────────────────────────────┬─────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                     APPLICATION LAYER                            │
│              (Commands, Queries, Handlers, DTOs)                │
│                                                                  │
│  • CreateBookingCommand / Handler                               │
│  • SearchHotelsQuery / Handler                                  │
│  • ProcessPaymentCommand / Handler                              │
└────────────────────────────┬─────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                       DOMAIN LAYER                               │
│              (Entities, Value Objects, Events)                  │
│                                                                  │
│  • BookingEntity                                                │
│  • Hotel                                                        │
│  • PaymentEntity                                                │
│  • BookingCreatedEvent                                          │
└────────────────────────────┬─────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                   INFRASTRUCTURE LAYER                           │
│         (Dapr, External Services, Persistence)                  │
│                                                                  │
│  • DaprEventBus                                                 │
│  • DaprStateStore                                               │
│  • EmailService                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## Event Flow Diagram

```
┌──────────────┐
│   Client     │
└──────┬───────┘
       │
       │ POST /api/bookings
       ▼
┌──────────────────┐
│  API Gateway     │
└──────┬───────────┘
       │
       ▼
┌──────────────────────────────────────────────────────────────┐
│  Booking Service                                             │
│                                                              │
│  1. Validate request                                         │
│  2. Create booking entity                                    │
│  3. Save to state store                                      │
│  4. Publish "booking-created" event                          │
└──────┬───────────────────────────────────────────────────────┘
       │
       │ Event: booking-created
       │
       ├─────────────────┬─────────────────┬─────────────────┐
       │                 │                 │                 │
       ▼                 ▼                 ▼                 ▼
┌─────────────┐   ┌─────────────┐   ┌─────────────┐   ┌─────────────┐
│   Email     │   │   Fraud     │   │  Loyalty    │   │  Payment    │
│Notification │   │ Detection   │   │  Service    │   │  Service    │
│             │   │             │   │             │   │             │
│ Send        │   │ Validate    │   │ Add Points  │   │ Process     │
│ Confirmation│   │ Transaction │   │             │   │ Payment     │
└─────────────┘   └─────────────┘   └─────────────┘   └──────┬──────┘
                                                              │
                                                              │
                                    Event: payment-completed  │
                                                              │
                                                              ▼
                                                    ┌─────────────────┐
                                                    │     Email       │
                                                    │  Notification   │
                                                    │                 │
                                                    │  Send Receipt   │
                                                    └─────────────────┘
```

## Service Dependencies

```
API Gateway
    ├── Search Hotel Service (Independent)
    ├── Room Rate Service (Independent)
    ├── Booking Service
    │   ├── Dapr State Store
    │   └── Dapr Pub/Sub
    ├── Payment Service
    │   ├── Dapr State Store
    │   └── Dapr Pub/Sub
    ├── Email Notification Service
    │   └── Dapr Pub/Sub (Subscriber)
    ├── Fraud Detection Service
    │   └── Dapr Pub/Sub (Subscriber)
    ├── Loyalty Service
    │   ├── Dapr State Store
    │   └── Dapr Pub/Sub (Subscriber)
    └── Refund Service
        ├── Dapr State Store
        └── Dapr Pub/Sub
```

## Data Flow

```
1. Search Flow
   Client → API Gateway → Search Hotel Service → Response

2. Booking Flow
   Client → API Gateway → Booking Service → State Store
                                          → Pub/Sub → Subscribers

3. Payment Flow
   Client → API Gateway → Payment Service → State Store
                                          → Pub/Sub → Email Service

4. Loyalty Flow
   Booking Event → Loyalty Service → State Store → Points Updated

5. Refund Flow
   Client → API Gateway → Refund Service → State Store
                                         → Pub/Sub → Email Service
```

## Technology Stack Visualization

```
┌─────────────────────────────────────────────────────────────┐
│                    Application Layer                         │
│                      .NET 10 / C#                           │
└─────────────────────────────┬───────────────────────────────┘
                              │
┌─────────────────────────────┴───────────────────────────────┐
│                    Dapr Runtime Layer                        │
│                      Dapr 1.14                              │
│                                                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐    │
│  │Service Invoke│  │  State Mgmt  │  │   Pub/Sub    │    │
│  └──────────────┘  └──────────────┘  └──────────────┘    │
└─────────────────────────────┬───────────────────────────────┘
                              │
┌─────────────────────────────┴───────────────────────────────┐
│                  Infrastructure Layer                        │
│                                                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐    │
│  │    Redis     │  │    Zipkin    │  │    Docker    │    │
│  │ State/PubSub │  │   Tracing    │  │  Containers  │    │
│  └──────────────┘  └──────────────┘  └──────────────┘    │
└─────────────────────────────────────────────────────────────┘
```
