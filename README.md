## MushroomsGuesser
Консольно приложение, которое отгадывает гриб, загаданный игроком <br>
Возможности: <br>
1. Угадывание гриба  <br>
2. Если гриба нет в базе данных (не удалось угадать гриб), можно вручную внести его название и вопрос, ответ на который поможет отличить загаданный гриб <br>
3. Поиск конкретного гриба в базе данных <br>
4. Информация об ответе (объяснение того как приложение пришло к определённому ответу в предыдущей игре) <br>
5. Просмотр базы данных <br>

База данных хранится в виде json-файла mushrooms <br>
```
{"id":1,"Name":"У этого гриба красная шапочка с белыми точками?","IsQuestion":true,"Yes":2,"No":3}
{"id":2,"Name":"Мухомор","IsQuestion":false,"Yes":-1,"No":7}
{"id":3,"Name":"Название гриба содержит в себе название животного?","IsQuestion":true,"Yes":4,"No":-1}
{"id":4,"Name":"Лисичка","IsQuestion":false,"Yes":-1,"No":5}
{"id":5,"Name":"У этого гриба края шляпки светлее, чем центр, а также ровные и гладкие?","IsQuestion":true,"Yes":6,"No":-1}
{"id":6,"Name":"Ложная лисичка","IsQuestion":false,"Yes":-1,"No":-1}
{"id":7,"Name":"то же, что и мухомор, но назыается по-другому","IsQuestion":true,"Yes":8,"No":-1}
{"id":8,"Name":"Гриб с белыми точками","IsQuestion":false,"Yes":-1,"No":-1}
```
