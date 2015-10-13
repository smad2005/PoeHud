using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PoeHUD.Models.Enums;
using PoeHUD.Poe.RemoteMemoryObjects;

namespace PoeHUD.Models
{
    public class StatTranslator
    {
        private readonly Dictionary<string, AddStat> mods;

        public StatTranslator()
        {
            mods = new Dictionary<string, AddStat>();
            mods.Add("Dexterity", Single(ItemStatEnum.Dexterity));
            mods.Add("Strength", Single(ItemStatEnum.Strength));
            mods.Add("Intelligence", Single(ItemStatEnum.Intelligence));
            mods.Add("IncreasedMana", Single(ItemStatEnum.AddedMana));
            mods.Add("IncreasedLife", Single(ItemStatEnum.AddedHP));
            mods.Add("IncreasedEnergyShield", Single(ItemStatEnum.AddedES));
            mods.Add("IncreasedEnergyShieldPercent", Single(ItemStatEnum.AddedESPercent));
            mods.Add("ColdResist", Single(ItemStatEnum.ColdResistance));
            mods.Add("FireResist", Single(ItemStatEnum.FireResistance));
            mods.Add("LightningResist", Single(ItemStatEnum.LightningResistance));
            mods.Add("ChaosResist", Single(ItemStatEnum.ChaosResistance));
            mods.Add("AllResistances", MultipleSame(new[]
            {
                ItemStatEnum.ColdResistance,
                ItemStatEnum.FireResistance,
                ItemStatEnum.LightningResistance
            }));
            mods.Add("CriticalStrikeChance", Single(ItemStatEnum.CritChance));
            mods.Add("LocalCriticalMultiplier", Single(ItemStatEnum.CritMultiplier));
            mods.Add("MovementVelocity", Single(ItemStatEnum.MovementSpeed));
            mods.Add("ItemFoundRarityIncrease", Single(ItemStatEnum.Rarity));
            mods.Add("ItemFoundQuantityIncrease", Single(ItemStatEnum.Quantity));
            mods.Add("ManaLeech", Single(ItemStatEnum.ManaLeech));
            mods.Add("LifeLeech", Single(ItemStatEnum.LifeLeech));
            mods.Add("AddedLightningDamage", Average(ItemStatEnum.AddedLightningDamage));
            mods.Add("AddedColdDamage", Average(ItemStatEnum.AddedColdDamage));
            mods.Add("AddedFireDamage", Average(ItemStatEnum.AddedFireDamage));
            mods.Add("AddedPhysicalDamage", Average(ItemStatEnum.AddedPhysicalDamage));
            mods.Add("WeaponElementalDamage", Single(ItemStatEnum.WeaponElementalDamagePercent));
            mods.Add("FireDamagePercent", Single(ItemStatEnum.FireDamagePercent));
            mods.Add("ColdDamagePercent", Single(ItemStatEnum.ColdDamagePercent));
            mods.Add("LightningDamagePercent", Single(ItemStatEnum.LightningDamagePercent));
            mods.Add("SpellDamage", Single(ItemStatEnum.SpellDamage));
            mods.Add("SpellDamageAndMana", Dual(ItemStatEnum.SpellDamage, ItemStatEnum.AddedMana));
            mods.Add("SpellCriticalStrikeChance", Single(ItemStatEnum.SpellCriticalChance));
            mods.Add("IncreasedCastSpeed", Single(ItemStatEnum.CastSpeed));
            mods.Add("ProjectileSpeed", Single(ItemStatEnum.ProjectileSpeed));
            mods.Add("LocalIncreaseSocketedMinionGemLevel", Single(ItemStatEnum.MinionSkillLevel));
            mods.Add("LocalIncreaseSocketedFireGemLevel", Single(ItemStatEnum.FireSkillLevel));
            mods.Add("LocalIncreaseSocketedColdGemLevel", Single(ItemStatEnum.ColdSkillLevel));
            mods.Add("LocalIncreaseSocketedLightningGemLevel", Single(ItemStatEnum.LightningSkillLevel));
            mods.Add("LocalAddedPhysicalDamage", Average(ItemStatEnum.LocalPhysicalDamage));
            mods.Add("LocalIncreasedPhysicalDamagePercent", Single(ItemStatEnum.LocalPhysicalDamagePercent));
            mods.Add("LocalAddedColdDamage", Average(ItemStatEnum.LocalAddedColdDamage));
            mods.Add("LocalAddedFireDamage", Average(ItemStatEnum.LocalAddedFireDamage));
            mods.Add("LocalAddedLightningDamage", Average(ItemStatEnum.LocalAddedLightningDamage));
            mods.Add("LocalCriticalStrikeChance", Single(ItemStatEnum.LocalCritChance));
            mods.Add("LocalIncreasedAttackSpeed", Single(ItemStatEnum.LocalAttackSpeed));
            mods.Add("LocalIncreasedEnergyShield", Single(ItemStatEnum.LocalES));
            mods.Add("LocalIncreasedEvasionRating", Single(ItemStatEnum.LocalEV));
            mods.Add("LocalIncreasedPhysicalDamageReductionRating", Single(ItemStatEnum.LocalArmor));
            mods.Add("LocalIncreasedEvasionRatingPercent", Single(ItemStatEnum.LocalEVPercent));
            mods.Add("LocalIncreasedEnergyShieldPercent", Single(ItemStatEnum.LocalESPercent));
            mods.Add("LocalIncreasedPhysicalDamageReductionRatingPercent", Single(ItemStatEnum.LocalArmorPercent));
            mods.Add("LocalIncreasedArmourAndEvasion", MultipleSame(new[]
            {
                ItemStatEnum.LocalArmorPercent,
                ItemStatEnum.LocalEVPercent
            }));
            mods.Add("LocalIncreasedArmourAndEnergyShield", MultipleSame(new[]
            {
                ItemStatEnum.LocalArmorPercent,
                ItemStatEnum.LocalESPercent
            }));
            mods.Add("LocalIncreasedEvasionAndEnergyShield", MultipleSame(new[]
            {
                ItemStatEnum.LocalEVPercent,
                ItemStatEnum.LocalESPercent
            }));
        }

        public void Translate(ItemStats stats, ItemMod m)
        {
            Contract.Requires(m != null);
            Contract.Requires(m.Name != null);
            if (!mods.ContainsKey(m.Name))
            {
                return;
            }
            mods[m.Name](stats, m);
        }

        private AddStat Single(ItemStatEnum stat)
        {
            return delegate(ItemStats x, ItemMod m) { x.AddToMod(stat, m.Value1); };
        }

        private AddStat Average(ItemStatEnum stat)
        {
            return delegate(ItemStats x, ItemMod m) { x.AddToMod(stat, (m.Value1 + m.Value2) / 2f); };
        }

        private AddStat Dual(ItemStatEnum s1, ItemStatEnum s2)
        {
            return delegate(ItemStats x, ItemMod m)
            {
                x.AddToMod(s1, m.Value1);
                x.AddToMod(s2, m.Value2);
            };
        }

        private AddStat MultipleSame(params ItemStatEnum[] stats)
        {
            return delegate(ItemStats x, ItemMod m)
            {
                foreach (ItemStatEnum stat in stats)
                {
                    x.AddToMod(stat, m.Value1);
                }
            };
        }

        private delegate void AddStat(ItemStats stats, ItemMod m);
    }
}