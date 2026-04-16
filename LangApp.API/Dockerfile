FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["LangApp.API/LangApp.API.csproj", "LangApp.API/"]
COPY ["LangApp.Core/LangApp.Core.csproj", "LangApp.Core/"] 

RUN dotnet restore "LangApp.API/LangApp.API.csproj"

COPY LangApp.API/ LangApp.API/
COPY LangApp.Core/ LangApp.Core/

WORKDIR "/src/LangApp.API"
RUN dotnet publish "LangApp.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LangApp.API.dll"]