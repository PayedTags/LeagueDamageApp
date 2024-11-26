using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Security.Policy;
using wip_LeagueThing;

namespace wip_LeagueThing
{
    public class ChampionKit
    {
        public ChampionKit()
        { }

        public ChampionKit(string name, double basehp, double hp_lvl, double mp, double mp_lvl, double arm, double arm_lvl, double a, double b, double c, double d, double e, double f, double g, double h)
        {
            Name = name;
            BaseHealth = basehp;
            HealthGrowth = hp_lvl;
            BaseMana = mp;
            ManaGrowth = mp_lvl;
            BaseArmor = arm;
            ArmorGrowth = arm_lvl;
            BaseMagicResist = a;
            MagicResistGrowth = b;
            CritDamage = c;
            BaseAttackSpeed = d;
            AttackSpeedGrowth = e;
            AttackSpeedRatio = f;
            BaseAttackDamage = g;
            AttackDamageGrowth = h;
            Level = 1;
        }



        #region Base Stats
        public double BaseHealth { get; set; }
        public double HealthGrowth { get; set; }
        public double BaseMana { get; set; }
        public double ManaGrowth { get; set; }
        public double BaseArmor { get; set; }
        public double ArmorGrowth { get; set; }
        public double BaseMagicResist { get; set; }
        public double MagicResistGrowth { get; set; }
        public double CritDamage { get; set; }
        public double BaseAttackSpeed { get; set; }
        public double AttackSpeedGrowth { get; set; }
        public double AttackSpeedRatio { get; set; }
        public double BaseAttackDamage { get; set; }
        public double AttackDamageGrowth { get; set; }
        public double CurrentHealth { get; set; }

        #endregion

        public List<ShopItems> inventory = new List<ShopItems>();

        #region Built Stats
        public bool Dawncore = false;
        public double TotalAttackDamage = 0.0, TotalAbilityPower = 0.0;
        public double TotalAttackSpeed = 0.0;
        public double MaxHealth = 0.0;
        public double TotalArmor = 0.0, TotalMagicResist = 0.0;
        public double CritChance = 0.0;
        public double BuiltAttackDamage = 0.0, BuiltHealth = 0.0, BuiltArmor = 0.0, BuiltMagicResist = 0.0;
        private double builtAttackSpeed;
        public double BuiltAttackSpeed
        {
            get { return builtAttackSpeed; }
            set 
            {
                double totalAttackSpeed = 0.0;
                builtAttackSpeed = value;
                BonusAttackSpeed = AttackSpeedGrowth * (Level - 1);
                BonusAttackSpeed *= 100;
                BonusAttackSpeed += builtAttackSpeed;
                totalAttackSpeed =  AttackSpeedRatio * (BonusAttackSpeed / 100);
                totalAttackSpeed += BaseAttackSpeed;
                TotalAttackSpeed = totalAttackSpeed;
            }
        }

        public double BaseManaRegen = 0.0;
        public double BonusAttackDamage = 0.0, BonusAttackSpeed = 0.0, BonusAbilityPower = 0.0;
        public double AttackDamageModifier = 0.0, AbilityPowerModifier = 0.0, HealthModifier = 0.0, ArmorModifier = 0.0, MagicResistModifier = 0.0;
        public double BonusHealth = 0.0, BonusMana = 0.0;
        public double BonusArmor = 0.0, BonusMagicResist = 0.0;
        public double FlatArmorPen = 0.0, PercentageArmorPen = 0.0;
        public double FlatMagicPen = 0.0, PercentageMagicPen = 0.0;
        public double BonusHealShieldPower = 0.0, TotalHealShieldPower = 0.0;
        public double AbilityHase = 0.0;
        public double BonusDamage = 0.0, AbilityBonusDamage = 0.0;
        public double OnHit = 0.0;
        public double CritDamageReduction = 0.0;
        #endregion

        #region temp
        public string Name { get; set; }
        public double maxMana { get; set; }
        public int Level = 1;
        public int skillPoints { get; set; }
        public int Icon { get; set; }
        #endregion

        public List<IAbility> Abilities { get; set; } = new List<IAbility>();

