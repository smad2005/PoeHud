using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows.Forms;

namespace PoeHUD.Poe.FilesInMemory
{
    public class ItemClassesDisplay
    {
        private List<string> contents;

        public string this[int index]
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return contents?[index] ?? LoadItemTypes()[index];
            }
        }
        private List<string> LoadItemTypes()
        {
            contents = new List<string>
            {
                "Life Flasks",
                "Mana Flasks",
                "Hybrid Flasks",
                "Currency",
                "Amulets",
                "Rings",
                "Claws",
                "Daggers",
                "Wands",
                "One Hand Swords",
                "Thrusting One Hand Swords",
                "One Hand Axes",
                "One Hand Maces",
                "Bows",
                "Staves",
                "Two Hand Swords",
                "Two Hand Axes",
                "Two Hand Maces",
                "Active Skill Gems",
                "Support Skill Gems",
                "Quivers",
                "Belts",
                "Gloves",
                "Boots",
                "Body Armours",
                "Helmets",
                "Shields",
                "Small Relics",
                "Medium Relics",
                "Large Relics",
                "Stackable Currency",
                "Quest Items",
                "Sceptres",
                "Utility Flasks",
                "Critical Utility Flasks",
                "Maps",
                "",
                "Fishing Rods",
                "Map Fragments",
                "Hideout Doodads",
                "Microtransactions",
                "Jewel",
                "Divination Card"
            };
            return contents;
        }
    }
}