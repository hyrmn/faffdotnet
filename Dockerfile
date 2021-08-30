FROM node:lts-buster-slim AS node-build
ENV NODE_ENV=production
WORKDIR /assets
COPY ["package.json", "package-lock.json", "postcss.config.js", "tailwind.config.js", "./"]
COPY ./src/Faff/css ./src/Faff/css
COPY ./src/Faff/Pages ./src/Faff/Pages
RUN npm ci --prefer-offline --no-audit --progress=false
RUN npm run build

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
COPY --from=node-build /assets/src/Faff/wwwroot/css/site.css ./wwwroot/css/site.css
COPY --from=publish /dist .

VOLUME /app/data

ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

ENTRYPOINT ["./Faff"]