        public ChampionKit LoadChampionFromJson(string championName)
        {
            string jsonData = File.ReadAllText("F:\\Projects\\wip_LeagueThing\\wip_LeagueThing\\ChampionData.Json");

            JsonDocument doc = JsonDocument.Parse(jsonData);

            foreach (var championElement in doc.RootElement.EnumerateArray())
            {
                if (championElement.GetProperty("name").GetString().Equals(championName, StringComparison.OrdinalIgnoreCase))
                {
                    ChampionKit test = new ChampionKit()
                    {

                        Name = championElement.GetProperty("name").GetString(),
                        BaseHealth = championElement.GetProperty("baseHealth").GetDouble(),
                        HealthGrowth = championElement.GetProperty("healthGrowth").GetDouble(),
                        BaseMana = championElement.GetProperty("baseMana").GetDouble(),
                        ManaGrowth = championElement.GetProperty("manaGrowth").GetDouble(),
                        BaseArmor = championElement.GetProperty("baseArmor").GetDouble(),
                        ArmorGrowth = championElement.GetProperty("armorGrowth").GetDouble(),
                        BaseMagicResist = championElement.GetProperty("baseMagicResist").GetDouble(),
                        MagicResistGrowth = championElement.GetProperty("magicResistGrowth").GetDouble(),
                        CritDamage = championElement.GetProperty("baseCritDamage").GetDouble(),
                        BaseAttackSpeed = championElement.GetProperty("baseAttackSpeed").GetDouble(),
                        AttackSpeedGrowth = championElement.GetProperty("attackSpeedGrowth").GetDouble(),
                        AttackSpeedRatio = championElement.GetProperty("attackSpeedRatio").GetDouble(),
                        BaseAttackDamage = championElement.GetProperty("baseAttackDamage").GetDouble(),
                        AttackDamageGrowth = championElement.GetProperty("attackDamageGrowth").GetDouble(),
                        Icon = championElement.GetProperty("imageIndex").GetInt32()
                    };

                    foreach (var abilityElement in championElement.GetProperty("abilities").EnumerateArray())
                    {
                        IAbility ability = CreateAbilityFromJson(abilityElement);
                        if (ability != null)
                        {
                            test.Abilities.Add(ability);
                        }
                    }
                    return test;
                }
            }
            Console.WriteLine("Error champ not found");
            return null;
        }


