# Используем официальный образ .NET для ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

# Используем образ SDK для сборки проекта
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Копируем файлы проекта и восстанавливаем зависимости
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Восстановление зависимостей для WebAPI
RUN dotnet restore "WebAPI/WebAPI.csproj"

# Копируем оставшиеся файлы проекта
COPY . .

# Собираем проект
WORKDIR "/src/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

# Запускаем тесты (опционально, можно убрать на этапе продакшн)
RUN dotnet test /src/WebAPI.Tests/WebAPI.Tests.csproj

# Публикуем проект (собираем финальный образ)
FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish

# Финальная стадия: использование базового образа ASP.NET Core для запуска приложения
FROM base AS final
WORKDIR /app

# Копируем опубликованные файлы из стадии публикации
COPY --from=publish /app/publish .

# Точка входа для запуска приложения
ENTRYPOINT ["dotnet", "WebAPI.dll"]
