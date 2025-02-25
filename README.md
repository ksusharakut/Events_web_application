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

2. Update data in appsettings.json file

   "DefaultConnection": "Host=localhost;Port=5432;Database=events;Username=your_username;Password=your_password"

   - Replace your_username and your_password with your PostgreSQL credentials.

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

2. Update appsettings.json

   "DefaultConnection": "Host=host.docker.internal;Port=5432;Database=events;Username=your_username;Password=your_password"

   - Use host.docker.internal like host!!!!

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
