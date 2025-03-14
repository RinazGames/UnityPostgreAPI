# Используем официальный образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Указываем рабочую директорию
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY ["UnityPostreAPI/UnityPostreAPI.csproj", "UnityPostreAPI/"]
RUN dotnet restore "UnityPostreAPI/UnityPostreAPI.csproj"

# Копируем все остальные файлы
COPY . .

# Публикуем приложение
RUN dotnet publish "UnityPostreAPI/UnityPostreAPI.csproj" -c Release -o /app/publish

# Используем официальный образ .NET Runtime для выполнения приложения
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=build /app/publish .

# Открываем порт для приложения
EXPOSE 80

# Запускаем приложение
ENTRYPOINT ["dotnet", "UnityPostreAPI.dll"]

