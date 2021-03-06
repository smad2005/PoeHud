using System;
using System.Linq;
using System.Windows.Forms;

using PoeHUD.Controllers;
using PoeHUD.Framework;
using PoeHUD.Framework.InputHooks;
using PoeHUD.Hud.AdvancedTooltip;
using PoeHUD.Hud.Health;
using PoeHUD.Hud.Loot;
using PoeHUD.Hud.Settings;
using PoeHUD.Hud.UI;

using SharpDX;

namespace PoeHUD.Hud.Menu
{
    public class MenuPlugin : Plugin<MenuSettings>
    {
        private readonly SettingsHub settingsHub;

        private readonly Action<MouseInfo> onMouseDown, onMouseUp, onMouseMove;

        private bool holdKey;

        private RootButton root;

        public MenuPlugin(GameController gameController, Graphics graphics, SettingsHub settingsHub)
            : base(gameController, graphics, settingsHub.MenuSettings)
        {
            this.settingsHub = settingsHub;
            CreateMenu();
            MouseHook.MouseDown += onMouseDown = info => info.Handled = OnMouseEvent(MouseEventID.LeftButtonDown, info.Position);
            MouseHook.MouseUp += onMouseUp = info => info.Handled = OnMouseEvent(MouseEventID.LeftButtonUp, info.Position);
            MouseHook.MouseMove += onMouseMove = info => info.Handled = OnMouseEvent(MouseEventID.MouseMove, info.Position);
        }

        public override void Dispose()
        {
            MouseHook.MouseDown -= onMouseDown;
            MouseHook.MouseUp -= onMouseUp;
            MouseHook.MouseMove -= onMouseMove;
        }

        public override void Render()
        {
            if (!holdKey && WinApi.IsKeyDown(Keys.F12))
            {
                holdKey = true;
                Settings.Enable.Value = !Settings.Enable.Value;
                if (!Settings.Enable.Value)
                {
                    SettingsHub.Save(settingsHub);
                }
            }
            else if (holdKey && !WinApi.IsKeyDown(Keys.F12))
            {
                holdKey = false;
            }

            if (Settings.Enable)
            {
                root.Render(Graphics);
            }
        }

        private static MenuItem AddChild(MenuItem parent, string text, ToggleNode node, string key = null, Func<MenuItem, bool> hide = null)
        {
            var item = new ToggleButton(parent,text, node,key, hide);
            parent.AddChild(item);
            return item;
        }

        private static MenuItem AddChild(MenuItem parent, FileNode path)
        {
            var item = new FileButton(path);
            parent.AddChild(item);
            return item;
        }



        private static void AddChild(MenuItem parent, string text, ColorNode node)
        {
            var item = new ColorButton(text, node);
            parent.AddChild(item);
        }

        private static void AddChild<T>(MenuItem parent, string text, RangeNode<T> node) where T : struct
        {
            var item = new Picker<T>(text, node);
            parent.AddChild(item);
        }

