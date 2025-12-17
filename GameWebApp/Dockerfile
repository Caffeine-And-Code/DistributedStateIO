# ---- build/publish stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy everything and publish
# (If you want better layer caching, split COPY into csproj first + restore, then COPY . .)
COPY . .

# Change this to your .csproj path
ARG PROJECT=GameWebApp/GameWebApp.csproj
RUN dotnet publish "$PROJECT" -c Release -o /app/publish

# ---- runtime stage (nginx) ----
FROM nginx:alpine AS final

# Nginx serves static content from /usr/share/nginx/html by default [web:2]
WORKDIR /usr/share/nginx/html

# Copy published static site (Blazor WASM output is in wwwroot) [web:18]
COPY --from=build /app/publish/wwwroot ./

# SPA fallback so client-side routes work (try /index.html when a file isn't found) [web:18]
COPY GameWebApp/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

