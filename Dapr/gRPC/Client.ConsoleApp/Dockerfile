#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Client.ConsoleApp/Client.ConsoleApp.csproj", "Client.ConsoleApp/"]
COPY ["Ifx/Ifx.csproj", "Ifx/"]
COPY ["Manager.Content.Service/Manager.Content.Service.csproj", "Manager.Content.Service/"]
COPY ["Engine.Calculator.Interface/Engine.Calculator.Interface.csproj", "Engine.Calculator.Interface/"]
COPY ["Manager.Content.Interface/Manager.Content.Interface.csproj", "Manager.Content.Interface/"]
RUN dotnet restore "./Client.ConsoleApp/Client.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/Client.ConsoleApp"
RUN dotnet build "./Client.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Client.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Client.ConsoleApp.dll"]