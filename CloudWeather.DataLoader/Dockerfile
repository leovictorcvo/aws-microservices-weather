#BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./CloudWeather.DataLoader.csproj" --disable-parallel
RUN dotnet publish "./CloudWeather.DataLoader.csproj" -c release -o /app --no-restore

#SERVE STAGE
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "CloudWeather.DataLoader.dll"]