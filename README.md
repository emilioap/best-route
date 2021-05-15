# BestRoute
Program to find the cheapest airline route to travel.

The solution structure is divided by 5 projects based on DDD :

1. Console Application (To run in console)
2. WebApi (Rest API to get best routes and update datasource - more info in Swagger)
3. Domain (Class library containing models to share and interfaces for DI)
4. Service (Class library with business implementations)
5. Tests (Unit tests made by xUnit)

# Getting Started

- Begin by cloning this repository
- Install the version .NET 5.0 for your OS (https://dotnet.microsoft.com/download/dotnet/5.0)

# Running

## Console App

1. Go to the folder YOUR_DIR\best-route\BestRoute.ConsoleApp
2. Run the .NET CLI command ``dotnet run``

## WebApi

1. Go to the folder YOUR_DIR\best-route\BestRoute.WebApi
2. Run the .NET CLI command ``dotnet run``
3. Access the url https://localhost:5001/swagger/index.htm

## Tests
1. From the root folder (best-route) run the command ``dotnet test``
