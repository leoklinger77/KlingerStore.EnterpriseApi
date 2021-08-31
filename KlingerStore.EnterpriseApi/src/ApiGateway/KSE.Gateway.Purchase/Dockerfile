#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["./ApiGateway/KSE.Gateway.Purchase/KSE.Gateway.Purchase.csproj", "ApiGateway/KSE.Gateway.Purchase/"]
COPY ["./BuildingBlocks/Core/KSE.Core/KSE.Core.csproj", "buildingblocks/KSE.WebApi.Core/"]
COPY ["./BuildingBlocks/KSE.WebApi.Core/KSE.WebApi.Core.csproj", "buildingblocks/KSE.WebApi.Core/"]
COPY ["./BuildingBlocks/MessageBus/KSE.MessageBus/KSE.MessageBus.csproj", "buildingblocks/KSE.MessageBus/"]

RUN dotnet restore "ApiGateway/KSE.Gateway.Purchase/KSE.Gateway.Purchase.csproj"
COPY . .
WORKDIR "ApiGateway/KSE.Gateway.Purchase"
RUN dotnet build "KSE.Gateway.Purchase.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KSE.Gateway.Purchase.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KSE.Gateway.Purchase.dll"]