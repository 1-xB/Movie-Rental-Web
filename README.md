# Movie-Rental-Web
## Overview

Movie-Rental-Web is a web application by which users can view, rent, and manage movies. The application has a backend API and frontend user interface.

## Operations

- User registration and login

- View list of available movies
- Search for movies by title
- Renting movies and rental management
- Admin operations to add, edit, and delete movies and genres
- JWT authentication and authorization using refresh token and access token
- Responsive design for mobile and desktop
## Technologies Used

- **Backend:**

  - ASP.NET Core 9.0
  - Entity Framework Core 9.0
  - SQLite
  - JWT Authentication with refresh token and access token
  - Swagger API
- **Frontend:**

  - Blazor
  - Bootstrap 5.3
  - Protected Browser Storage for authentications state
## Getting Started

### Prerequisites

-.NET 9.0 SDK

- Node.js (frontend dependencies)
### Setup

1. Clone the repository:

```sh
   git clone https://github.com/1-xB/Movie-Rental-Web.git
   cd Movie-Rental-Web
```
2. Navigate to the backend project directory and restore dependencies:

   ```sh
   cd MovieRental
   dotnet restore
    ```
3. Run database migrations:

   ```sh
   dotnet ef database update
    ```
4. Navigate to the frontend project directory and restore dependencies:

   ```sh
cd./MovieRental.Frontend
npm install  
    ```
5. Execute the backend and frontend projects:

   ```sh
   dotnet run --project./MovieRental
   dotnet run --project MovieRental.Frontend
   ```
6. Open your browser and navigate to:

   - Backend API: [https://localhost:7242](https://localhost:7242)
   - Frontend: [https://localhost:7086](https://localhost:7086)
---

## Usage

- **Users:**

- Sign up for a new account or log in to an existing one.
- Browse and search for movies by name.
  - Rent available movies and monitor your rentals.
- **Admin:**

  - Log in with admin user.
  - Add, delete or update videos and genres via swagger.
---
## API Documentation

The backend API is Swagger-documented.

After executing the application, go to [https://localhost:7242/swagger](https://localhost:7242/swagger) in order to browse the API endpoints.
---

## Screenshots

Some screenshots of the application are below:

1. **Homepage:**

 ![Homepage](img/Image1.png)
2. **Movie List:**

  ![Movie List](img/Image2.png)
3. **Movie Rental:**

  ![Movie Rental](img/Image3.png)
4. **User Profile:**

  ![User Profile](img/Image4.png)
