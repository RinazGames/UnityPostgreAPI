# Используем .NET 8 SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем файл проекта и восстанавливаем зависимости
COPY ["UnityPostreAPI.csproj", "./"]
RUN dotnet restore "UnityPostreAPI.csproj"

# Копируем остальные файлы и собираем приложение
COPY . .
RUN dotnet publish "UnityPostreAPI.csproj" -c Release -o /app/publish

# Используем ASP.NET 8.0 для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Открываем порт 80
EXPOSE 80

# Запускаем приложение
ENTRYPOINT ["dotnet", "UnityPostreAPI.dll"]
