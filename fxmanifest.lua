fx_version 'bodacious'
game 'gta5'

files {
    'static/index.css',
    'static/index.js',
    'static/index.html',
    'static/money.png',
    'static/water.png',
    'static/cap.png',
    'static/tshirt.png',
    'static/rubber-gloves.png',
    'static/trousers.png',
    'static/sneakers.png',
    'static/backpack.png',
    'static/AirJordan11.png',
    'Newtonsoft.Json.dll'
}

client_script 'Client/*.net.dll'
server_script 'Server/*.net.dll'

author 'zabbix-byte'
version '1.0.0'
description 'ztzbx inventory system'

ui_pages {
    'static/index.html'
}

dependencies {
    "fivem-mysql",
    "core-ztzbx",
    "notification"
}

client_exports {
    "getItemMeta",
    "addItemInventory"
}