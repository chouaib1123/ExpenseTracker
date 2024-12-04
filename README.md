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

1. Navigate to the backend directory:

   ```bash
   cd backend
   ```

2. Restore packages:

   ```bash
   dotnet restore
   ```

3. Update `appsettings.json` with your database connection string.

4. Apply migrations (if needed):

   ```bash
   dotnet ef database update
   ```

5. Run the backend:

   ```bash
   dotnet run
   ```

### Frontend Setup

1. Navigate to the frontend directory:

   ```bash
   cd frontend
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Start the frontend:

   ```bash
   ng serve
   ```

Access the app at [http://localhost:4200](http://localhost:4200) (frontend) and [http://localhost:5000](http://localhost:5000) (backend).

## Project Structure

- `/backend` - .NET backend
- `/frontend` - Angular frontend
- `.gitignore` - Git ignore rules
- `README.md` - Documentation

## Contact

Email: ChouaibMonsif123@gmail.com
