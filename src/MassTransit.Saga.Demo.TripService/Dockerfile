#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MassTransit.Saga.Demo.TripService/MassTransit.Saga.Demo.TripService.csproj", "MassTransit.Saga.Demo.TripService/"]
COPY ["src/MassTransit.Saga.Demo.Contracts/MassTransit.Saga.Demo.Contracts.csproj", "MassTransit.Saga.Demo.Contracts/"]
COPY ["src/MassTransit.Common/MassTransit.Common.csproj", "MassTransit.Common/"]
RUN dotnet restore "MassTransit.Saga.Demo.TripService/MassTransit.Saga.Demo.TripService.csproj"
COPY ./src/MassTransit.Saga.Demo.TripService/ ./MassTransit.Saga.Demo.TripService/
COPY ./src/MassTransit.Saga.Demo.Contracts/ ./MassTransit.Saga.Demo.Contracts/
COPY ./src/MassTransit.Common/ ./MassTransit.Common/
WORKDIR "/src/MassTransit.Saga.Demo.TripService"
RUN dotnet build "MassTransit.Saga.Demo.TripService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MassTransit.Saga.Demo.TripService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MassTransit.Saga.Demo.TripService.dll"]