using System;
using System.Collections.Generic;
using PoeHUD.Poe.Elements;
using PoeHUD.Poe.UI;
using PoeHUD.Poe.UI.Elements;

namespace PoeHUD.Poe.RemoteMemoryObjects
{
    public class IngameUIElements : RemoteMemoryObject
    {
        public Element Uknown0
        {
            get { return ReadObjectAt<Element>(0x40); }
        }

        public Element HpGlobe
        {
            get { return ReadObjectAt<Element>(0x44); }
        }

        public Element ManaGlobe
        {
            get { return ReadObjectAt<Element>(0x48); }
        }

        public Element Flasks
        {
            get { return ReadObjectAt<Element>(0x50); }
        }

        public Element XpBar
        {
            get { return ReadObjectAt<Element>(0x54); }
        }

        public Element MenuButton
        {
            get { return ReadObjectAt<Element>(0x58); }
        }

        public Element ShopButton
        {
            get { return ReadObjectAt<Element>(0x84); }
        }

        public Element HideoutEditButton
        {
            get { return ReadObjectAt<Element>(0x88); }
        }

        public Element HideoutStashButton
        {
            get { return ReadObjectAt<Element>(0x8C); }
        }

        public Element Mouseposition
        {
            get { return ReadObjectAt<Element>(0xA4); }
        }

        public Element ActionButtons
        {
            get { return ReadObjectAt<Element>(0xA8); }
        }

        public Element Chat
        {
            get { return ReadObjectAt<Element>(0xe8); }
        }

        public Element QuestTracker
        {
            get { return ReadObjectAt<Element>(0xf0); }
        }

        public Element MtxInventory
        {
            get { return ReadObjectAt<Element>(0xf4); }
        }

        public Element MtxShop
        {
            get { return ReadObjectAt<Element>(0xf8); }
        }

        public Element InventoryPanel
        {
            get { return ReadObjectAt<Element>(0x108 + M.Server.IngameUIElementsOffset); }
        }

        public Element StashPanel
        {
            get { return ReadObjectAt<Element>(0x108); }
        }

        public Element SocialPanel
        {
            get { return ReadObjectAt<Element>(0x114); }
        }

        public Element TreePanel
        {
            get { return ReadObjectAt<Element>(0x11c + M.Server.IngameUIElementsOffset); }
        }

        public Element CharacterPanel
        {
            get { return ReadObjectAt<Element>(0x11c); }
        }

        public Element OptionsPanel
        {
            get { return ReadObjectAt<Element>(0x120); }
        }

        public Element AchievementsPanel
        {
            get { return ReadObjectAt<Element>(0x124); }
        }

        public Element WorldPanel
        {
            get { return ReadObjectAt<Element>(0x12c); }
        }

        public Map Map
        {
            get { return ReadObjectAt<Map>(0x134 + M.Server.IngameUIElementsOffset); }
        }

        public IEnumerable<ItemsOnGroundLabelElement> ItemsOnGroundLabels
        {
            get
            {
                var itemsOnGroundLabelRoot = ReadObjectAt<ItemsOnGroundLabelElement>(0x138 + M.Server.IngameUIElementsOffset);
                return itemsOnGroundLabelRoot.Children;
            }
        }

        public List<HPbarElement> MonsterHpLabels
        {
            get
            {
                var monsterHpLabelsRoot = ReadObjectAt<HPbarElement>(0x12c);
                return monsterHpLabelsRoot.Children;
            }
        }

        public Element Buffs
        {
            get { return ReadObjectAt<Element>(0x138); }
        }

        public Element Buffs2
        {
            get { return ReadObjectAt<Element>(0x194); }
        }

        public Element OpenLeftPanel
        {
            get { return ReadObjectAt<Element>(0x16c + M.Server.IngameUIElementsOffset); }
        }

        public Element OpenRightPanel
        {
            get { return ReadObjectAt<Element>(0x170 + M.Server.IngameUIElementsOffset); }
        }

        public Element OpenNpcDialogPanel
        {
            get { return ReadObjectAt<Element>(0x168); }
        }

        public Element CreatureInfoPanel
        {
            get { return ReadObjectAt<Element>(0x18c); }
        } // above, it shows name and hp

        public Element InstanceManagerPanel
        {
            get { return ReadObjectAt<Element>(0x1a0); }
        }

        public Element InstanceManagerPanel2
        {
            get { return ReadObjectAt<Element>(0x1a4); }
        }

        // dunno what it is
        public Element SwitchingZoneInfo
        {
            get { return ReadObjectAt<Element>(0x1CC); }
        }

        public Element GemLvlUpPanel
        {
            get { return ReadObjectAt<Element>(0x21c + M.Server.IngameUIElementsOffset); }
        }

        public ItemOnGroundTooltip ItemOnGroundTooltip
        {
            get { return ReadObjectAt<ItemOnGroundTooltip>(0x22c + M.Server.IngameUIElementsOffset); }
        }
    }
}