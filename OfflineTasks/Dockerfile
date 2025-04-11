FROM mcr.microsoft.com/dotnet/sdk:9.0 AS setup
RUN dotnet workload install wasm-tools
RUN apt-get update -y
RUN apt-get install -y python3
WORKDIR /src

FROM setup as publish
COPY . .
WORKDIR /src/OfflineTasks
RUN dotnet publish "OfflineTasks.csproj" -c Release -o /publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf