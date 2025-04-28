# 🚗 Car Rental System

A modern **Car Rental** management system built using **ASP.NET Core MVC** and **Microsoft SQL Server**.  
Manage cars and rentals easily with **Admin** and **User** roles.

![Admin](https://img.shields.io/badge/Admin-Full%20Control-blue?style=for-the-badge&logo=windows&logoColor=white)
![User](https://img.shields.io/badge/User-Rent%20Cars-green?style=for-the-badge&logo=person&logoColor=white)

![.NET Core](https://img.shields.io/badge/.NET%20Core-9.0-blueviolet?logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-red?logo=microsoftsqlserver&logoColor=white)

---

## ✨ Features

### 🛠️ Admin

- ➕ Add new cars.
- ✏️ Edit car details.
- 🗑️ Delete cars.
- 📋 View all rented cars.
- 🚫 Cancel an active rental.

### 👤 User

- 🔍 Browse available cars.
- 🛒 Rent a car.
- 📄 View their rented cars.

---

## 🏗️ Tech Stack

- **Framework:** ASP.NET Core MVC
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **Frontend:** Razor Views (HTML, CSS, Bootstrap)

---

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK

- Microsoft SQL Server

- IDE: Visual Studio 2022+

### Installation

#### Clone the repository

```bash
git clone https://github.com/Kareem-Mohamed-Wardany/CarRental.git
```

```bash
cd CarRental
```

#### Configure database connection In appsettings.json:

```json
"ConnectionStrings": {
"DB": "Data Source=SERVERNAME;Initial Catalog=CarRental;Integrated Security=True; TrustServerCertificate=true;"
}
```

#### Apply migrations

```bash
dotnet ef database update
Run the project
```

```bash
dotnet run
Access the app
Visit: https://localhost:5001 or http://localhost:5000
```

## 🔒 Roles & Permissions

#### Role Permissions

- Admin Full control over cars and rentals
- User Rent cars and view own rentals

## 🌟 Future Enhancements

- ✉️ Email notifications.
- 💳 Payment gateway integration.

- 📅 Car availability calendar.

- ⭐ User reviews and ratings.
