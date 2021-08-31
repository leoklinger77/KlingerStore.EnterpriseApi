#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["./Web/MVC/KSE.WebAppMvc/KSE.WebAppMvc.csproj", "Web/MVC/KSE.WebAppMvc/"]
COPY ["./BuildingBlocks/Core/KSE.Core/KSE.Core.csproj", "buildingblocks/KSE.WebApi.Core/"]
COPY ["./BuildingBlocks/KSE.WebApi.Core/KSE.WebApi.Core.csproj", "buildingblocks/KSE.WebApi.Core/"]

RUN dotnet restore "Web/MVC/KSE.WebAppMvc/KSE.WebAppMvc.csproj"
COPY . .
WORKDIR "Web/MVC/KSE.WebAppMvc"
RUN dotnet build "KSE.WebAppMvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KSE.WebAppMvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KSE.WebAppMvc.dll"]