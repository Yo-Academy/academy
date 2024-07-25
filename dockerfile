# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the rest of the application code
COPY . .

# Restore package dependencies for the solution
RUN dotnet restore "Academy.sln"

WORKDIR /src/Academy.Api

# Publish the project to the /app directory
RUN dotnet publish -c Release -o /app

# Use the official .NET runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app .

EXPOSE 80

ENTRYPOINT ["dotnet", "Academy.Api.dll"]
