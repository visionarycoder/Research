#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug #Release
WORKDIR /src
COPY ["MyFrontEnd/MyFrontEnd.csproj", "MyFrontEnd/"]
RUN dotnet restore "./MyFrontEnd/MyFrontEnd.csproj"
COPY . .
WORKDIR "/src/MyFrontEnd"
RUN dotnet build "./MyFrontEnd.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyFrontEnd.csproj" -c $BUILD_CONFIGURATION -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFrontEnd.dll"]