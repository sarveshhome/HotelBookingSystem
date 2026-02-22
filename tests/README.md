# Hotel Booking System - Unit Tests

Unit tests for the Hotel Booking System microservices.

## Test Projects

### 1. Common.Tests
Tests for shared building blocks (Result pattern, domain entities).

**Run:**
```bash
dotnet test tests/Common.Tests/Common.Tests.csproj
```

### 2. Booking.Tests
Tests for Booking service handlers and domain entities.

**Coverage:**
- CreateBookingHandler (command handling, state management, event publishing)
- BookingEntity (domain logic, status transitions)

**Run:**
```bash
dotnet test tests/Services/Booking.Tests/Booking.Tests.csproj
```

### 3. Payment.Tests
Tests for Payment service handlers.

**Coverage:**
- ProcessPaymentHandler (payment processing, state management, events)
- Multiple payment methods validation

**Run:**
```bash
dotnet test tests/Services/Payment.Tests/Payment.Tests.csproj
```

### 4. SearchHotel.Tests
Tests for Hotel Search service handlers.

**Coverage:**
- SearchHotelsHandler (hotel search logic, filtering)
- Hotel data validation

**Run:**
```bash
dotnet test tests/Services/SearchHotel.Tests/SearchHotel.Tests.csproj
```

## Run All Tests

```bash
dotnet test
```

## Test Framework

- **xUnit** - Test framework
- **Moq** - Mocking library for Dapr dependencies
- **FluentAssertions** - Assertion library

## Test Patterns

### Handler Tests
- Verify command/query handling
- Mock Dapr client interactions
- Validate state management
- Verify event publishing

### Domain Tests
- Entity validation
- Business logic
- Status transitions

## Example Test

```csharp
[Fact]
public async Task Handle_ValidCommand_CreatesBooking()
{
    var command = new CreateBookingCommand(
        Guid.NewGuid(), "John Doe", "john@example.com",
        DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(5), 2, 500m);

    var result = await _handler.Handle(command);

    result.Should().NotBeNull();
    result.Status.Should().Be(BookingStatus.Pending);
}
```
