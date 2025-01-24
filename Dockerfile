# Dockerfile for .NET Core 8 API with SQL Server

# Base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project file and restore dependencies
COPY . ./
RUN dotnet restore

# Copy the remaining source code
#COPY . .

# Build the application
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published output from the build image
COPY --from=build /app/out .

# Set the environment variables for SQL Server connection
# ENV ConnectionStrings__DefaultConnection "server=localhost; database=EShop;uid=sa;pwd=reallyStrongPwd123;TrustServerCertificate=True"

# Expose the port
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "Dormy.WebService.Api.dll"]