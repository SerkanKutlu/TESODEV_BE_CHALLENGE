#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OrderPresentation/OrderPresentation.csproj", "OrderPresentation/"]
COPY ["OrderApplication/OrderApplication.csproj", "OrderApplication/"]
COPY ["OrderDomain/OrderDomain.csproj", "OrderDomain/"]
COPY ["OrderInfrastructure/OrderInfrastructure.csproj", "OrderInfrastructure/"]
RUN dotnet restore "OrderPresentation/OrderPresentation.csproj"
COPY . .
WORKDIR "/src/OrderPresentation"
RUN dotnet build "OrderPresentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderPresentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderPresentation.dll"]
