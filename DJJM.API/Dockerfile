# Stage 1: Build the API and Blazor WebAssembly
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Set the ASP.NET Core environment
ENV ASPNETCORE_ENVIRONMENT=Development

# Copy only the project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code and publish
COPY . .
RUN dotnet publish -c Release -o /app

# Stage 2: Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Expose ports for both the API and Swagger UI
EXPOSE 5000
EXPOSE 5001

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
#ENV ASPNETCORE_URLS=http://+:5000;https://+:5001
ENV ASPNETCORE_URLS=http://+:5000

# Copy published output to runtime image
COPY --from=build /app .

# Set the entrypoint to the API
ENTRYPOINT ["dotnet", "DJJM.API.dll"]
