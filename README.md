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
   git clone https://github.com/ValuIancu24/bankingapp.git
   cd bankingapp

2. Update the database connection string in `Backend/BankingApp.Api/appsettings.json` with your PostgreSQL credentials

3. Run the migrations to create the database
```bash
cd Backend/BankingApp.Api
dotnet ef database update

### Frontend Setup
-Navigate to the frontend directory:

bash
-Copy
-Edit
-cd Frontend
-Install dependencies:

-bash
-Copy
-Edit
-npm install
-Start the development server:

bash
Copy
Edit
npm run dev
The application will be available at:
👉 http://localhost:5173

### Testing
Backend Tests
bash
Copy
Edit
cd Backend/BankingApp.Tests
dotnet test
Frontend Tests
bash
Copy
Edit
cd Frontend
npm test
## API Documentation
The API follows RESTful conventions with the following main endpoints:

/api/auth – Authentication (login, register)

/api/user – User profile management

/api/account – Account operations

/api/transaction – Transaction operations

/api/billpayment – Bill payment endpoints

## Detailed API documentation is available via Swagger UI when the backend is running in development mode.

### Project Structure
csharp
Copy
Edit
BankingApp/
├── Backend/
│   └── BankingApp.Api/
│       ├── Controllers/    # API endpoints
│       ├── Models/         # Database entities
│       ├── DTOs/           # Data transfer objects
│       ├── Services/       # Business logic
│       ├── Repositories/   # Data access layer
│       ├── Data/           # Database context
│       ├── Migrations/     # EF Core migrations
│       ├── Middleware/     # Custom middleware
│       └── Helpers/        # Utility classes
├── Frontend/
│   ├── public/             # Static assets
│   └── src/
│       ├── components/     # React components
│       ├── contexts/       # React contexts
│       ├── services/       # API service layers
│       └── utils/          # Utility functions
└── .github/
    └── workflows/          # CI/CD workflows
🔐 Authentication Flow
User registers or logs in through the frontend

Backend validates credentials and issues a JWT token

Token is stored in localStorage and sent with the Authorization header

Backend middleware validates the token on protected routes

Expired token triggers automatic logout

💰 Transaction Processing
User initiates a transaction (transfer, deposit, withdrawal, bill payment)

Backend validates the transaction (e.g., funds, limits)

Transaction is recorded in the database

Account balances are updated

Currency conversion applied if needed

Confirmation sent to the user

👥 Contributors
Your Name

Your Friend's Name

📄 License
This project is licensed under the MIT License – see the LICENSE file for details.

🔮 Future Enhancements
📱 Mobile application using React Native

🔐 Two-factor authentication

🧾 PDF statement generation

⏰ Scheduled/recurring payments

🎯 Savings goals and financial planning tools

📲 Push notifications for account activities

🌙 Dark mode theme
