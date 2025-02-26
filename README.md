Как запустить приложение
1. Скачайте проект
Склонируйте репозиторий командой git clone https://github.com/yourusername/events-web-application.git, затем перейдите в папку проекта командой cd events-web-application.

2. Проверьте ресурсы
Убедитесь, что на компьютере есть минимум 4 ГБ свободной памяти (в диспетчере задач на вкладке "Производительность").
Откройте Docker Desktop → Settings → Resources и выделите Docker минимум 4 ГБ памяти.
3. Запустите приложение
В корневой папке проекта (где лежат Dockerfile и docker-compose.yml) выполните команду docker-compose up --build. Это соберёт и запустит приложение и базу данных.

Приложение будет доступно по адресу http://localhost:5000.
База данных — на localhost:1433 (если нужно подключиться извне).
4. Настройка базы данных (если нужно)
Если база EventsDb ещё не создана:

Запустите только базу командой docker-compose up -d mssql.
Примените миграции командой dotnet ef database update --project Infrastructure --startup-project WebApi (для этого временно измените строку подключения в appsettings.json на Server=localhost,1433;User Id=sa;Password=fiug2787tfgyfFtftyxnjb;TrustServerCertificate=True).
Перезапустите всё командой docker-compose down и затем docker-compose up --build.
5. Остановка
Чтобы остановить приложение, выполните docker-compose down. Если нужно удалить данные базы, добавьте -v: docker-compose down -v.

Подробности настройки
WebAPI работает на порту 5000 (снаружи) и 80 (внутри контейнера).
MSSQL использует образ 2019-latest, логин sa и пароль fiug2787tfgyfFtftyxnjb.
Данные базы сохраняются в томе mssql-data.
Если что-то не работает
База не запускается из-за памяти
Если в логах написано "requires at least 2000 megabytes of memory":

Освободите память на компьютере (закройте лишние программы).
Увеличьте память в Docker Desktop до 4 ГБ и перезапустите docker-compose up --build.
Приложение не подключается к базе
Проверьте логи базы командой docker logs events_web_application-mssql-1.
Убедитесь, что в строке подключения указано Server=mssql.
Другие проблемы
Посмотрите логи командой docker-compose logs и обратитесь за помощью, если что-то непонятно.

Для разработки без Docker
Если хотите запустить локально, обновите строку подключения в appsettings.json под вашу базу (например, Server=localhost;...) и запускайте через dotnet run в папке WebApi.