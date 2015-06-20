PoeHud
======

Reads data from Path of Exile client application and displays it on transparent overlay, while you play PoE.
Without writing to it so no map hack, disabling particles, zoom hack, fullbright.

### Requirements
* .NET framerwork v.4.5 or newer (you already have it on Windows 8+)
* Windows Vista or newer (XP won't work)
* Path of Exile should be running in Windowed or Windowed Fullscreen mode (the pure Fullscreen mode does not let PoeHUD draw anything over the game window)
* Windows Aero transparency effects must be enabled. (If you get a black screen this is the issue)

### Available features
* Health bars
* Icons on minimap
* Icons on large map
* Item alerts
* Advenced tooltip: item level, item mods, weapon DPS
* Boss warnings
* XP per hours
* Preload alerts
* DPS meter
* Monster kills counter
* Inventory preview

### Item alert settings
The file config/crafting_bases.txt has the following syntax:
`Name,[Level],[Quality],[Rarity1,[Rarity2,[Rarity3]]]`

Examples of valid declarations:
```
Vaal Regalia,78
Corsair Sword,78,10
Gold Ring,75,,White,Rare
Ironscale Gauntlets,,10,White,Magic
Quicksilver Flask,1,5
Portal Scroll
Iron Ring
```
Also the mods used for mobs and items are listed in Content.ggpk\Data\Mods.dat.


### Отзывы
>Эта программа читает память, и выдает данные, которые не предусмотрены стандартным геймплеем. Ты не можешь видеть рефлект под землей и должен играть осторожно, программа предупреждает о рефлекте заранее. Ты не можешь знать о наличии случайных экзайлов на карте и их имена, программа сообщит тебе о них как только ты вошел в локацию.
С читом можно играть в разы менее осторожно и агрессивно. Ты зашел на карту, ты видишь всех монстров до того как они появились у тебя на экране, а если ты какой-нибудь лучник то ты можешь расстреливать сложных монстров вне экрана даже ни разу не увидев их. ©cyfer.russia