        //Make sure to update this after adding an ability type
        public static IAbility CreateAbilityFromJson(JsonElement abilityData)
        {
            switch (abilityData.GetProperty("type").GetString())
            {
                case "Passive":
                    return new Passive
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32()
                    };
                #region TotalAd Scaling
                case "TotalAdChampLv":
                    return new TotalAdChampLv
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        AttackDamageScaling = abilityData.GetProperty("attackDamageScaling").GetDouble(),
                        LevelThreshold = abilityData.GetProperty("levelThreshold").GetInt32(),
                        AttackDamageScalingGrowth = abilityData.GetProperty("attackDamageScalingGrowth").GetDouble()
                    };
                case "BaseDamageLvTotalAd":
                    return new BaseDamageLvTotalAd
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        BaseDamage = abilityData.GetProperty("baseDamage").GetDouble(),
                        BaseDamageGrowth = abilityData.GetProperty("baseDamageGrowth").GetDouble(),
                        AttackDamageScaling = abilityData.GetProperty("attackDamageScaling").GetDouble()
                    };
                case "BaseDamageLvTotalAdLv":
                    return new BaseDamageLvTotalAdLv
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        BaseDamage = abilityData.GetProperty("baseDamage").GetDouble(),
                        BaseDamageGrowth = abilityData.GetProperty("baseDamageGrowth").GetDouble(),
                        AttackDamageScaling = abilityData.GetProperty("attackDamageScaling").GetDouble(),
                        AttackDamageScalingGrowth = abilityData.GetProperty("attackDamageScalingGrowth").GetDouble()
                    };
                #endregion

                #region BonusAd Scaling
                case "BaseDamageLvBonusAd":
                    return new BaseDamageLvBonusAd
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        BaseDamage = abilityData.GetProperty("baseDamage").GetDouble(),
                        BaseDamageGrowth = abilityData.GetProperty("baseDamageGrowth").GetDouble(),
                        AttackDamageScaling = abilityData.GetProperty("attackDamageScaling").GetDouble(),
                    };
                case "BaseDamageLvBonusAdBuffAsLv":
                    return new BaseDamageLvBonusAdBuffAsLv
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        BaseDamage = abilityData.GetProperty("baseDamage").GetDouble(),
                        BaseDamageGrowth = abilityData.GetProperty("baseDamageGrowth").GetDouble(),
                        AttackDamageScaling = abilityData.GetProperty("attackDamageScaling").GetDouble(),
                        AttackSpeedBuff = abilityData.GetProperty("attackSpeedBuff").GetDouble(),
                        AttackSpeedBuffGrowth = abilityData.GetProperty("attackSpeedBuffGrowth").GetDouble()
                    };
                case "BaseDamageLvBonusAdCR":
                    return new BaseDamageLvBonusAdCR
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        BaseDamage = abilityData.GetProperty("baseDamage").GetDouble(),
                        BaseDamageGrowth = abilityData.GetProperty("baseDamageGrowth").GetDouble(),
                        AttackDamageScaling = abilityData.GetProperty("attackDamageScaling").GetDouble(),
                        CritRateScaling = abilityData.GetProperty("critScaling").GetDouble()
                    };
                #endregion

                case "BaseDamageLvAp":
                    return new BaseDamageLvAp
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        BaseDamage = abilityData.GetProperty("baseDamage").GetDouble(),
                        BaseDamageGrowth = abilityData.GetProperty("baseDamageGrowth").GetDouble(),
                        AbilityPowerScaling = abilityData.GetProperty("abilityPowerScaling").GetDouble()
                    };
                case "HealAp":
                    return new HealAp
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        BaseHeal = abilityData.GetProperty("baseHeal").GetInt32(),
                        BaseHealGrowth = abilityData.GetProperty("baseHealGrowth").GetInt32(),
                        AbilityPowerScaling = abilityData.GetProperty("abilityPowerScaling").GetDouble()
                    };


                #region Ability Specific
                case "DaredevilImpulse":
                    return new DaredevilImpulse
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        BaseDamage = abilityData.GetProperty("baseDamage").GetDouble(),
                        BaseDamageGrowth = abilityData.GetProperty("baseDamageGrowth").GetDouble(),
                        AttackDamageScaling = abilityData.GetProperty("attackDamageScaling").GetDouble(),
                        AttackDamageScalingGrowth = abilityData.GetProperty("attackDamageScalingGrowth").GetDouble()
                    };
                case "DeadlyPlumage":
                    return new DeadlyPlumage
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        Buff1 = abilityData.GetProperty("damageBuff").GetDouble(),
                        Buff2 = abilityData.GetProperty("attackSpeedBuff").GetDouble(),
                        Buff2Growth = abilityData.GetProperty("attackSpeedBuffGrowth").GetDouble()
                    };
                case "Starcall":
                    return new Starcall
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        BaseDamage = abilityData.GetProperty("baseDamage").GetDouble(),
                        BaseDamageGrowth = abilityData.GetProperty("baseDamageGrowth").GetDouble(),
                        AbilityPowerScaling = abilityData.GetProperty("abilityPowerScaling").GetDouble(),
                        BaseHeal = abilityData.GetProperty("baseHeal").GetDouble(),
                        BaseHealGrowth = abilityData.GetProperty("baseHealGrowth").GetDouble(),
                        HealAbilityPowerScaling = abilityData.GetProperty("healAbilityPowerScaling").GetDouble(),

                    };
                #endregion
                #region Varus
                case "LivingVengeance":
                    return new LivingVengeance
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        AttackSpeedBuff = abilityData.GetProperty("attackSpeedBuff").GetDouble(),
                        AttackSpeedScaling = abilityData.GetProperty("attackSpeedScaling").GetDouble()
                    };
                case "PiercingArrow":
                    return new PiercingArrow
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        BaseDamage = abilityData.GetProperty("baseDamage").GetDouble(),
                        BaseDamageGrowth = abilityData.GetProperty("baseDamageGrowth").GetDouble(),
                        AttackDamageScaling = abilityData.GetProperty("attackDamageScaling").GetDouble(),
                        AttackDamageScalingGrowth = abilityData.GetProperty("attackDamageScalingGrowth").GetDouble()
                    };
                case "BlightedQuiver":
                    return new BlightedQuiver
                    {
                        Name = abilityData.GetProperty("name").GetString(),
                        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
                        CritDamage = abilityData.GetProperty("critDamage").GetDouble(),
                        DamageType = abilityData.GetProperty("damageType").ToString(),
                        HitCount = abilityData.GetProperty("hitCount").GetInt32(),
                        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
                        Level = abilityData.GetProperty("level").GetInt32(),
                        BaseOnHitDamage = abilityData.GetProperty("baseOnHitDamage").GetDouble(),
                        BaseOnHitDamageGrowth = abilityData.GetProperty("baseOnHitDamageGrowth").GetDouble(),
                        OnHitDamageScaling = abilityData.GetProperty("onHitDamageScaling").GetDouble(),
                        MaxHealthBaseDamage = abilityData.GetProperty("maxHealthBaseDamage").GetDouble(),
                        MaxHealthBaseDamageGrowth = abilityData.GetProperty("maxHealthBaseDamageGrowth").GetDouble(),
                        MaxHealthDamageScaling = abilityData.GetProperty("maxHealthDamageScaling").GetDouble(),
                        MissingHealthBaseDamage = abilityData.GetProperty("missingHealthBaseDamage").GetDouble(),
                        MissingHealthBaseDamageGrowth = abilityData.GetProperty("missingHealthBaseDamageGrowth").GetDouble(),
                    };
                    #endregion
                    // Add more cases as needed
            }
            return null;
        }
    }
}

 /*case "Temp":
    return new Temp
    {
        Name = abilityData.GetProperty("name").GetString(),
        BaseDamage = abilityData.GetProperty("baseDamage").GetSingle(),
        DamageType = abilityData.GetProperty("damageType").ToString(),
        CanCrit = abilityData.GetProperty("canCrit").GetBoolean(),
        ImageIndex = abilityData.GetProperty("imageIndex").GetInt32(),
    };*/
