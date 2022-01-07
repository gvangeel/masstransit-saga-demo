#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MassTransit.Saga.Demo.HotelService/MassTransit.Saga.Demo.HotelService.csproj", "MassTransit.Saga.Demo.HotelService/"]
COPY ["src/MassTransit.Saga.Demo.Contracts/MassTransit.Saga.Demo.Contracts.csproj", "MassTransit.Saga.Demo.Contracts/"]
RUN dotnet restore "MassTransit.Saga.Demo.HotelService/MassTransit.Saga.Demo.HotelService.csproj"
COPY ./src/MassTransit.Saga.Demo.HotelService/ ./MassTransit.Saga.Demo.HotelService/
COPY ./src/MassTransit.Saga.Demo.Contracts/ ./MassTransit.Saga.Demo.Contracts/
WORKDIR "/src/MassTransit.Saga.Demo.HotelService"
RUN dotnet build "MassTransit.Saga.Demo.HotelService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MassTransit.Saga.Demo.HotelService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MassTransit.Saga.Demo.HotelService.dll"]