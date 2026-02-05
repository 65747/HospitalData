Как использовать в Unity
========================

1. Скопируй в проект Unity:
   - Всю папку HospitalData (Models, Storage и Unity), например в Assets/HospitalData/

2. Положи JSON-файлы в StreamingAssets:
   - Создай папку Assets/StreamingAssets/HospitalData/
   - Скопируй туда: les_patients.json, les_superviseur.json, sessions.json
   (Либо положи их прямо в Assets/StreamingAssets/ и в инспекторе у скрипта поле "Data Folder" оставь пустым.)

3. В сцене:
   - Создай пустой GameObject (ПКМ в Hierarchy -> Create Empty)
   - Назови, например, "DatabaseTester"
   - Перетащи на него скрипт HospitalDataTestRunner (из папки Unity)

4. Запусти сцену (Play). В консоли Unity появятся все данные, которые загружают менеджеры.

5. Опционально: в инспекторе у DatabaseTester можно включить "Verbose" для подробного вывода по каждому полю.

Важно: для Unity нужна поддержка System.Text.Json (Unity 2021+ с .NET Standard 2.1 или .NET 4.x).
Если будут ошибки компиляции из-за Json, можно подключить NuGet/package для System.Text.Json.
