#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN apt-get -y update; apt-get -y install curl unzip bash dos2unix
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . OTelDemo.Backend/
RUN dotnet restore "./OTelDemo.Backend/OTelDemo.Backend.csproj"
WORKDIR "/src/OTelDemo.Backend"
RUN dotnet build "./OTelDemo.Backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OTelDemo.Backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
COPY ./auto-telemetry-install.sh /app/publish/
COPY ./auto-telemetry-run.sh /app/publish/

FROM base AS final
USER root
WORKDIR /app
COPY --from=publish /app/publish .
RUN ls -la /app/
RUN dos2unix /app/auto-telemetry-install.sh /app/auto-telemetry-run.sh
RUN chmod +x /app/auto-telemetry-install.sh /app/auto-telemetry-run.sh
USER app
RUN bash /app/auto-telemetry-install.sh
ENTRYPOINT ["bash", "/app/auto-telemetry-run.sh"]
