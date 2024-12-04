# Expense Tracker

A modern web application for tracking personal expenses, built with .NET and Angular. Users can manage transactions, view spending analytics, and maintain budgets.

## Tech Stack

### Frontend

- Angular 16+
- TypeScript
- Angular Material UI
- RxJS

### Backend

- .NET 7.0
- Entity Framework Core
- MySQL Database
- REST APIs

## Prerequisites

Before you begin, ensure you have the following installed:

- Node.js (v16+)
- Angular CLI (`npm install -g @angular/cli`)
- .NET SDK 7.0
- MySQL Server

## Getting Started

### Backend Setup

1. Navigate to backend directory:

```bash
cd backend
```

Restore packages:

dotnet restore

Update appsettings.json with your database connection string.
Apply migrations (if needed):

dotnet ef database update

Run the backend:

    dotnet run

### Frontend Setup

Navigate to frontend directory:

```bash
cd frontend
```

Install dependencies:

npm install

Start the frontend:

    ng serve

Access the app at http://localhost:4200 (frontend) and http://localhost:5000 (backend).

## Project Structure

/backend # .NET backend
/frontend # Angular frontend
.gitignore # Git ignore rules
README.md # Documentation

## Contact

    Email: your-email@example.com
    GitHub: yourusername
