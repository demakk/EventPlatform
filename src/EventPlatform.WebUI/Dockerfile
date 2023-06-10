FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/EventPlatform.WebUI/EventPlatform.WebUI.csproj", "EventPlatform.WebUI/"]
RUN dotnet restore "src/EventPlatform.WebUI/EventPlatform.WebUI.csproj"
COPY . .
WORKDIR "/src/EventPlatform.WebUI"
RUN dotnet build "EventPlatform.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventPlatform.WebUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventPlatform.WebUI.dll"]
