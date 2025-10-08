# 🖼️ Online-Images-Hub

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-blue.svg)](https://docs.microsoft.com/en-us/aspnet/core/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-green.svg)](https://docs.microsoft.com/en-us/ef/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red.svg)](https://www.microsoft.com/en-us/sql-server)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen.svg)](https://github.com/yourusername/Online-Images-Hub)
[![Stars](https://img.shields.io/github/stars/yourusername/Online-Images-Hub?style=social)](https://github.com/yourusername/Online-Images-Hub)

### Professional Online Photo Printing System

Online-Images-Hub is a comprehensive web-based platform that enables users to upload, customize, and order high-quality printed photos online. Built with modern ASP.NET Core MVC architecture, it features secure payment processing, admin management, and a responsive user interface.

---

## ✨ Features

### 👤 User Features
- **🔐 Secure Authentication**: User registration and login with strong password validation
- **📸 Photo Upload**: Easy JPEG photo uploads with file validation
- **📏 Custom Print Sizes**: Multiple print size options (4x6, 5x7, and more)
- **🛒 Order Management**: Add multiple photos to cart with quantity selection
- **💰 Automatic Pricing**: Real-time price calculation based on size and quantity
- **💳 Secure Payments**: 
  - Encrypted credit card processing using ASP.NET Data Protection
  - Alternative branch payment option
- **📦 Order Tracking**: View order history and current status
- **🏠 Address Management**: Shipping address configuration

### 🛠️ Admin Features
- **👨‍💼 Admin Dashboard**: Comprehensive admin panel for order management
- **📊 Order Processing**: View, update, and manage customer orders
- **⚙️ Price Management**: Configure print sizes and pricing
- **👥 User Management**: Monitor and manage user accounts
- **📈 Order Analytics**: Track order statuses and customer data

---

## 🏗️ Technology Stack

| Layer | Technology | Version |
|-------|------------|---------|
| **Frontend** | HTML5, CSS3, Bootstrap 5, JavaScript, jQuery | Latest |
| **Backend** | ASP.NET Core MVC | 8.0 |
| **Database** | SQL Server with Entity Framework Core | 9.0 |
| **Security** | ASP.NET Data Protection, Password Hashing | Built-in |
| **Authentication** | Session-based authentication | Custom |
| **File Handling** | IFormFile with validation | Built-in |

---

## 🚀 Quick Start

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/Online-Images-Hub.git
   cd Online-Images-Hub
   ```

2. **Configure the database**
   - Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "AppDbContext": "Server=(localdb)\\mssqllocaldb;Database=OnlineImagesHubDb;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Build and run the application**
   ```bash
   dotnet build
   dotnet run
   ```

5. **Access the application**
   - Navigate to `https://localhost:5001` in your browser
   - Register a new user account or use admin credentials

---

## 📖 Usage Examples

### For Users

1. **Register & Login**
   ```csharp
   // Navigate to /Users/Register
   // Fill in: Full Name, Email, Password, Confirm Password
   ```

2. **Upload Photos**
   ```csharp
   // Navigate to /Orders/Create
   // Upload JPEG files (max size validation included)
   // Select print sizes and quantities
   ```

3. **Place Order**
   ```csharp
   // Choose payment method (Credit Card or Branch Payment)
   // Enter shipping address
   // Complete secure checkout
   ```

### For Administrators

1. **Access Admin Panel**
   ```csharp
   // Navigate to /Admins/Login
   // Use admin credentials
   ```

2. **Manage Orders**
   ```csharp
   // View all orders in /Admins/Orders
   // Update order status (Pending, Processing, Shipped, Delivered)
   ```

3. **Configure Pricing**
   ```csharp
   // Manage print sizes and prices
   // Update pricing in real-time
   ```

---

## 🏛️ Project Structure

```
Online-Images-Hub/
├── Controllers/
│   ├── HomeController.cs          # Landing page and navigation
│   ├── UsersController.cs         # User registration, login, profile
│   ├── OrdersController.cs        # Order creation and management
│   └── AdminsController.cs        # Admin panel functionality
├── Models/
│   ├── User.cs                    # User entity with validation
│   ├── Order.cs                   # Order entity with relationships
│   ├── PhotoOrderItem.cs          # Individual photo order items
│   ├── PrintSizePrice.cs          # Print size and pricing configuration
│   └── Admin.cs                   # Admin user entity
├── Services/
│   └── CreditCardProtector.cs     # Credit card encryption service
├── Views/
│   ├── Home/                      # Landing page views
│   ├── Users/                     # User authentication views
│   ├── Orders/                    # Order management views
│   └── Admins/                    # Admin panel views
├── Data/
│   └── AppDbContext.cs            # Entity Framework context
└── wwwroot/
    └── MyImages/                  # Static assets and uploads
```

---

## 🔒 Security Features

- **Password Security**: Strong password requirements with hashing
- **Credit Card Encryption**: AES encryption using ASP.NET Data Protection
- **Input Validation**: Comprehensive server-side validation
- **Session Management**: Secure session-based authentication
- **File Upload Security**: File type and size validation
- **SQL Injection Protection**: Entity Framework parameterized queries

---

## 🧪 Testing

```bash
# Run the application in development mode
dotnet run --environment Development

# Run with specific configuration
dotnet run --configuration Release
```

### Test Scenarios
- User registration and login
- Photo upload with various file types
- Order creation with different payment methods
- Admin order management
- Price calculation accuracy

---

## 🤝 Contributing

We welcome contributions to PhotoPrintHub! Please follow these guidelines:

### Getting Started
1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Make your changes and add tests if applicable
4. Commit your changes: `git commit -m 'Add amazing feature'`
5. Push to the branch: `git push origin feature/amazing-feature`
6. Open a Pull Request

### Code Standards
- Follow C# coding conventions
- Add XML documentation for public methods
- Include unit tests for new features
- Ensure all tests pass before submitting PR

### Issue Reporting
- Use the issue tracker for bug reports and feature requests
- Provide detailed reproduction steps for bugs
- Include screenshots for UI-related issues

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👥 Authors

- **Jawad ul bahar** - *Web Apps Developer* - [GitHub](https://github.com/Jawad-Ul-Bahar)

---

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- Bootstrap team for the responsive UI components
- Entity Framework team for the ORM capabilities
- All contributors who help improve this project

---

## 📞 Support

If you have any questions or need help:

- 📧 Email: jawadulbahar@gmail.com.com
- 🐛 Issues: [GitHub Issues](https://github.com/Jawad-Ul-Bahar/Online-Images-Hub/issues)
- 📖 Documentation: [Wiki](https://github.com/Jawad-Ul-Bahar/Online-Images-Hub/wiki)

---

<div align="center">

**⭐ Star this repository if you found it helpful!**

Made with ❤️ using ASP.NET Core

</div>
