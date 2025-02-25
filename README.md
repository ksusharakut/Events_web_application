Event Management API

This is a web application for managing events and participants. Features include:
Creating, updating, deleting, and filtering events
Registering participants, canceling participation, and viewing the list of participants
Authentication and authorization via JWT

Technologies

.NET 8.0
ASP.NET Core Web API
Entity Framework Core + PostgreSQL
AutoMapper
FluentValidation
JWT
Swagger
EF Fluent API
xUnit
Docker

Architecture

The project follows the Clean Architecture principle and includes:
Domain – business logic and entities
Application – interfaces and services
Infrastructure – database operations and repositories
WebAPI – REST API

Installation and Running

Option 1: Running Locally (Without Docker)

1. Clone the Repository
   git clone repository*
   cd projectfolder*

2. Add appsettings.json
   Create a file named appsettings.json in the WebAPI project folder with the following content (adjust values as needed):

   {
   "Logging": {
   "LogLevel": {
   "Default": "Information",
   "Microsoft.AspNetCore": "Warning"
   }
   },
   "AllowedHosts": "\*",
   "ConnectionStrings": {
   "DefaultConnection": "Host=localhost;Port=5432;Database=events;Username=your_username;Password=your_password"
   },
   "Jwt": {
   "Key": "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZXIiLCJVc2VybmFtZSI6IkphdmFJblVzZSIsImV4cCI6MTc0KDA1OTU5MywiaWF0IjoxNzQwMDU5NTkzfQ.",
   "Issuer": "https://localhost:7247/",
   "Audience": "https://localhost:7247/"
   }
   }

   - Replace your_username and your_password with your PostgreSQL credentials.
   - Update the Jwt section (especially Key) with a secure secret key for production use.

3. Apply Database Migrations
   Ensure you have the .NET CLI installed, then run the following command from the project root folder to create or update the database:

   dotnet ef database update --project Infrastructure

4. Run the Application
   Start the application using the .NET CLI:
   dotnet run --project WebAPI

5. Access the API
   Open the following link in your browser:
   https://localhost:7247/swagger (or the port specified in your launchSettings.json).



Option 2: Running with Docker

1. Clone the Repository

git clone repository*
cd project folder*

2. Add appsettings.json
   Create a file named appsettings.json in the WebAPI project folder with the following content (adjust values as needed):

   {
   "Logging": {
   "LogLevel": {
   "Default": "Information",
   "Microsoft.AspNetCore": "Warning"
   }
   },
   "AllowedHosts": "\*",
   "ConnectionStrings": {
   "DefaultConnection": "Host=host.docker.internal;Port=5432;Database=events;Username=your_username;Password=your_password"
   },
   "Jwt": {
   "Key": "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZXIiLCJVc2VybmFtZSI6IkphdmFJblVzZSIsImV4cCI6MTc0KDA1OTU5MywiaWF0IjoxNzQwMDU5NTkzfQ.",
   "Issuer": "https://localhost:7247/",
   "Audience": "https://localhost:7247/"
   }
   }

   - Replace your_username and your_password with your PostgreSQL credentials.
   - Update the Jwt section (especially Key) with a secure secret key for production use.

3. Build and Run with Docker

# Navigate to the project root folder

cd project folder\*

# Build the Docker image

docker build -t myeventapp .

# Run the container

docker run -d -p 8080:80 myeventapp

4. Access the API

Open the following link in your browser:
http://localhost:8080/swagger

Testing

Run unit tests:
dotnet test

Additional Information

API documentation is available in Swagger (/swagger).
Bcrypt is used for password hashing.
All controllers follow the "Controller without logic" principle.
