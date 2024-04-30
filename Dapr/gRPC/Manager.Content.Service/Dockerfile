#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Manager.Content.Service/Manager.Content.Service.csproj", "Manager.Content.Service/"]
COPY ["Engine.Calculator.Interface/Engine.Calculator.Interface.csproj", "Engine.Calculator.Interface/"]
COPY ["Ifx/Ifx.csproj", "Ifx/"]
COPY ["Manager.Content.Interface/Manager.Content.Interface.csproj", "Manager.Content.Interface/"]
RUN dotnet restore "./Manager.Content.Service/Manager.Content.Service.csproj"
COPY . .
WORKDIR "/src/Manager.Content.Service"
RUN dotnet build "./Manager.Content.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Manager.Content.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Manager.Content.Service.dll"]