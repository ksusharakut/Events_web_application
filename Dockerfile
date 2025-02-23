# Используем официальный образ .NET 8 SDK как базовый для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем файл решения
COPY Events_web_application.sln .

# Явно копируем все .csproj файлы из соответствующих папок
COPY Application/*.csproj Application/
COPY Domain/*.csproj Domain/
COPY EventApp/*.csproj EventApp/
COPY Infrastructure/*.csproj Infrastructure/
COPY Infrastructure.Tests/*.csproj Infrastructure.Tests/
COPY WebApi/*.csproj WebApi/

# Выводим содержимое текущей директории для отладки
RUN echo "Содержимое директории /app:" && ls -la

# Восстанавливаем зависимости для всех проектов через файл решения
RUN dotnet restore Events_web_application.sln --verbosity detailed

# Копируем весь исходный код
COPY . .

# Собираем приложение в режиме Release
RUN dotnet publish WebApi/WebApi.csproj -c Release -o out

# Используем образ runtime для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Указываем порт, который будет использоваться (по умолчанию для ASP.NET Core — 80 или 5000)
EXPOSE 80

# Указываем, что приложение должно использовать только HTTP
ENV ASPNETCORE_URLS=http://+:80

# Запускаем приложение
ENTRYPOINT ["dotnet", "WebApi.dll"]