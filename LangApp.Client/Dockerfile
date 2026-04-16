FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY LangApp.Client/LangApp.Client.csproj LangApp.Client/
RUN dotnet restore LangApp.Client/LangApp.Client.csproj

COPY LangApp.Client/ LangApp.Client/

WORKDIR /src/LangApp.Client
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine
COPY --from=build /app/publish /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]