        private void CreateMenu()
        {
            root = new RootButton(new Vector2(Settings.X, Settings.Y));

            // Health bars
            HealthBarSettings healthBarPlugin = settingsHub.HealthBarSettings;
            MenuItem healthMenu = AddChild(root, "Health bars", healthBarPlugin.Enable);
            MenuItem playersMenu = AddChild(healthMenu, "Players", healthBarPlugin.Players.Enable);
            MenuItem enemiesMenu = AddChild(healthMenu, "Enemies", healthBarPlugin.ShowEnemies);
            MenuItem minionsMenu = AddChild(healthMenu, "Minions", healthBarPlugin.Minions.Enable);
            AddChild(healthMenu, "Show ES", healthBarPlugin.ShowES);
            AddChild(healthMenu, "Show in town", healthBarPlugin.ShowInTown);
            MenuItem debuffPanelMenu = AddChild(healthMenu, "Show debuff panel", healthBarPlugin.ShowDebuffPanel);
            AddChild(debuffPanelMenu, "Icon size", healthBarPlugin.DebuffPanelIconSize);
            AddChild(playersMenu, "Print percents", healthBarPlugin.Players.ShowPercents);
            AddChild(playersMenu, "Print health text", healthBarPlugin.Players.ShowHealthText);
            AddChild(playersMenu, "Width", healthBarPlugin.Players.Width);
            AddChild(playersMenu, "Height", healthBarPlugin.Players.Height);
            MenuItem playerDPSMenu=AddChild(playersMenu, "Floating combat text", healthBarPlugin.Players.ShowFloatingCombatDamage);
            AddChild(playerDPSMenu, "Font size", healthBarPlugin.Players.FloatingCombatFontSize);
            AddChild(playerDPSMenu, "Damage Color", healthBarPlugin.Players.FloatingCombatDamageColor);
            AddChild(playerDPSMenu, "Heal Color", healthBarPlugin.Players.FloatingCombatHealColor);
            AddChild(playerDPSMenu, "Number of lines", healthBarPlugin.Players.FloatingCombatStackSize);
            AddChild(minionsMenu, "Print percents", healthBarPlugin.Minions.ShowPercents);
            AddChild(minionsMenu, "Print health text", healthBarPlugin.Minions.ShowHealthText);
            AddChild(minionsMenu, "Width", healthBarPlugin.Minions.Width);
            AddChild(minionsMenu, "Height", healthBarPlugin.Minions.Height);
            MenuItem minionsDPSMenu = AddChild(minionsMenu, "Floating combat text", healthBarPlugin.Minions.ShowFloatingCombatDamage);
            AddChild(minionsDPSMenu, "Font size", healthBarPlugin.Minions.FloatingCombatFontSize);
            AddChild(minionsDPSMenu, "Damage Color", healthBarPlugin.Minions.FloatingCombatDamageColor);
            AddChild(minionsDPSMenu, "Heal Color", healthBarPlugin.Minions.FloatingCombatHealColor);
            AddChild(minionsDPSMenu, "Number of lines", healthBarPlugin.Minions.FloatingCombatStackSize);
            MenuItem whiteEnemyMenu = AddChild(enemiesMenu, "White", healthBarPlugin.NormalEnemy.Enable);
            AddChild(whiteEnemyMenu, "Print percents", healthBarPlugin.NormalEnemy.ShowPercents);
            AddChild(whiteEnemyMenu, "Print health text", healthBarPlugin.NormalEnemy.ShowHealthText);
            AddChild(whiteEnemyMenu, "Width", healthBarPlugin.NormalEnemy.Width);
            AddChild(whiteEnemyMenu, "Height", healthBarPlugin.NormalEnemy.Height);
            MenuItem whiteEnemyDPSMenu = AddChild(whiteEnemyMenu, "Floating combat text", healthBarPlugin.NormalEnemy.ShowFloatingCombatDamage);
            AddChild(whiteEnemyDPSMenu, "Font size", healthBarPlugin.NormalEnemy.FloatingCombatFontSize);
            AddChild(whiteEnemyDPSMenu, "Damage Color", healthBarPlugin.NormalEnemy.FloatingCombatDamageColor);
            AddChild(whiteEnemyDPSMenu, "Heal Color", healthBarPlugin.NormalEnemy.FloatingCombatHealColor);
            AddChild(whiteEnemyDPSMenu, "Number of lines", healthBarPlugin.NormalEnemy.FloatingCombatStackSize);
            MenuItem magicEnemyMenu = AddChild(enemiesMenu, "Magic", healthBarPlugin.MagicEnemy.Enable);
            AddChild(magicEnemyMenu, "Print percents", healthBarPlugin.MagicEnemy.ShowPercents);
            AddChild(magicEnemyMenu, "Print health text", healthBarPlugin.MagicEnemy.ShowHealthText);
            AddChild(magicEnemyMenu, "Width", healthBarPlugin.MagicEnemy.Width);
            AddChild(magicEnemyMenu, "Height", healthBarPlugin.MagicEnemy.Height);
            MenuItem magicEnemyDPSMenu = AddChild(magicEnemyMenu, "Floating combat text", healthBarPlugin.MagicEnemy.ShowFloatingCombatDamage);
            AddChild(magicEnemyDPSMenu, "Font size", healthBarPlugin.MagicEnemy.FloatingCombatFontSize);
            AddChild(magicEnemyDPSMenu, "Damage Color", healthBarPlugin.MagicEnemy.FloatingCombatDamageColor);
            AddChild(magicEnemyDPSMenu, "Heal Color", healthBarPlugin.MagicEnemy.FloatingCombatHealColor);
            AddChild(magicEnemyDPSMenu, "Number of lines", healthBarPlugin.MagicEnemy.FloatingCombatStackSize);
            MenuItem rareEnemyMenu = AddChild(enemiesMenu, "Rare", healthBarPlugin.RareEnemy.Enable);
            AddChild(rareEnemyMenu, "Print percents", healthBarPlugin.RareEnemy.ShowPercents);
            AddChild(rareEnemyMenu, "Print health text", healthBarPlugin.RareEnemy.ShowHealthText);
            AddChild(rareEnemyMenu, "Width", healthBarPlugin.RareEnemy.Width);
            AddChild(rareEnemyMenu, "Height", healthBarPlugin.RareEnemy.Height);
            MenuItem rareEnemyDPSMenu = AddChild(rareEnemyMenu, "Floating combat text", healthBarPlugin.RareEnemy.ShowFloatingCombatDamage);
            AddChild(rareEnemyDPSMenu, "Font size", healthBarPlugin.RareEnemy.FloatingCombatFontSize);
            AddChild(rareEnemyDPSMenu, "Damage Color", healthBarPlugin.RareEnemy.FloatingCombatDamageColor);
            AddChild(rareEnemyDPSMenu, "Heal Color", healthBarPlugin.RareEnemy.FloatingCombatHealColor);
            AddChild(rareEnemyDPSMenu, "Number of lines", healthBarPlugin.RareEnemy.FloatingCombatStackSize);
            MenuItem uniquesEnemyMenu = AddChild(enemiesMenu, "Uniques", healthBarPlugin.UniqueEnemy.Enable);
            AddChild(uniquesEnemyMenu, "Print percents", healthBarPlugin.UniqueEnemy.ShowPercents);
            AddChild(uniquesEnemyMenu, "Print health text", healthBarPlugin.UniqueEnemy.ShowHealthText);
            AddChild(uniquesEnemyMenu, "Width", healthBarPlugin.UniqueEnemy.Width);
            AddChild(uniquesEnemyMenu, "Height", healthBarPlugin.UniqueEnemy.Height);
            MenuItem uniqueEnemyDPSMenu = AddChild(uniquesEnemyMenu, "Floating combat text", healthBarPlugin.UniqueEnemy.ShowFloatingCombatDamage);
            AddChild(uniqueEnemyDPSMenu, "Font size", healthBarPlugin.UniqueEnemy.FloatingCombatFontSize);
            AddChild(uniqueEnemyDPSMenu, "Damage Color", healthBarPlugin.UniqueEnemy.FloatingCombatDamageColor);
            AddChild(uniqueEnemyDPSMenu, "Heal Color", healthBarPlugin.UniqueEnemy.FloatingCombatHealColor);
            AddChild(uniqueEnemyDPSMenu, "Number of lines", healthBarPlugin.UniqueEnemy.FloatingCombatStackSize);

            // Map icons
            MenuItem mapIconsMenu = AddChild(root, "Map icons", settingsHub.MapIconsSettings.Enable);
            AddChild(mapIconsMenu, "Icons on minimap", settingsHub.MapIconsSettings.IconsOnMinimap);
            AddChild(mapIconsMenu, "Icons on large map", settingsHub.MapIconsSettings.IconsOnLargeMap);
            AddChild(mapIconsMenu, "Precious items", settingsHub.ItemAlertSettings.ShowItemOnMap);
            AddChild(mapIconsMenu, "Monsters", settingsHub.MonsterTrackerSettings.Monsters);
            AddChild(mapIconsMenu, "Minions", settingsHub.MonsterTrackerSettings.Minions);
            AddChild(mapIconsMenu, "Strongboxes", settingsHub.PoiTrackerSettings.Strongboxes);
            AddChild(mapIconsMenu, "Chests", settingsHub.PoiTrackerSettings.Chests);
            AddChild(mapIconsMenu, "Masters", settingsHub.PoiTrackerSettings.Masters);

            // Item Alert
            MenuItem itemAlertMenu = AddChild(root, "Item alert", settingsHub.ItemAlertSettings.Enable);
            var itemAlertStaticMenuList = new[] {"Alternative", "Play sound", "Show text", "Hide Others", "Show border" };
            MenuItem alternative = AddChild(itemAlertMenu, itemAlertStaticMenuList[0], settingsHub.ItemAlertSettings.Alternative,null, y=> itemAlertStaticMenuList.All(x => x != (y as ToggleButton)?.Name))  ;
            AddChild(alternative,  settingsHub.ItemAlertSettings.FilePath);
            AddChild(alternative, "With border", settingsHub.ItemAlertSettings.WithBorder);
            AddChild(alternative, "With sound", settingsHub.ItemAlertSettings.WithSound);
            AddChild(itemAlertMenu, itemAlertStaticMenuList[1], settingsHub.ItemAlertSettings.PlaySound);
            MenuItem alertTextMenu = AddChild(itemAlertMenu, itemAlertStaticMenuList[2], settingsHub.ItemAlertSettings.ShowText);
            AddChild(alertTextMenu, "Font size", settingsHub.ItemAlertSettings.TextSize);
            AddChild(itemAlertMenu, itemAlertStaticMenuList[3], settingsHub.ItemAlertSettings.HideOthers);
            BorderSettings borderSettings = settingsHub.ItemAlertSettings.BorderSettings;
            MenuItem showBorderMenu = AddChild(itemAlertMenu, itemAlertStaticMenuList[4], borderSettings.Enable);
            AddChild(showBorderMenu, "Border width", borderSettings.BorderWidth);
            AddChild(showBorderMenu, "Border color:", borderSettings.BorderColor);
            AddChild(showBorderMenu, "Cn't pck up brd color:", borderSettings.CantPickUpBorderColor);
            AddChild(showBorderMenu, "Not my item brd color:", borderSettings.NotMyItemBorderColor);
            AddChild(showBorderMenu, "Show timer", borderSettings.ShowTimer);
            AddChild(showBorderMenu, "Timer text size", borderSettings.TimerTextSize);
            AddChild(itemAlertMenu, "Rares", settingsHub.ItemAlertSettings.Rares);
            AddChild(itemAlertMenu, "Uniques", settingsHub.ItemAlertSettings.Uniques);
            AddChild(itemAlertMenu, "Currency", settingsHub.ItemAlertSettings.Currency);
            AddChild(itemAlertMenu, "Maps", settingsHub.ItemAlertSettings.Maps);
            AddChild(itemAlertMenu, "RGB", settingsHub.ItemAlertSettings.Rgb);
            AddChild(itemAlertMenu, "Crafting bases", settingsHub.ItemAlertSettings.Crafting);
            AddChild(itemAlertMenu, "Divination cards", settingsHub.ItemAlertSettings.DivinationCards);
            AddChild(itemAlertMenu, "Jewels", settingsHub.ItemAlertSettings.Jewels);
            QualityItemsSettings qualityItemsSettings = settingsHub.ItemAlertSettings.QualityItems;
            MenuItem qualityMenu = AddChild(itemAlertMenu, "Show quality items", qualityItemsSettings.Enable);
            MenuItem qualityWeaponMenu = AddChild(qualityMenu, "Weapons", qualityItemsSettings.Weapon.Enable);
            AddChild(qualityWeaponMenu, "Min. quality", qualityItemsSettings.Weapon.MinQuality);
            MenuItem qualityArmourMenu = AddChild(qualityMenu, "Armours", qualityItemsSettings.Armour.Enable);
            AddChild(qualityArmourMenu, "Min. quality", qualityItemsSettings.Armour.MinQuality);
            MenuItem qualityFlaskMenu = AddChild(qualityMenu, "Flasks", qualityItemsSettings.Flask.Enable);
            AddChild(qualityFlaskMenu, "Min. quality", qualityItemsSettings.Flask.MinQuality);
            MenuItem qualitySkillGemMenu = AddChild(qualityMenu, "Skill gems", qualityItemsSettings.SkillGem.Enable);
            AddChild(qualitySkillGemMenu, "Min. quality", qualityItemsSettings.SkillGem.MinQuality);
           

            // Advanced tooltip
            AdvancedTooltipSettings tooltipSettings = settingsHub.AdvancedTooltipSettings;
            MenuItem tooltipMenu = AddChild(root, "Adv. tooltip", tooltipSettings.Enable);
            MenuItem itemLevelMenu = AddChild(tooltipMenu, "Item level", tooltipSettings.ItemLevel.Enable);
            AddChild(itemLevelMenu, "Font size", tooltipSettings.ItemLevel.TextSize);
            MenuItem itemModsMenu = AddChild(tooltipMenu, "Item mods", tooltipSettings.ItemMods.Enable,"F9");
            AddChild(itemModsMenu, "Mods size", tooltipSettings.ItemMods.ModTextSize);
            MenuItem weaponDpsMenu = AddChild(tooltipMenu, "Weapon DPS", tooltipSettings.WeaponDps.Enable);
            AddChild(weaponDpsMenu, "DPS size", tooltipSettings.WeaponDps.DpsTextSize);
            AddChild(weaponDpsMenu, "DPS name size", tooltipSettings.WeaponDps.DpsNameTextSize);

            // Monster tracker Options
            MenuItem MonsterTrackerMenu = AddChild(root, "Monster Tracker", settingsHub.MonsterTrackerSettings.Enable);
            AddChild(MonsterTrackerMenu, "Sound warning", settingsHub.MonsterTrackerSettings.PlaySound);
            MenuItem warningTextMenu = AddChild(MonsterTrackerMenu, "Text warning", settingsHub.MonsterTrackerSettings.ShowText);
            AddChild(warningTextMenu, "Font size", settingsHub.MonsterTrackerSettings.TextSize);
            AddChild(warningTextMenu, "Default text color:", settingsHub.MonsterTrackerSettings.DefaultTextColor);
            AddChild(warningTextMenu, "Background color:", settingsHub.MonsterTrackerSettings.BackgroundColor);
            AddChild(warningTextMenu, "Position X", settingsHub.MonsterTrackerSettings.TextPositionX);
            AddChild(warningTextMenu, "Position Y", settingsHub.MonsterTrackerSettings.TextPositionY);

            // Xph Display
            MenuItem xpRateMenu = AddChild(root, "Xph Display", settingsHub.XpRateSettings.Enable);
            MenuItem xpReceivingMenu = AddChild(xpRateMenu, "Show XP receiving", settingsHub.XpRateSettings.ShowXpReceiving);
            AddChild(xpReceivingMenu, "With level penalty", settingsHub.XpRateSettings.LevelPenalty);
            AddChild(xpReceivingMenu, "With party Penalty", settingsHub.XpRateSettings.PartyPenalty);
            AddChild(xpRateMenu, "Font size", settingsHub.XpRateSettings.TextSize);
            AddChild(xpRateMenu, "Background color:", settingsHub.XpRateSettings.BackgroundColor);

            // Preload Alert
            MenuItem preloadMenu = AddChild(root, "Preload Alert", settingsHub.PreloadAlertSettings.Enable);
            AddChild(preloadMenu, "Font size", settingsHub.PreloadAlertSettings.TextSize);
            AddChild(preloadMenu, "Default text color:", settingsHub.PreloadAlertSettings.DefaultTextColor);
            AddChild(preloadMenu, "Corrupted area color:", settingsHub.PreloadAlertSettings.CorruptedAreaColor);
            AddChild(preloadMenu, "Background color:", settingsHub.PreloadAlertSettings.BackgroundColor);
            
            // Show DPS
            MenuItem showDpsMenu = AddChild(root, "Show DPS", settingsHub.DpsMeterSettings.Enable);
            AddChild(showDpsMenu, "DPS font size", settingsHub.DpsMeterSettings.DpsTextSize);
            AddChild(showDpsMenu, "Peak DPS font size", settingsHub.DpsMeterSettings.PeakDpsTextSize);
            AddChild(showDpsMenu, "Background color:", settingsHub.DpsMeterSettings.BackgroundColor);

            // Show monster kills
            MenuItem showMonsterKillsMenu = AddChild(root, "Show MK", settingsHub.KillsCounterSettings.Enable);
            AddChild(showMonsterKillsMenu, "Show details", settingsHub.KillsCounterSettings.ShowDetail);

            // show Item-Drop-Counter // doesnt work yet
            //MenuItem showItemDropMenu = AddChild(root, "Show ItemDrops", settingsHub.ItemCounterSettings.Enable);
            //AddChild(showItemDropMenu, "Show details", settingsHub.ItemCounterSettings.ShowDetail);

            // Show inventory preview
            MenuItem showInventoryPreviewMenu = AddChild(root, "Show inv preview", settingsHub.InventoryPreviewSettings.Enable);
            AddChild(showInventoryPreviewMenu, "Auto update", settingsHub.InventoryPreviewSettings.AutoUpdate);
            AddChild(showInventoryPreviewMenu, "Free cell color", settingsHub.InventoryPreviewSettings.CellFreeColor);
            AddChild(showInventoryPreviewMenu, "Used cell color", settingsHub.InventoryPreviewSettings.CellUsedColor);
            AddChild(showInventoryPreviewMenu, "Cell size", settingsHub.InventoryPreviewSettings.CellSize);
            AddChild(showInventoryPreviewMenu, "Cell padding", settingsHub.InventoryPreviewSettings.CellPadding);
            AddChild(showInventoryPreviewMenu, "Position X", settingsHub.InventoryPreviewSettings.PositionX);
            AddChild(showInventoryPreviewMenu, "Position Y", settingsHub.InventoryPreviewSettings.PositionY);
        }

        private bool OnMouseEvent(MouseEventID id, Point position)
        {
            if (!Settings.Enable || !GameController.Window.IsForeground())
            {
                return false;
            }

            Vector2 mousePosition = GameController.Window.ScreenToClient(position.X, position.Y);
            return root.OnMouseEvent(id, mousePosition);
        }
    }
}
