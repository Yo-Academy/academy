# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

# restore package dependencies for the solution
RUN dotnet restore "/src/Academy.Api/Academy.Api.csproj"

WORKDIR /src/AcademyAPI

# publish dotnet project
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 as runtime

WORKDIR /app

copy --from=build-env /app .

EXPOSE 80

ENTRYPOINT ["dotnet", "Academy.Api.dll"]
