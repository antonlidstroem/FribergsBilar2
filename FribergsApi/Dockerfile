# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS build
#USER $APP_UID
WORKDIR /src
#EXPOSE 8080
#EXPOSE 8081

COPY ["FribergsApi/FribergsApi.csproj", "FribergsApi/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Fribergs.Cor/Fribergs.C.csproj.", "Fribergs.Core/"]

RUN dotnet restore "Fribergs/FribergsApi.csproj"

#KOPIERA HELA KÄLLKODEN

WORKDIR "/src/FribergsApi"
RUN dotnet publish "FribergsApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final





# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FribergsApi/FribergsApi.csproj", "FribergsApi/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Fribergs.Core/Fribergs.Core.csproj", "Fribergs.Core/"]
RUN dotnet restore "./FribergsApi/FribergsApi.csproj"
COPY . .
WORKDIR "/src/FribergsApi"
RUN dotnet build "./FribergsApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FribergsApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FribergsApi.dll"]