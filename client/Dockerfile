FROM node:13.12.0 AS build
WORKDIR /app

COPY . ./
RUN npm install
RUN npm run publish

FROM socialengine/nginx-spa
COPY --from=build /app/index.html /app
COPY --from=build /app/index.js   /app
COPY --from=build /app/public     /app/public
