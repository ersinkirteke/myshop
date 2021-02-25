FROM mcr.microsoft.com/azure-functions/dotnet:3.0-appservice AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Basket/ShoppingCart/ShoppingCart.csproj", "Basket/ShoppingCart/"]
RUN dotnet restore "Basket/ShoppingCart/ShoppingCart.csproj"
COPY . .
WORKDIR "/src/Basket/ShoppingCart"
RUN dotnet build "ShoppingCart.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShoppingCart.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true \
    AzureWebJobsStorage="UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://host.docker.internal"