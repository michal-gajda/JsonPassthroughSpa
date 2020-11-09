FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY . .

RUN dotnet restore
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

RUN apt-get update && apt-get -y install iputils-ping net-tools

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JsonPassthroughSpa.dll"]