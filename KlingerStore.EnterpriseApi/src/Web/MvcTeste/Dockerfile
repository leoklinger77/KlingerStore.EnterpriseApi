#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Web/MvcTeste/MvcTeste.csproj", "src/Web/MvcTeste/"]
RUN dotnet restore "src/Web/MvcTeste/MvcTeste.csproj"
COPY . .
WORKDIR "/src/src/Web/MvcTeste"
RUN dotnet build "MvcTeste.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MvcTeste.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MvcTeste.dll"]