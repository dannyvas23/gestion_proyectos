# ============================================================
# Dockerfile — Frontend Angular 17 + Nginx (multi-stage build)
#
# Para defender:
# - Etapa 1 (build): usa Node 20 para compilar Angular.
#   'npm ci' instala dependencias exactas del package-lock.json (más rápido y determinista).
#   'npx ng build --configuration=production' genera los archivos estáticos optimizados.
# - Etapa 2 (nginx): copia los archivos compilados a Nginx.
#   nginx.conf configura el reverse proxy al backend y el fallback a index.html para SPA.
# ============================================================

# Etapa 1: Build
FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npx ng build --configuration=production

# Etapa 2: Nginx
FROM nginx:alpine AS runtime
COPY --from=build /app/dist/gestion_proyectos_web/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
