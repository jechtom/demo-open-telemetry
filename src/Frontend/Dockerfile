FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . OTelDemo.Web/
RUN dotnet restore "OTelDemo.Web/OTelDemo.Web.csproj"
WORKDIR /src/OTelDemo.Web
RUN dotnet build "OTelDemo.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OTelDemo.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OTelDemo.Web.dll"]
