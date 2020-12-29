#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ApiGateways/MyShop.Ocelot.ApiGateway/MyShop.Ocelot.ApiGateway.csproj", "ApiGateways/MyShop.Ocelot.ApiGateway/"]
RUN dotnet restore "ApiGateways/MyShop.Ocelot.ApiGateway/MyShop.Ocelot.ApiGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateways/MyShop.Ocelot.ApiGateway"
RUN dotnet build "MyShop.Ocelot.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyShop.Ocelot.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyShop.Ocelot.ApiGateway.dll"]
