#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/MassTransit.Saga.Demo.TransportationService/MassTransit.Saga.Demo.TransportationService.csproj", "MassTransit.Saga.Demo.TransportationService/"]
COPY ["src/MassTransit.Saga.Demo.Contracts/MassTransit.Saga.Demo.Contracts.csproj", "MassTransit.Saga.Demo.Contracts/"]
RUN dotnet restore "MassTransit.Saga.Demo.TransportationService/MassTransit.Saga.Demo.TransportationService.csproj"
COPY ./src/MassTransit.Saga.Demo.TransportationService/ ./MassTransit.Saga.Demo.TransportationService/
COPY ./src/MassTransit.Saga.Demo.Contracts/ ./MassTransit.Saga.Demo.Contracts/
WORKDIR "/src/MassTransit.Saga.Demo.TransportationService"
RUN dotnet build "MassTransit.Saga.Demo.TransportationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MassTransit.Saga.Demo.TransportationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MassTransit.Saga.Demo.TransportationService.dll"]