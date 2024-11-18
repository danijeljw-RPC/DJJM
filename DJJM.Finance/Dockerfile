# Stage 1: Build the Blazor WebAssembly App
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install necessary tools for WebAssembly builds
RUN apt-get update && apt-get install -y \
    python3 \
    python3-pip \
    build-essential \
    && rm -rf /var/lib/apt/lists/*

# Install Emscripten
RUN curl -L https://github.com/emscripten-core/emsdk/archive/main.tar.gz | tar -xz \
    && cd emsdk-main \
    && ./emsdk install latest \
    && ./emsdk activate latest \
    && echo "source $(pwd)/emsdk-main/emsdk_env.sh" >> /etc/bash.bashrc

# Set environment variables for Emscripten
ENV PATH="/emsdk-main/upstream/emscripten:${PATH}"
ENV EMSDK="/emsdk-main"

# Install workloads
RUN dotnet workload install wasm-tools

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
