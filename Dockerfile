# Используем официальный образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Устанавливаем рабочую директорию
WORKDIR /src

# Копируем файл csproj и восстанавливаем зависимости
COPY UnityPostreAPI.csproj ./
RUN dotnet restore "./UnityPostreAPI.csproj"

# Копируем остальные файлы и публикуем приложение
COPY . .
RUN dotnet publish "./UnityPostreAPI.csproj" -c Release -o /app/publish

# Используем официальный образ ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Открываем порт 80
EXPOSE 80

# Задаем команду для запуска приложения
ENTRYPOINT ["dotnet", "UnityPostreAPI.dll"]
