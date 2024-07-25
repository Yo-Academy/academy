# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj and restore as distinct layers
COPY Academy.Api/*.csproj ./Academy.Api/
RUN dotnet restore ./Academy.Api/Academy.Api.csproj

# Copy the rest of the application code
COPY . .

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
