FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as build
ARG APP_CONFIG=Release
ARG BASE_PATH=

WORKDIR /src
COPY $BASE_PATH/Classifieds.Auth/Classifieds.Auth.csproj Classifieds.Auth/
COPY $BASE_PATH/DAL/DAL.csproj DAL/
COPY $BASE_PATH/UserService/UserService.csproj UserService/
RUN dotnet restore "UserService/UserService.csproj" /p:Configuration=$APP_CONFIG

COPY $BASE_PATH/Classifieds.Auth/. Classifieds.Auth/
COPY $BASE_PATH/DAL/. DAL/
COPY $BASE_PATH/UserService/. UserService/
RUN dotnet publish "UserService/UserService.csproj" -c $APP_CONFIG -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
RUN apk upgrade musl
RUN adduser --disabled-password --home /app --gecos '' appuser && chown -R appuser /app
USER appuser
WORKDIR /app
COPY --from=build /out ./
ENV ASPNETCORE_URLS=http://*:5000
ENTRYPOINT ["dotnet", "UserService.dll"]