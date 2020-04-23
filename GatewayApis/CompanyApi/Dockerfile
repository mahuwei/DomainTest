#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.3 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.201 AS build
WORKDIR /src
COPY ["GatewayApis/CompanyApi/CompanyApi.csproj", "GatewayApis/CompanyApi/"]
COPY ["GatewayApis/Models/Models.csproj", "GatewayApis/Models/"]
RUN dotnet restore "GatewayApis/CompanyApi/CompanyApi.csproj"
COPY . .
WORKDIR "/src/GatewayApis/CompanyApi"
RUN dotnet build "CompanyApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CompanyApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompanyApi.dll"]