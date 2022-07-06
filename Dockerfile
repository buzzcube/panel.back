#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build

WORKDIR /src
COPY ["panel.back.csproj", "."]
RUN dotnet restore "./panel.back.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "panel.back.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "panel.back.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "panel.back.dll"]