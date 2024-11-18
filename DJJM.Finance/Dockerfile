# Stage 1: Build the Blazor WebAssembly App
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file(s) and restore dependencies
COPY ["DJJM.Finance.csproj", "./"]
RUN dotnet restore

# Copy the entire source code and build the application
COPY . ./
RUN dotnet publish -c Release -o /out

# Stage 2: Serve the App using NGINX
FROM nginx:alpine AS runtime

# Copy built files from the build stage to the NGINX web root
COPY --from=build /out/wwwroot /usr/share/nginx/html

# Expose the default HTTP port
EXPOSE 80

# Start NGINX
CMD ["nginx", "-g", "daemon off;"]
