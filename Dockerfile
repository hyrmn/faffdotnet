FROM node:lts-buster-slim AS node-restore
ENV NODE_ENV=production
WORKDIR /assets
COPY ["package.json", "package-lock.json", "./"]
RUN npm ci --prefer-offline --no-audit --progress=false

FROM node-restore AS node-build
COPY ["postcss.config.js", "tailwind.config.js", "./"]
COPY ./src/css ./src/css
COPY ./src/Pages ./src/Pages
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS dotnet-restore
WORKDIR /work
COPY Faff.sln .
COPY ./src/Faff.csproj ./src/
COPY ./test/Faff.Tests.csproj ./test/
RUN dotnet restore -r linux-musl-x64

FROM dotnet-restore AS test
COPY ./src/ ./src
COPY ./test/ ./test
RUN dotnet build --no-restore
RUN dotnet test -r linux-musl-x64 --no-restore --no-build

FROM dotnet-restore AS publish
COPY ./src/ ./src
COPY ./test/ ./test
RUN dotnet build ./src/Faff.csproj -c release -o /build -r linux-musl-x64  --self-contained false --no-restore
RUN dotnet publish -c release -o /dist -r linux-musl-x64 --self-contained false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
WORKDIR /app
COPY --from=node-build /assets/src/wwwroot/css/site.css ./wwwroot/css/site.css
COPY --from=publish /dist .

VOLUME /app/data

ENV ASPNETCORE_ENVIRONMENT=production
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

ENTRYPOINT ["./Faff"]