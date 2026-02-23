# IFOSUP Blazor Web App

Blazor Web App developed in C# (.NET 9) that consumes an external API and allows authenticated users to manage local favorites.

Project goal:

> Develop, test, document and present a Blazor application that displays data from an API.  
> An authenticated user must be able to create local favorites and manage (edit/delete) them.

---

## Tech Stack

- C# / .NET 9
- Blazor Web App (Interactive Server)
- ASP.NET Core Identity
- Entity Framework Core
- SQLite
- xUnit (unit testing)
- VS Code

---

## Features

### 1. API Integration
- Weather data fetched from Open-Meteo API
- Display current weather and next 24 hours forecast
- User-friendly time formatting (dd/MM HH:mm)

### 2. Authentication
- ASP.NET Core Identity
- Register / Login / Logout
- Account management page
- UI adapts using `AuthorizeView`

### 3. Favorites System (Persistence + CRUD)
Authenticated users can:

- Add an hourly forecast entry to favorites
- Edit custom title
- Delete favorite
- Favorites stored locally in SQLite
- Ownership enforced (user can only manage their own favorites)

### 4. Data Flow
Open-Meteo API
↓
OpenMeteoClient (Service)
↓
APIWeather Page (UI)
↓
FavoritesService
↓
ApplicationDbContext (EF Core)
↓
SQLite database


### 5. Unit Testing

- xUnit test project
- Tests for FavoritesService:
  - Add favorite
  - Prevent duplicates
  - Update favorite
  - Delete favorite
  - Ownership validation
- Run with:

```bash
dotnet test

ifosup/
 ├── Components/
 │    ├── Pages/
 │    │    ├── Home.razor
 │    │    ├── APIWeather.razor
 │    │    └── Favorites.razor
 │    └── Layout/
 │         ├── MainLayout.razor
 │         └── NavMenu.razor
 │
 ├── Services/
 │    ├── OpenMeteoClient.cs
 │    └── FavoritesService.cs
 │
 ├── Data/
 │    └── ApplicationDbContext.cs
 │
 ├── Models/
 │    └── FavoriteItem.cs
 │
 ├── Areas/Identity/
 │    └── Pages/
 │
 ├── wwwroot/
 │    └── css/
 │         ├── app.css
 │         └── theme.css
 │
 └── Program.cs

 ### How to run
    dotnet restore
    dotnet build
    dotnet run
