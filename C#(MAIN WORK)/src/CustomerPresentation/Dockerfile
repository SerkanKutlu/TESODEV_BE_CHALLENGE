#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CustomerPresentation/CustomerPresentation.csproj", "CustomerPresentation/"]
COPY ["CustomerApplication/CustomerApplication.csproj", "CustomerApplication/"]
COPY ["CustomerDomain/CustomerDomain.csproj", "CustomerDomain/"]
COPY ["CustomerInfrastructure/CustomerInfrastructure.csproj", "CustomerInfrastructure/"]
RUN dotnet restore "CustomerPresentation/CustomerPresentation.csproj"
COPY . .
WORKDIR "/src/CustomerPresentation"
RUN dotnet build "CustomerPresentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerPresentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerPresentation.dll"]
