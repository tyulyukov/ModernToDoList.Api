FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ModernToDoList.Api/ModernToDoList.Api.csproj", "ModernToDoList.Api/"]
RUN dotnet restore "ModernToDoList.Api/ModernToDoList.Api.csproj"
COPY . .
WORKDIR "/src/ModernToDoList.Api"
RUN dotnet build "ModernToDoList.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ModernToDoList.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ModernToDoList.Api.dll"]
