FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src

# Libs
COPY ["Caching.Abstractions/*.csproj", "Caching.Abstractions/"]
COPY ["Communication/*.csproj", "Communication/"]
COPY ["Communication.Abstractions/*.csproj", "Communication.Abstractions/"]
COPY ["Communication.CodeGeneration/*.csproj", "Communication.CodeGeneration/"]
COPY ["Configuration/*.csproj", "Configuration/"]
COPY ["Core/*.csproj", "Core/"]
COPY ["Monitoring.Abstractions/*.csproj", "Monitoring.Abstractions/"]
COPY ["Monitoring/*.csproj", "Monitoring/"]
COPY ["Storage.Abstractions/*.csproj", "Storage.Abstractions/"]

# Demo
COPY ["DemoApi/*.csproj", "DemoApi/"]
COPY ["DemoShared/*.csproj", "DemoShared/"]
COPY ["DemoWorker/*.csproj", "DemoWorker/"]

RUN dotnet restore "DemoApi/DemoApi.csproj"

# Libs
COPY ["Caching.Abstractions/**", "Caching.Abstractions/"]
COPY ["Communication/**", "Communication/"]
COPY ["Communication.Abstractions/**", "Communication.Abstractions/"]
COPY ["Communication.CodeGeneration/**", "Communication.CodeGeneration/"]
COPY ["Configuration/**", "Configuration/"]
COPY ["Core/**", "Core/"]
COPY ["Monitoring.Abstractions/**", "Monitoring.Abstractions/"]
COPY ["Monitoring/**", "Monitoring/"]
COPY ["Storage.Abstractions/**", "Storage.Abstractions/"]

# Demo
COPY ["DemoApi/**", "DemoApi/"]
COPY ["DemoShared/**", "DemoShared/"]
#COPY ["DemoWorker/**", "DemoWorker/"]

WORKDIR /src/DemoApi
RUN dotnet publish "DemoApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoApi.dll"]