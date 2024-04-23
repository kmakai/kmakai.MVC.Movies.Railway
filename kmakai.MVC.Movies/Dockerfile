#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["kmakai.MVC.Movies/kmakai.MVC.Movies.csproj", "kmakai.MVC.Movies/"]
RUN dotnet restore "./kmakai.MVC.Movies/./kmakai.MVC.Movies.csproj"
COPY . .
WORKDIR "/src/kmakai.MVC.Movies"
RUN dotnet build "./kmakai.MVC.Movies.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./kmakai.MVC.Movies.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kmakai.MVC.Movies.dll"]