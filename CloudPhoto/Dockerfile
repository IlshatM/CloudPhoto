FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CloudPhoto/CloudPhoto.csproj", "CloudPhoto/"]
RUN dotnet restore "CloudPhoto/CloudPhoto.csproj"
COPY . .
WORKDIR "/src/CloudPhoto"
RUN dotnet build "CloudPhoto.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudPhoto.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudPhoto.dll"]
