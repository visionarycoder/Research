#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Access.Numbers.Service/Access.Numbers.Service.csproj", "Access.Numbers.Service/"]
COPY ["Access.Numbers.Interface/Access.Numbers.Interface.csproj", "Access.Numbers.Interface/"]
COPY ["Ifx/Ifx.csproj", "Ifx/"]
COPY ["Resource.Data.NumbersDb/Resource.Data.NumbersDb.csproj", "Resource.Data.NumbersDb/"]
RUN dotnet restore "./Access.Numbers.Service/Access.Numbers.Service.csproj"
COPY . .
WORKDIR "/src/Access.Numbers.Service"
RUN dotnet build "./Access.Numbers.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Access.Numbers.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Access.Numbers.Service.dll"]