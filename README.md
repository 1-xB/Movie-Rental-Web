# Movie-Rental-Web

## Description

**Movie-Rental-Web** is a comprehensive web application that enables users to browse, rent, and manage movies. It features a robust backend API and an intuitive frontend user interface, designed to provide a seamless movie rental experience.

---

## Features

- **User Authentication:** Registration and login functionality.
- **Movie Management:**
  - View a list of available movies.
  - Search for movies by title.
  - Rent and manage movie rentals.
- **Admin Panel:**
  - Add, edit, and delete movies and genres.
- **Authentication & Authorization:**
  - JWT-based authentication with refresh and access tokens.
- **Responsive Design:** Optimized for both mobile and desktop devices.

---

## Technologies Used

### Backend:
- **ASP.NET Core 9.0**: For building the API.
- **Entity Framework Core 9.0**: For database operations.
- **SQLite**: Lightweight database.
- **JWT Authentication**: Secure authentication using refresh and access tokens.
- **Swagger**: API documentation and testing.

### Frontend:
- **Blazor**: For building the user interface.
- **Bootstrap 5.3**: Responsive design.
- **Protected Browser Storage**: Maintaining authentication state.

---

## Getting Started

### Prerequisites

Ensure the following tools are installed:
- .NET 9.0 SDK
- Node.js (for managing frontend dependencies)

### Setup

1. **Clone the Repository:**
   ```sh
   git clone https://github.com/1-xB/Movie-Rental-Web.git
   cd Movie-Rental-Web
   ```

2. **Setup Backend:**
   - Navigate to the backend project directory:
     ```sh
     cd MovieRental
     ```
   - Restore dependencies:
     ```sh
     dotnet restore
     ```
   - Apply database migrations:
     ```sh
     dotnet ef database update
     ```

3. **Setup Frontend:**
   - Navigate to the frontend project directory:
     ```sh
     cd ./MovieRental.Frontend
     ```
   - Install dependencies:
     ```sh
     npm install
     ```

4. **Run the Application:**
   - Start the backend and frontend:
     ```sh
     dotnet run --project ./MovieRental
     dotnet run --project MovieRental.Frontend
     ```
   - Open your browser and access the application:
     - Backend API: [https://localhost:7242](https://localhost:7242)
     - Frontend: [https://localhost:7086](https://localhost:7086)

---

## Usage

### For Users:
- **Sign Up / Log In:** Create an account or log in.
- **Browse Movies:** Explore available movies and search by title.
- **Rent Movies:** Rent and monitor your current rentals.

### For Admins:
  - Manage movies and genres (add, update, delete) via Swagger or other tools.

---

## API Documentation

The backend API includes comprehensive Swagger documentation. After starting the application, visit:
[https://localhost:7242/swagger](https://localhost:7242/swagger) to explore available endpoints.

---

## Screenshots

Here are some visuals of the application:

1. **Homepage:**
   ![Homepage](img/Image1.png)

2. **Movie List:**
   ![Movie List](img/Image2.png)

3. **Movie Rental:**
   ![Movie Rental](img/Image3.png)

4. **User Profile:**
   ![User Profile](img/Image4.png)
