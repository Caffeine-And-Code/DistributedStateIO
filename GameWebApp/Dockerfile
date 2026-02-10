# Standalone da solo => da errore perchè non trova le classi delle altre solution
## BUILD
#FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
#WORKDIR /src
#
#COPY GameWebApp.csproj .
#RUN dotnet restore
#
#COPY . .
#RUN dotnet publish -c Release -o /app/publish
#
## RUNTIME
#FROM mcr.microsoft.com/dotnet/aspnet:10.0
#WORKDIR /app
#
#COPY --from=build /app/publish .
#
#EXPOSE 8080
#ENTRYPOINT ["dotnet", "GameWebApp.dll"]

# questa dovrebbe compilare con tutto ma attualmente ho degli errori nel mio branch per qualche motivo ;^(
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia TUTTO
COPY . .

# Restore usando la solution (best practice)
RUN dotnet restore DistributedStateIO.sln

# Publish solo il web
RUN dotnet publish GameWebApp/GameWebApp.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "GameWebApp.dll"]
