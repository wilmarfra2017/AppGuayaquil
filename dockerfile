# Etapa 1: Imagen base para la construcción con SDK de .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar NuGet.Config
COPY NuGet.Config ./

# Copiar los archivos de proyecto
COPY ["AppGuayaquil.Api/AppGuayaquil.Api.csproj", "AppGuayaquil.Api/"]
COPY ["AppGuayaquil.Application/AppGuayaquil.Application.csproj", "AppGuayaquil.Application/"]
COPY ["AppGuayaquil.Domain/AppGuayaquil.Domain.csproj", "AppGuayaquil.Domain/"]
COPY ["AppGuayaquil.Infrastructure/AppGuayaquil.Infrastructure.csproj", "AppGuayaquil.Infrastructure/"]

# Copiar el paquete local LogConfig al contenedor
COPY LocalPackages /src/LocalPackages


# Restaurar las dependencias
RUN dotnet restore "AppGuayaquil.Api/AppGuayaquil.Api.csproj"

# Copiar todo el código fuente y compilar
COPY . .
WORKDIR "/src/AppGuayaquil.Api"
RUN dotnet build -c Release -o /app/build

# Etapa 2: Publicar la aplicación
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Etapa 3: Imagen final para el runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiar la publicación desde la etapa de construcción
COPY --from=publish /app/publish .

# Comando de inicio de la aplicación
ENTRYPOINT ["dotnet", "AppGuayaquil.Api.dll"]
