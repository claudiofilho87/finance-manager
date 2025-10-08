FROM node:20-alpine AS builder
WORKDIR /app
COPY Frontend/package*.json ./
RUN npm ci --legacy-peer-deps
COPY Frontend/. ./
RUN npm run build

FROM nginx:1.29-alpine
COPY --from=builder /app/dist /usr/share/nginx/html
COPY Docker/nginx/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]