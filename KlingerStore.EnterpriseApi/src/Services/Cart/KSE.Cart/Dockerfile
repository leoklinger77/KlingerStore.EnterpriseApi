#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Services/Cart/KSE.Cart/KSE.Cart.csproj", "src/Services/Cart/KSE.Cart/"]
RUN dotnet restore "src/Services/Cart/KSE.Cart/KSE.Cart.csproj"
COPY . .
WORKDIR "/src/src/Services/Cart/KSE.Cart"
RUN dotnet build "KSE.Cart.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KSE.Cart.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KSE.Cart.dll"]