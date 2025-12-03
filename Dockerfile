FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY ["./Server/Server.csproj", "./Server/"]
COPY ["./Core/Core.csproj", "./Core/"]
COPY ["./Application/Application.csproj", "./Application/"]
COPY ["./DataAccess/DataAccess.csproj", "./DataAccess/"]
COPY ["./Infrastructure/Infrastructure.csproj", "./Infrastructure/"]

RUN dotnet restore "./Server/Server.csproj"

COPY . .
RUN dotnet publish "./Server/Server.csproj" -c Release -o out /p:UseAppHost=false 

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 8080

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENTRYPOINT ["dotnet", "Server.dll"]