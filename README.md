# 🛒 Skinet - ASP.NET Core Web API & Angular

**Skinet** is a comprehensive e-commerce platform built with ASP.NET Core Web API and Angular. This application provides a seamless shopping experience, integrating robust backend services with a dynamic, responsive frontend.

---

## 🚀 Features

- **Product Catalog**: Browse and search products with detailed descriptions and images.
- **Shopping Cart**: Add, update, or remove products in a user-friendly cart interface.
- **Order Processing**: Streamlined checkout process with order summaries and confirmations.
- **User Authentication**: Secure login and registration using ASP.NET Identity.
- **Payment Integration**: Process payments seamlessly with integrated payment gateways.
- **Responsive Design**: Optimized for various devices, ensuring a consistent user experience.

---

## 🛠 Tech Stack

- **Backend**: ASP.NET Core 8 Web API
- **Frontend**: Angular 17
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Identity
- **Caching**: Redis
- **API Documentation**: Swagger / OpenAPI
- **Containerization**: Docker

---

## 🏷 Setup & Installation

### 1️⃣ Prerequisites

Ensure you have the following installed:

- [.NET SDK 8](https://dotnet.microsoft.com/download)
- [Node.js (LTS version)](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis](https://redis.io/) (optional, for caching)
- [Docker](https://www.docker.com/) (optional, for containerization)

### 2️⃣ Clone the Repository

```bash
git clone https://github.com/twalker-codes/ECommerceApp.git
cd ECommerceApp
```

### 3️⃣ Backend Setup

Navigate to the API project directory:

```bash
cd src/ECommerceApp.API
```

Restore NuGet packages:

```bash
dotnet restore
```

Update the `appsettings.json` file with your database connection string and other configurations:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ECommerceDb;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "ECommerceApp",
    "Audience": "ECommerceAppUsers",
    "DurationInMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the API:

```bash
dotnet run
```

The API will be accessible at `https://localhost:5001`.

### 4️⃣ Frontend Setup

Navigate to the Angular project directory:

```bash
cd ../ECommerceApp.Client
```

Install dependencies:

```bash
npm install
```

Update the `environment.ts` file with the API base URL:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api/'
};
```

Run the Angular application:

```bash
ng serve
```

The frontend will be accessible at `http://localhost:4200`.

---

## 📝 API Endpoints

| Method | Endpoint                 | Description                       |
|--------|--------------------------|-----------------------------------|
| `GET`  | `/api/products`          | Retrieve all products             |
| `GET`  | `/api/products/{id}`     | Retrieve a specific product       |
| `POST` | `/api/cart`              | Add item to cart                  |
| `PUT`  | `/api/cart/{id}`         | Update item in cart               |
| `DELETE` | `/api/cart/{id}`       | Remove item from cart             |
| `POST` | `/api/orders`            | Place a new order                 |
| `POST` | `/api/auth/register`     | Register a new user               |
| `POST` | `/api/auth/login`        | Authenticate a user               |

For detailed API documentation, visit `https://localhost:5001/swagger`.

---

## 📦 NuGet Packages Used

| Package                               | Description                                  | NuGet URL                                                                 |
|---------------------------------------|----------------------------------------------|---------------------------------------------------------------------------|
| `Microsoft.EntityFrameworkCore`       | Entity Framework Core ORM                    | [NuGet](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)     |
| `Microsoft.AspNetCore.Identity`       | ASP.NET Core Identity for authentication     | [NuGet](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity/)     |
| `StackExchange.Redis`                 | Redis client for .NET                        | [NuGet](https://www.nuget.org/packages/StackExchange.Redis/)               |
| `Swashbuckle.AspNetCore`              | Swagger tools for API documentation          | [NuGet](https://www.nuget.org/packages/Swashbuckle.AspNetCore/)            |
| `Serilog.AspNetCore`                  | Serilog logging integration for ASP.NET Core | [NuGet](https://www.nuget.org/packages/Serilog.AspNetCore/)                |

---

## 📌 Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

---

## 🏆 License

This project is licensed under the MIT License.

---

## 📞 Contact

For any inquiries or support, please contact [your-email@example.com](mailto:your-email@example.com).

---

*Note: Replace placeholder values (e.g., `YOUR_SERVER`, `YOUR_SECRET_KEY`, `your-email@example.com`) with actual values specific to your setup.*
