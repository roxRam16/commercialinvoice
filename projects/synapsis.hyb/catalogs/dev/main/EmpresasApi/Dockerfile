# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copiamos el archivo .csproj y restauramos dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiamos el resto de los archivos y compilamos
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app

# Copiamos la app publicada desde la etapa anterior
COPY --from=build /app/publish .

# Exponemos el puerto (Kestrel escucha por defecto en el 80)
EXPOSE 80

# Comando para ejecutar la app con Kestrel
ENTRYPOINT ["dotnet", "CompaniesApi.dll"]
