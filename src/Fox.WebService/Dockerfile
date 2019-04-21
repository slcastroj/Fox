FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Fox/Fox.WebService.csproj", "Fox/"]
COPY ["Fox.DataAccess/Fox.DataAccess.csproj", "Fox.DataAccess/"]
RUN dotnet restore "Fox/Fox.WebService.csproj"
COPY . .
WORKDIR "/src/Fox"
RUN dotnet build "Fox.WebService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Fox.WebService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Fox.WebService.dll"]