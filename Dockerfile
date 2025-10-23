# Stage 1: Build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Stage 2: Run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Business.dll"]
