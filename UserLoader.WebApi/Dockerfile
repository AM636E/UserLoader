#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["UserLoader.WebApi/UserLoader.WebApi.csproj", "UserLoader.WebApi/"]
RUN dotnet restore "UserLoader.WebApi/UserLoader.WebApi.csproj"
COPY . .
WORKDIR "/src/UserLoader.WebApi"
RUN dotnet build "UserLoader.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserLoader.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserLoader.WebApi.dll"]