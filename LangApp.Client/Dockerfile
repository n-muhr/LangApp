FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY LangApp.Client/LangApp.Client.csproj LangApp.Client/
RUN dotnet restore LangApp.Client/LangApp.Client.csproj

COPY LangApp.Client/ LangApp.Client/

WORKDIR /src/LangApp.Client
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "LangApp.Client.dll"]
