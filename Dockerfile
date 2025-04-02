# Use official .NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["courier-scam-finder-back-end.csproj", "./"]
RUN dotnet restore "./courier-scam-finder-back-end.csproj"
COPY . .
RUN dotnet publish "./courier-scam-finder-back-end.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "courier-scam-finder-back-end.dll"]
