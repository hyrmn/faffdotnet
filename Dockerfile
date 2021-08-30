FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS restore
WORKDIR /src
COPY ./src .

RUN dotnet restore -r linux-musl-x64

FROM restore AS test
COPY . ./
RUN dotnet build --no-restore
RUN dotnet test -r linux-musl-x64 --no-restore --no-build

FROM restore AS publish
RUN dotnet build /src/Faff/Faff.csproj -c release -o /build -r linux-musl-x64  --self-contained false --no-restore
RUN dotnet publish -c release -o /dist -r linux-musl-x64 --self-contained false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app
COPY --from=publish /dist .

VOLUME /app/data

ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

ENTRYPOINT ["./Faff"]