#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Engine.Calculator.Service/Engine.Calculator.Service.csproj", "Engine.Calculator.Service/"]
COPY ["Access.Numbers.Interface/Access.Numbers.Interface.csproj", "Access.Numbers.Interface/"]
COPY ["Ifx/Ifx.csproj", "Ifx/"]
COPY ["Engine.Calculator.Interface/Engine.Calculator.Interface.csproj", "Engine.Calculator.Interface/"]
RUN dotnet restore "./Engine.Calculator.Service/Engine.Calculator.Service.csproj"
COPY . .
WORKDIR "/src/Engine.Calculator.Service"
RUN dotnet build "./Engine.Calculator.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Engine.Calculator.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Engine.Calculator.Service.dll"]