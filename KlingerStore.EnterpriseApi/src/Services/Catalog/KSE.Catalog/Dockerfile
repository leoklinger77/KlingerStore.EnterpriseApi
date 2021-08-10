#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Services/Catalog/KSE.Catalog/KSE.Catalog.csproj", "src/Services/Catalog/KSE.Catalog/"]
RUN dotnet restore "src/Services/Catalog/KSE.Catalog/KSE.Catalog.csproj"
COPY . .
WORKDIR "/src/src/Services/Catalog/KSE.Catalog"
RUN dotnet build "KSE.Catalog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KSE.Catalog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KSE.Catalog.dll"]