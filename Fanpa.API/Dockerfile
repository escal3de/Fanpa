FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Fanpa.API/Fanpa.API.csproj", "Fanpa.API/"]
COPY ["Fanpa.Application/Fanpa.Application.csproj", "Fanpa.Application/"]
COPY ["Fanpa.Domain/Fanpa.Domain.csproj", "Fanpa.Domain/"]
COPY ["Fanpa.Infrastructure/Fanpa.Infrastructure.csproj", "Fanpa.Infrastructure/"]
COPY ["Fanpa.Persistence/Fanpa.Persistence.csproj", "Fanpa.Persistence/"]

RUN dotnet restore "Fanpa.API/Fanpa.API.csproj"

COPY . .
WORKDIR "/src/Fanpa.API"
RUN dotnet publish "Fanpa.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Fanpa.API.dll"]