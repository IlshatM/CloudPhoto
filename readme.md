## Запуск на Linux

### 1. Скопировать этот репозиторий репозиторий
### 2. Прописать разрешение на запуск
`chmod 777 {путь папки cloud_photo_linux}/CloudPhoto`

Пример, `chmod 777 /usr/bin/cloud_photo_linux/CloudPhoto`

### 3. Прописать алиас
1. `cd ~`
2. `nano .bashrc`
3. Вставить строку `alias cloudphoto='{путь папки cloud_photo_linux}/CloudPhoto'`
Пример, `alias cloudphoto='/usr/bin/cloud_photo_linux/CloudPhoto'`
4. Сохранить изменения.

### Готово!
