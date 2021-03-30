##### Con Dockerfile####
#1. docker build -t releasedebug/dockerapi:v1 . 
#2. docker run -d -p 80:80 releasedebug/dockerapi:v1
#3. docker push releasedebug/dockerapi:v1

##### Con docker-compose ####
#1. docker-compose build
#2. docker-compose up -d
#3. docker push releasedebug/dockerapi:v1

# Get Base Image (Full .NET Core SDK)
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80
RUN apt-get -y update
RUN apt-get -y install libc6-dev
RUN apt-get -y install libgdiplus
EXPOSE 80
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","Minedu.AprendoEnCasaOffLine.Contenido.Fabio.dll" ]