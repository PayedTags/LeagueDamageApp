using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wip_LeagueThing
{
    public interface ShopItems
    {
        public string Name { get; set; }
        public int ImageIndex {  get; set; }
        public void Bought(ChampionKit champion);
        public void Sold(ChampionKit champion);
    }

    public class AdCritItem : ShopItems
    {
        public string Name { get; set; }
        public int ImageIndex {  get; set; }
        public double AttackDamage {  get; set; }
        public double CritChance { get; set; }
        public void Bought(ChampionKit champion)
        {
            champion.BuiltAttackDamage += AttackDamage;
            champion.CritChance += CritChance;
            champion.inventory.Add(this);
        }

        public void Sold(ChampionKit champion)
        {
            champion.BuiltAttackDamage -= AttackDamage;
            champion.CritChance -= CritChance;
            champion.inventory.Remove(this);
        }
    }

    public class AdCritFlatArmorPen : ShopItems
    {
        public string Name { get; set; }
        public int ImageIndex { get; set; }
        public double AttackDamage { get; set; }
        public double CritChance { get; set; }
        public double FlatArmorPen { get; set; }
        public void Bought(ChampionKit champion)
        {
            champion.BuiltAttackDamage += AttackDamage;
            champion.CritChance += CritChance;
            champion.FlatArmorPen += FlatArmorPen;
            champion.inventory.Add(this);
        }

        public void Sold(ChampionKit champion)
        {
            champion.BuiltAttackDamage -= AttackDamage;
            champion.CritChance -= CritChance;
            champion.FlatArmorPen -= FlatArmorPen;
            champion.inventory.Remove(this);
        }
    }

    public class AdCritPercentageArmorPen :ShopItems
    {
        public string Name { get; set; }
        public int ImageIndex { get; set; }
        public double AttackDamage { get; set; }
        public double CritChance { get; set; }
        public double PercentageArmorPen {  get; set; }
        public void Bought(ChampionKit champion)
        {
            champion.BuiltAttackDamage += AttackDamage;
            champion.CritChance += CritChance;
            champion.PercentageArmorPen += PercentageArmorPen;
            champion.inventory.Add(this);
        }

        public void Sold(ChampionKit champion)
        {
            champion.BuiltAttackDamage -= AttackDamage;
            champion.CritChance -= CritChance;
            champion.PercentageArmorPen -= PercentageArmorPen;
            champion.inventory.Remove(this);
        }
    }

    public class ApAhHealthManaRegen : ShopItems
    {
        public string Name { get; set; }
        public int ImageIndex { get; set; }

        public double AbilityPower { get; set; }
        public double AbilityHaste { get; set; }
        public double Health { get; set; }
        public double BaseManaRegeneration { get; set; }
        public void Bought(ChampionKit champion)
        {
            champion.BonusAbilityPower += AbilityPower;
            champion.AbilityHase += AbilityHaste;
            champion.BuiltHealth += Health;
            champion.CurrentHealth += Health;
            champion.BaseManaRegen += BaseManaRegeneration;
            champion.inventory.Add(this);
        }
        public void Sold(ChampionKit champion)
        {
            champion.BonusAbilityPower -= AbilityPower;
            champion.AbilityHase -= AbilityHaste;
            champion.CurrentHealth -= Health;
            champion.BuiltHealth -= Health;
            champion.BaseManaRegen -= BaseManaRegeneration;
            champion.inventory.Remove(this);
        }
    }

    public class ApAhManaRegenHS : ShopItems
    {
        public string Name { get; set; }
        public int ImageIndex { get; set; }

        public double AbilityPower { get; set; }
        public double AbilityHaste { get; set; }
        public double BaseManaRegeneration { get; set; }
        public double HealShieldPower { get; set; }
        public void Bought(ChampionKit champion)
        {
            champion.BonusAbilityPower += AbilityPower;
            champion.AbilityHase += AbilityHaste;
            champion.BonusHealShieldPower += HealShieldPower;
            champion.BaseManaRegen += BaseManaRegeneration;
            champion.inventory.Add(this);
        }
        public void Sold(ChampionKit champion)
        {
            champion.BonusAbilityPower -= AbilityPower;
            champion.AbilityHase -= AbilityHaste;
            champion.BonusHealShieldPower -= HealShieldPower;
            champion.BaseManaRegen -= BaseManaRegeneration;
            champion.inventory.Remove(this);
        }
    }
    public class ApManaRegenHS : ShopItems
    {
        public string Name { get; set; }
        public int ImageIndex { get; set; }

        public double AbilityPower { get; set; }
        public double BaseManaRegeneration { get; set; }
        public double HealShieldPower { get; set; }
        public void Bought(ChampionKit champion)
        {
            champion.BonusAbilityPower += AbilityPower;
            champion.BonusHealShieldPower += HealShieldPower;
            champion.BaseManaRegen += BaseManaRegeneration;
            if (Name == "Dawncore")
                champion.Dawncore = true;
            champion.inventory.Add(this);
        }
        public void Sold(ChampionKit champion)
        {
            champion.BonusAbilityPower -= AbilityPower;
            champion.BonusHealShieldPower -= HealShieldPower;
            champion.BaseManaRegen -= BaseManaRegeneration;
            if (Name == "Dawncore")
                champion.Dawncore = false;
            champion.inventory.Remove(this);
        }
    }

    public class InfinityEdge : ShopItems
    {
        public string Name { get; set; }
        public int ImageIndex {  get; set; }
        public double AttackDamage { get; set; }
        public double CritChance { get; set; }
        public double CritDamage { get; set; }
        public void Bought(ChampionKit champion)
        {
            for (int i = 0; i < champion.Abilities.Count; i++)
            {
                if (champion.Abilities[i].CritDamage != 0)
                    champion.Abilities[i].CritDamage += (CritDamage / 100);
            }

            champion.BuiltAttackDamage += AttackDamage;
            champion.CritDamage += (CritDamage / 100);
            champion.CritChance += CritChance;
            champion.inventory.Add(this);
        }
        public void Sold(ChampionKit champion)
        {
            for (int i = 0; i < champion.Abilities.Count; i++)
            {
                if (champion.Abilities[i].CritDamage != 0)
                    champion.Abilities[i].CritDamage -= (CritDamage / 100);
            }

            champion.BuiltAttackDamage -= AttackDamage;
            champion.CritDamage -= (CritDamage / 100);
            champion.CritChance -= CritChance;
            champion.inventory.Remove(this);
        }

    }

    public class Rabadons :ShopItems
    {
        public string Name { get; set; }
        public int ImageIndex {  get; set; }
        public double AbilityPower { get; set; }
        public double ApModifier { get; set; }
        public void Bought(ChampionKit champion)
        {
            champion.BonusAbilityPower += AbilityPower;
            champion.AbilityPowerModifier += ApModifier;
            champion.inventory.Add(this);
        }
        public void Sold(ChampionKit champion)
        {
            champion.BonusAbilityPower -= AbilityPower;
            champion.AbilityPowerModifier -= ApModifier;
            champion.inventory.Remove(this);
        }
    }

}
