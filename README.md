# Hotel Management System

A comprehensive hotel management system built with ASP.NET Core MVC that helps manage rooms, bookings, guests, and payments efficiently.

## Features

- **Room Management**: Add, edit, and manage hotel rooms with different room types and capacities
- **Guest Management**: Maintain guest information and booking history
- **Booking System**: Create and manage guest bookings with check-in/check-out dates
- **Payment Processing**: Track payments and generate invoices
- **Dashboard**: View key statistics and metrics at a glance
- **Room Status Tracking**: Monitor room availability (Available, Occupied, Maintenance, Reserved)

## Technology Stack

- **Framework**: ASP.NET Core 6.0 MVC
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Frontend**: Bootstrap 5, HTML5, CSS3, JavaScript
- **Language**: C#

## Prerequisites

- .NET 6.0 SDK or higher
- SQL Server 2019 or higher
- Visual Studio 2022 (recommended) or Visual Studio Code
- Git

## Installation & Setup

### 1. Clone the Repository

```bash
git clone https://github.com/bsef22m003-maker/hotel-management-system.git
cd hotel-management-system
```

### 2. Configure Database Connection

Edit `appsettings.json` and update the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=HotelManagementDB;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

### 3. Install Dependencies

```bash
dotnet restore
```

### 4. Apply Database Migrations

```bash
dotnet ef database update
```

This will create the database schema and seed initial data.

### 5. Run the Application

```bash
dotnet run
```

The application will be available at `https://localhost:7000` (or the port specified in your configuration).

## Project Structure

```
hotel-management-system/
├── Controllers/
│   ├── RoomController.cs
│   ├── BookingController.cs
│   ├── GuestController.cs
│   ├── PaymentController.cs
│   └── DashboardController.cs
├── Models/
│   ├── Room.cs
│   ├── Booking.cs
│   ├── Guest.cs
│   ├── Payment.cs
│   └── RoomType.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Repository/
│       ├── IRepository.cs
│       ├── RoomRepository.cs
│       ├── BookingRepository.cs
│       └── ...
├── Views/
│   ├── Room/
│   ├── Booking/
│   ├── Guest/
│   ├── Payment/
│   └── Dashboard/
└── wwwroot/
    ├── css/
    ├── js/
    └── images/
```

## Usage Guide

### Room Management

1. Navigate to **Rooms** section from the main menu
2. Click **Add New Room** to create a new room
3. Fill in room details (number, type, capacity, price, status)
4. Click **Create** to save

### Guest Management

1. Go to **Guests** section
2. Click **Add New Guest** to register a new guest
3. Enter guest information (name, email, phone, address)
4. Click **Create** to save

### Create a Booking

1. Navigate to **Bookings** section
2. Click **New Booking**
3. Select guest and room
4. Enter check-in and check-out dates
5. Review the total price
6. Click **Create Booking**

### Process Payment

1. Go to **Payments** section or from a booking
2. Click **New Payment**
3. Select payment method (Credit Card, Debit Card, Cash, etc.)
4. Enter transaction details
5. Click **Process Payment**

### View Dashboard

- The **Dashboard** shows key metrics:
  - Total rooms and available rooms
  - Total guests
  - Pending bookings
  - Total revenue

## Database Schema

### Tables

- **Rooms**: Store room information
- **RoomTypes**: Room type categories
- **Guests**: Guest information
- **Bookings**: Booking records with dates and prices
- **Payments**: Payment transactions
- **Users**: System users (optional authentication)

## API Endpoints (if applicable)

### Room Endpoints
- `GET /api/rooms` - List all rooms
- `GET /api/rooms/{id}` - Get room details
- `POST /api/rooms` - Create new room
- `PUT /api/rooms/{id}` - Update room
- `DELETE /api/rooms/{id}` - Delete room

### Booking Endpoints
- `GET /api/bookings` - List all bookings
- `GET /api/bookings/{id}` - Get booking details
- `POST /api/bookings` - Create new booking
- `PUT /api/bookings/{id}` - Update booking
- `DELETE /api/bookings/{id}` - Cancel booking

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- Ensure database user has proper permissions

### Migration Errors
```bash
dotnet ef migrations remove
dotnet ef database drop
dotnet ef database update
```

### Port Already in Use
Change the port in `launchSettings.json` or use:
```bash
dotnet run --urls "https://localhost:8080"
```

## Contributing

1. Create a feature branch (`git checkout -b feature/AmazingFeature`)
2. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
3. Push to the branch (`git push origin feature/AmazingFeature`)
4. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contact

**Developer**: Umair Ashraf  
**Email**: bsef22m003@pucit.edu.pk  
**GitHub**: [bsef22m003-maker](https://github.com/bsef22m003-maker)

## Support

For issues, questions, or suggestions, please open an issue on GitHub or contact the developer.

---

**Last Updated**: June 2026  
**Version**: 1.0.0
