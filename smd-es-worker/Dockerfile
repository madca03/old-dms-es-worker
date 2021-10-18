FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["smd-es-worker/smd-es-worker.csproj", "smd-es-worker/"]
RUN dotnet restore "smd-es-worker/smd-es-worker.csproj"
COPY . .
WORKDIR "/src/smd-es-worker"
RUN dotnet build "smd-es-worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "smd-es-worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "smd-es-worker.dll"]
