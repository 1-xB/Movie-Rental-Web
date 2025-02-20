# Movie-Rental-Web
## Description

Movie-Rental-Web is a web application where users can browse, rent, and administer movies. The application includes a backend API as well as a frontend user interface.

## Functions

- Registration and login of the user

- View available movies
- Find movies by title
- Rent movies and manage the rental
- Admin functions to add, edit, and delete movies and genres
- JWT authentication and authorization using refresh token and access token
- Responsive design for both mobile and desktop
## Technologies Used

- **Backend:**

  - ASP.NET Core 9.0
  - Entity Framework Core 9.0
  - SQLite
  - JWT Authentication using refresh token and access token
  - Swagger for API
- **Frontend:**

  - Blazor
  - Bootstrap 5.3
  - Protected Browser Storage for authentication state
## Getting Started

### Prerequisites

-.NET 9.0 SDK

- Node.js (for frontend dependencies)
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
   - Frontend: [https://localhost:5001](https://localhost:7086)
---

## Usage

- **Users:**

- Sign up for a new account or log in to an existing one.
  - Browse and search for movies by title.
  - Rent available movies and monitor your rentals.
- **Admin:**

  - Log in with an admin account.
  - Add, update or delete videos and genres via swagger.
---


## API Documentation

The backend API is Swagger-documented.

Once the application is run, navigate to [https://localhost:7242/swagger](https://localhost:7242/swagger) in order to explore the API endpoints.
---

## Screenshots

Below are some screenshots of the application:

1. **Homepage:**

  ![Homepage](img/Image1.png)
2. **Movie List:**

  ![Movie List](img/Image2.png)
3. **Movie Rental:**

  ![Movie Rental](img/Image3.png)
4. **User Profile:**

  ![User Profile](img/Image4.png)
