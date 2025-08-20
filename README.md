# 🎯 Dynamic QR Code System

> **Revolutionize your QR code experience with dynamic content management**

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-orange.svg)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## 🌟 Overview

The **Dynamic QR Code System** is a cutting-edge solution that transforms static QR codes into powerful, flexible content delivery tools. Unlike traditional QR codes that point to fixed URLs, our system allows you to generate a QR code once and dynamically change its content without ever needing to regenerate the code itself.

### ✨ Key Features

- **🔄 Dynamic Content Management**: Change QR code content instantly without regenerating codes
- **📱 Multi-Format Support**: Deliver URLs, images, and custom content
- **🔐 Secure Authentication**: Built-in user management and access control
- **📊 Real-time Updates**: Content changes take effect immediately
- **🎨 Modern Web Interface**: Beautiful Blazor-based admin panel
- **🚀 RESTful API**: Comprehensive API for integration
- **📈 Scalable Architecture**: Clean architecture with microservices support

## 🏗️ Architecture

This project follows **Clean Architecture** principles with a modular design:

```
📁 QShirt/
├── 🏛️ 01_Core/
│   ├── QShirt.Domain/          # Domain entities and business logic
│   ├── QShirt.Application/     # Application services and use cases
│   ├── QShirt.Infrastructure/  # External services and implementations
│   └── QShirt.Persistence/     # Data access and database layer
├── 🌐 02_Public/
│   ├── QShirt.Public.Client/   # Blazor WebAssembly frontend
│   ├── QShirt.Public.Server/   # gRPC server for public access
│   ├── QShirt.Public.Application/ # Public application logic
│   └── QShirt.Public.Proxy/    # Client-side proxy generation
└── 🔌 03_WebApi/
    ├── QShirt.WebApi/          # REST API endpoints
    └── QShirt.WebApi.Application/ # Web API application logic
```

## 🚀 How It Works

### 1. **QR Code Generation**
- Generate a unique QR code that points to your system
- Each QR code gets a unique identifier in the database
- The QR code URL remains static: `https://yourdomain.com/content/get?id={unique-id}`

### 2. **Content Management**
- Upload images, set URLs, or configure custom content
- Content is stored in the database linked to the QR code identifier
- Multiple content types supported per QR code

### 3. **Dynamic Delivery**
- When someone scans the QR code, the system looks up the current content
- Content is served dynamically based on what you've configured
- Changes take effect immediately without QR code regeneration

### 4. **Content Types Supported**
- **🔗 URLs**: Redirect users to any website
- **🖼️ Images**: Display custom images (JPEG, PNG)
- **📝 Custom Content**: Text, descriptions, and metadata

## 🛠️ Technology Stack

- **Backend**: .NET 8, ASP.NET Core, Entity Framework Core
- **Frontend**: Blazor WebAssembly, Bootstrap
- **Database**: SQL Server (with Entity Framework migrations)
- **API**: RESTful APIs with versioning
- **Communication**: gRPC for high-performance services
- **Architecture**: Clean Architecture with CQRS pattern
- **Dependency Injection**: Autofac
- **Code Generation**: Custom proxy generator for type safety

## 📋 Prerequisites

- .NET 8.0 SDK or later
- SQL Server (LocalDB, Express, or full version)
- Visual Studio 2022 or VS Code
- Git

## 🚀 Getting Started

### 1. **Clone the Repository**
```bash
git clone https://github.com/yourusername/qrcode-dynamic-system.git
cd qrcode-dynamic-system
```

### 2. **Database Setup**
```bash
# Update connection string in appsettings.json
# Run Entity Framework migrations
dotnet ef database update --project QShirt.Persistence
```

### 3. **Build and Run**
```bash
# Build the solution
dotnet build

# Run the Web API
dotnet run --project QShirt.WebApi

# Run the Public Client (Blazor)
dotnet run --project QShirt.Public.Client

# Run the Public Server (gRPC)
dotnet run --project QShirt.Public.Server
```

### 4. **Access the Application**
- **Web API**: `https://localhost:7001`
- **Blazor Client**: `https://localhost:7002`
- **gRPC Server**: `https://localhost:7003`

## 📖 Usage Examples

### Creating a Dynamic QR Code

1. **Generate QR Code**: Create a QR code pointing to your system
2. **Configure Content**: Set the content you want to display
3. **Share QR Code**: Distribute the QR code to your audience
4. **Update Content**: Change content anytime without touching the QR code

### Content Management

```csharp
// Example: Adding customer content
var command = new AddCustomerContentCommand
{
    CustomerId = customerId,
    Title = "Welcome to Our Store",
    Description = "Special offer inside!",
    Url = "https://yourstore.com/special-offer",
    IsMain = true
};

await command.Execute();
```

### API Endpoints

```http
GET /v1/content/get?id={guid}
POST /v1/content/add
PUT /v1/content/update
DELETE /v1/content/delete
```

## 🔧 Configuration

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=QShirtDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Authentication Settings
```json
{
  "AuthOptions": {
    "AdminAuthOptions": {
      "SecretKey": "your-secret-key-here"
    }
  }
}
```

## 🏛️ Project Structure Details

### Core Layer
- **Domain**: Business entities (`Customer`, `CustomerContent`, `File`)
- **Application**: Use cases, commands, queries, and business logic
- **Infrastructure**: External services (email, SMS, payments, file downloads)
- **Persistence**: Data access, Entity Framework configurations

### Public Layer
- **Client**: Blazor WebAssembly frontend for content management
- **Server**: gRPC server for high-performance content delivery
- **Application**: Public-facing business logic
- **Proxy**: Auto-generated client proxies for type safety

### Web API Layer
- **WebApi**: REST API controllers and endpoints
- **Application**: Web API specific business logic and models

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- **Documentation**: Check the project wiki for detailed guides
- **Issues**: Report bugs and request features via GitHub Issues
- **Discussions**: Join community discussions for help and ideas

## 🎯 Roadmap

- [ ] **Analytics Dashboard**: Track QR code scans and user engagement
- [ ] **A/B Testing**: Test different content variations
- [ ] **Scheduled Content**: Automatically change content based on time/date
- [ ] **Geolocation Targeting**: Serve different content based on location
- [ ] **Mobile App**: Native mobile applications for content management
- [ ] **Advanced Analytics**: Heat maps, user behavior tracking
- [ ] **API Rate Limiting**: Enhanced security and performance
- [ ] **Multi-language Support**: Internationalization features

---

**Built with ❤️ using .NET 8 and Clean Architecture**

*Transform your QR codes from static links to dynamic content powerhouses!*
