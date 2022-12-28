#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/SecondAPI/ChargesAPI.csproj", "src/SecondAPI/"]
COPY ["src/Common/RedisCache/CacheLibrary/CacheLibrary.csproj", "src/Common/RedisCache/CacheLibrary/"]
RUN dotnet restore "src/SecondAPI/ChargesAPI.csproj"
COPY . .
WORKDIR "/src/src/SecondAPI"
RUN dotnet build "ChargesAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChargesAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChargesAPI.dll"]