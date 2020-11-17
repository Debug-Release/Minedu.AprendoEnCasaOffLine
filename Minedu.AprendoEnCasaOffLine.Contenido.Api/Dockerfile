#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#EXPOSE 80
#EXPOSE 443
#ENV ASPNETCORE_CORS http://localhost:4200
#ENV ASPNETCORE_MONGODB mongo
#ENV HORA_DESCARGA_INICIO 02:00:00
#ENV HORA_DESCARGA_INTERVALO 01:00:00
#ENV RUTA_ARCHIVOS /app/archivos

#COPY /bin/Publish/ /app/
#WORKDIR /app
#ENTRYPOINT ["dotnet", "Minedu.AprendoEnCasaOffLine.Contenido.Api.dll"]

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app
ENV ASPNETCORE_CORS http://localhost:4200
ENV ASPNETCORE_MONGODB mongo
ENV HORA_DESCARGA_INICIO 02:00:00
ENV HORA_DESCARGA_INTERVALO 01:00:00
ENV SERVIDOR_INTERVALO 500
ENV RUTA_ARCHIVOS /app/archivos
ENV RUTA_ACK /app/ack
ENV RUTA_TRAZABILIDAD /app/trazabilidad
ENV RUTA_CONTINUIDAD /app/continuidad
ENV TOKEN_SECRET SECRET
ENV TOKEN_USER DOWNLOADOFFLINE_DEV
ENV TOKEN_PASSWORD rYWXlDIqFSsjrpFEehaqYFkcofOsdFLdhsZIsViF3a6WxHZq0CFDdg==
ENV TOKEN_CLIENT_ID download-offline_dev
ENV TOKEN_EXPIRES_DAYS 7

COPY *.sln ./

COPY Minedu.AprendoEnCasaOffLine.Contenido.Api ./Minedu.AprendoEnCasaOffLine.Contenido.Api/
COPY Minedu.AprendoEnCasaOffLine.Contenido.Core ./Minedu.AprendoEnCasaOffLine.Contenido.Core/

WORKDIR /app/Minedu.AprendoEnCasaOffLine.Contenido.Api
RUN dotnet restore "Minedu.AprendoEnCasaOffLine.Contenido.Api.csproj" --configfile NuGet.config -nowarn:msb3202,nu1503 
RUN dotnet publish "Minedu.AprendoEnCasaOffLine.Contenido.Api.csproj" -c Release -o out

# Imagen final

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app

COPY --from=build /app/Minedu.AprendoEnCasaOffLine.Contenido.Api/out .
EXPOSE 80
#EXPOSE 443
ENV ASPNETCORE_CORS http://localhost:4200
ENV ASPNETCORE_MONGODB mongo
ENV HORA_DESCARGA_INICIO 02:00:00
ENV HORA_DESCARGA_INTERVALO 01:00:00
ENV SERVIDOR_INTERVALO 500
ENV RUTA_ARCHIVOS /app/archivos
ENV RUTA_ACK /app/ack
ENV RUTA_TRAZABILIDAD /app/trazabilidad
ENV RUTA_CONTINUIDAD /app/continuidad
ENV TOKEN_SECRET SECRET
ENV TOKEN_USER DOWNLOADOFFLINE_DEV
ENV TOKEN_PASSWORD rYWXlDIqFSsjrpFEehaqYFkcofOsdFLdhsZIsViF3a6WxHZq0CFDdg==
ENV TOKEN_CLIENT_ID download-offline_dev
ENV TOKEN_EXPIRES_DAYS 7

ENTRYPOINT ["dotnet", "Minedu.AprendoEnCasaOffLine.Contenido.Api.dll"]
