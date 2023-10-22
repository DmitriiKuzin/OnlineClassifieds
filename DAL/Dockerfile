FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as build
ARG APP_CONFIG=Release
ARG BASE_PATH=

WORKDIR /src
COPY $BASE_PATH/DAL/DAL.csproj DAL/
RUN dotnet restore "DAL/DAL.csproj" /p:Configuration=$APP_CONFIG

COPY $BASE_PATH/DAL/. DAL/
RUN dotnet publish "DAL/DAL.csproj" -c $APP_CONFIG -o /out --no-restore

FROM mcr.microsoft.com/dotnet/runtime:7.0-alpine
RUN apk upgrade musl
RUN adduser --disabled-password --home /app --gecos '' appuser && chown -R appuser /app
USER appuser
WORKDIR /app
COPY --from=build /out ./

ENTRYPOINT ["dotnet", "DAL.dll"]