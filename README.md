# BankingApp

A modern full-stack banking application built with ASP.NET Core and React.

## 🌟 Overview

BankingApp is a comprehensive online banking solution that allows users to manage accounts, transfer funds, pay bills, and monitor transaction history. This project demonstrates a fully-functional banking system with secure authentication, real-time balance updates, and intuitive user interface.

## 🛠️ Technology Stack

### Backend
- **Framework**: ASP.NET Core 9.0
- **ORM**: Entity Framework Core 9.0
- **Database**: PostgreSQL
- **Authentication**: JWT (JSON Web Tokens)
- **Deployment**: Azure App Service

### Frontend
- **Framework**: React 18
- **Build Tool**: Vite
- **Styling**: Tailwind CSS
- **State Management**: React Context API
- **Routing**: React Router 6
- **HTTP Client**: Axios

### DevOps
- **CI/CD**: GitHub Actions
- **Source Control**: Git
- **Containerization**: Ready for Docker 

## ✨ Features

### User Authentication
- User registration with personal details
- Secure login with JWT authentication
- Protected routes and API endpoints
- Profile management

### Account Management
- View all accounts with balances
- Create new accounts in different currencies (RON, USD, EUR)
- Set and customize spending limits
- Monitor daily withdrawal limits

### Transactions
- Transfer funds between accounts (including currency conversion)
- View detailed transaction history
- Mark important transactions for easy reference
- Filter and sort transactions

### Bill Payments
- Pay bills for various services (utilities, internet, phone, etc.)
- Store payment history and receipts
- Schedule future payments

### Security
- Encrypted passwords and sensitive data
- JWT authentication with token expiration
- Middleware for secure API access
- Protection against common web vulnerabilities

## 🚀 Installation and Setup

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18+)
- [PostgreSQL](https://www.postgresql.org/download/)

### Backend Setup
1. Clone the repository
   ```bash
   git clone https://github.com/yourusername/bankingapp.git
   cd bankingapp

2. Update the database connection string in `Backend/BankingApp.Api/appsettings.json` with your PostgreSQL credentials

3. Run the migrations to create the database
```bash
cd Backend/BankingApp.Api
dotnet ef database update
