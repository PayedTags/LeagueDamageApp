using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace wip_LeagueThing
{
    public class DamageCalculationContext
    {
        public double BaseAttackDamage { get; set; }
        public double BonusAttackDamage { get; set; }
        public double TotalAttackDamage { get; set; }
        public double BonusAbilityPower { get; set; }
        public double BonusAttackSpeed { get; set; }
        public double BonusHealth { get; set; }
        public double MaxHealth { get; set; }
        public double Level { get; set; }
        public double CritDamage { get; set; }
        public double CritChance { get; set; }
        public double BonusDamage { get; set; }
        public double AbilityBonusDamage { get; set; }
        public double Lethality { get; set; }
        public double ArmorPen { get; set; }
        public double FlatMagicPen { get; set; }
        public double PercentageMagicPen { get; set; }
        public double HealShieldPower { get; set; }

        public List<IAbility> Abilities { get; set; }

        public double EnemyMaxHealth { get; set; }
        public double EnemyCurrentHealth { get; set; }
        public double EnemyArmor { get; set; }
        public double EnemyMagicResist { get; set; }

    }


    public interface IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public double CalculateDamage(DamageCalculationContext context);
        public double CalculateCritDamage(DamageCalculationContext context);
        public double CalculateSecondaryDamage(DamageCalculationContext context);
        public double CalculateTertiaryDamage(DamageCalculationContext context);
        public double ToggleAbility(ChampionKit champion);
    }

    public class Passive : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion

        public double CalculateDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }

    }

   


    #region BonusAd Scaling
    public class BaseDamageLvBonusAd : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public double BaseDamage { get; set; }
        public double BaseDamageGrowth { get; set; }
        public double AttackDamageScaling { get; set; }

        public double CalculateDamage(DamageCalculationContext context)
        {
            int level = Level;
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (level - 1);
            attackDamageScaling /= 100;

            output += baseDamage + (attackDamageScaling * context.BonusAttackDamage);
            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output += baseDamage + (attackDamageScaling * context.BonusAttackDamage);

            if (CritDamage == 0)
                output = output * context.CritDamage;
            else
                output = output * CritDamage;
            return output;
        }
        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }
    }

    public class BaseDamageLvBonusAdLv : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public double BaseDamage { get; set; }
        public double BaseDamageGrowth { get; set; }
        public double AttackDamageScaling { get; set; }
        public double AttackDamageScalingGrowth { get; set; }

        public double CalculateDamage(DamageCalculationContext context)
        {
            int level = Level;
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (level - 1);
            attackDamageScaling += AttackDamageScalingGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output += baseDamage + (attackDamageScaling * context.BonusAttackDamage);
            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output += baseDamage + (attackDamageScaling * context.BonusAttackDamage);

            if (CritDamage == 0)
                output = output * context.CritDamage;
            else
                output = output * CritDamage;
            return output;
        }
        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }
    }

    public class BaseDamageLvBonusAdCR : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public double BaseDamage { get; set; }
        public double BaseDamageGrowth { get; set; }
        public double AttackDamageScaling { get; set; }
        public double CritRateScaling { get; set; }

        public double CalculateDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling, critRateScaling = CritRateScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling /= 100;
            critRateScaling /= 100;

            output = baseDamage + (attackDamageScaling * context.BonusAttackDamage);
            output = output * (1 + ((context.CritChance * critRateScaling) / 100));
            return output;

        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling, critRateScaling = CritRateScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling /= 100;
            critRateScaling /= 100;

            output = baseDamage + (attackDamageScaling * context.BonusAttackDamage);
            output = output * (1 + (context.CritChance * critRateScaling));

            if (CritDamage == 0)
                output = output * context.CritDamage;
            else
                output = output * CritDamage;
            return output;
        }

        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }
    }

    public class BaseDamageLvBonusAdBuffAsLv : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public double BaseDamage { get; set; }
        public double BaseDamageGrowth { get; set; }
        public double AttackDamageScaling { get; set; }
        public double AttackSpeedBuff { get; set; }
        public double AttackSpeedBuffGrowth { get; set; }
        public bool Active { get; set; }
        public double CalculateDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output += baseDamage + (attackDamageScaling * context.BonusAttackDamage);
            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output += baseDamage + (attackDamageScaling * context.BonusAttackDamage);

            if (CritDamage == 0)
                output = output * context.CritDamage;
            else
                output = output * CritDamage;
            return output;
        }
        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double ToggleAbility(ChampionKit champion)
        {
            double attackSpeedBuff = AttackSpeedBuff;
            attackSpeedBuff += AttackSpeedBuffGrowth * (Level - 1);
            if (Active)
            {
                champion.BuiltAttackSpeed -= attackSpeedBuff;
                Active = false;
                return attackSpeedBuff;
            }
            else
            {
                champion.BuiltAttackSpeed += attackSpeedBuff;
                Active = true;
                return attackSpeedBuff;
            }
        }
    }
    #endregion

    #region BaseAd Scaling
    public class BaseDamageLvBaseAd : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public double BaseDamage { get; set; }
        public double BaseDamageGrowth { get; set; }
        public double AttackDamageScaling { get; set; }

        public double CalculateDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output = baseDamage + (attackDamageScaling * context.BaseAttackDamage);
            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output = baseDamage + (attackDamageScaling * context.BaseAttackDamage);

            if (CritDamage == 0)
                output = output * context.CritDamage;
            else
                output = output * CritDamage;
            return output;
        }
        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }
    }
    #endregion

    #region TotalAd Scaling
    public class BaseDamageLvTotalAd : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public double BaseDamage { get; set; }
        public double BaseDamageGrowth { get; set; }
        public double AttackDamageScaling { get; set; }

        public double CalculateDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output = baseDamage + (context.TotalAttackDamage * attackDamageScaling);
            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output = baseDamage + (context.TotalAttackDamage * attackDamageScaling);

            if (CritDamage == 0)
                output = output * context.CritDamage;
            else
                output = output * CritDamage;
            return output;
        }
        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }
    }

    public class BaseDamageLvTotalAdLv : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public double BaseDamage { get; set; }
        public double BaseDamageGrowth { get; set; }
        public double AttackDamageScaling { get; set; }
        public double AttackDamageScalingGrowth { get; set; }

        public double CalculateDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling += AttackDamageScalingGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output = baseDamage + (context.TotalAttackDamage * attackDamageScaling);
            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling += AttackDamageScalingGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output = baseDamage + (context.TotalAttackDamage * attackDamageScaling);

            if (CritDamage == 0)
                output = output * context.CritDamage;
            else
                output = output * CritDamage;
            return output;
        }
        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }
    }

    public class TotalAdChampLv : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public int LevelThreshold { get; set; }
        public double AttackDamageScaling { get; set; }
        public double AttackDamageScalingGrowth { get; set; }

        public double CalculateDamage(DamageCalculationContext context)
        {
            double output = 0.0;
            double attackDamageScaling = AttackDamageScaling, attackDamageScalingGrowth = AttackDamageScalingGrowth;
            int scalingMultiplier = 0;

            attackDamageScaling /= 100;
            attackDamageScalingGrowth = AttackDamageScalingGrowth / 100;

            for (; context.Level > LevelThreshold; scalingMultiplier++)
            {
                context.Level -= LevelThreshold;
            }

            output = context.TotalAttackDamage * (attackDamageScaling + (attackDamageScalingGrowth * scalingMultiplier));
            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0;
            double attackDamageScaling = AttackDamageScaling, attackDamageScalingGrowth = AttackDamageScalingGrowth;
            int scalingMultiplier = 0;

            attackDamageScaling /= 100;
            attackDamageScalingGrowth = AttackDamageScalingGrowth / 100;

            for (double i = context.Level; i > LevelThreshold; scalingMultiplier++)
            {
                i -= LevelThreshold;
            }

            output = context.TotalAttackDamage * (attackDamageScaling + (attackDamageScalingGrowth * scalingMultiplier));

            if (CritDamage == 0)
                output = output * context.CritDamage;
            else
                output = output * CritDamage;
            return output;
        }
        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }
    }
    #endregion

    #region AbilityPower
    public class BaseDamageLvAp : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public double BaseDamage { get; set; }
        public double BaseDamageGrowth { get; set; }
        public double AbilityPowerScaling { get; set; }

        public double CalculateDamage(DamageCalculationContext context)
        {
            int level = Level;
            double output = 0.0, baseDamage = BaseDamage, abilityPowerScaling = AbilityPowerScaling;

            baseDamage += BaseDamageGrowth * (level - 1);
            abilityPowerScaling /= 100;

            output += baseDamage + (abilityPowerScaling * context.BonusAbilityPower);

            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, abilityPowerScaling = AbilityPowerScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            abilityPowerScaling /= 100;

            output += baseDamage + (abilityPowerScaling * context.BonusAbilityPower);

            if (CritDamage == 0)
                output = output * context.CritDamage;
            else
                output = output * CritDamage;
            return output;
        }
        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }

    }

    public class HealAp : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public double BaseHeal { get; set; }
        public double BaseHealGrowth { get; set; }
        public double AbilityPowerScaling { get; set; }


        public double CalculateDamage(DamageCalculationContext context)
        {
            int level = Level;
            double output = 0.0, baseHeal = BaseHeal, abilityPowerScaling = AbilityPowerScaling;

            baseHeal += BaseHealGrowth * (level - 1);
            abilityPowerScaling /= 100;

            output = baseHeal + (abilityPowerScaling * context.BonusAbilityPower);
            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }

    }

    #endregion


    #region Ability Specifics
    public class DeadlyPlumage : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public bool Active = false;
        public double Buff1 { get; set; }
        public double Buff2 { get; set; }
        public double Buff2Growth { get; set; }

        public double ToggleAbility(ChampionKit champion)
        {
            double attackSpeedBuff = Buff2;
            attackSpeedBuff += Buff2Growth * (Level - 1);
            if (Active)
            {
                champion.BuiltAttackSpeed -= attackSpeedBuff;
                Active = false;
                return Convert.ToInt32(Active);
            }
            else
            {
                champion.BuiltAttackSpeed += attackSpeedBuff;
                Active = true;
                return Convert.ToInt32(Active);
            }

        }

        public double CalculateDamage(DamageCalculationContext context)
        {
            double output = 0.0;
            output = context.TotalAttackDamage * Buff1;

            output = output * (1 + context.BonusDamage);
            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0;
            output = context.TotalAttackDamage * Buff1;

            output = output * context.CritDamage;
            output = output * (1 + context.BonusDamage);
            return output;
        }
        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
    }

    public class DaredevilImpulse : IAbility
    {
        
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public double BaseDamage { get; set; }
        public double BaseDamageGrowth { get; set; }
        public double AttackDamageScaling { get; set; }
        public double AttackDamageScalingGrowth { get; set; }

        public double CalculateDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (context.Level - 1);
            attackDamageScaling = AttackDamageScalingGrowth * (context.Level - 1);
            attackDamageScaling /= 100;

            output = baseDamage + (attackDamageScaling * context.TotalAttackDamage);
            output = output * (1 + context.BonusDamage);
            return output;
        }

        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = 0.0, attackDamageScaling = 0.0;

            baseDamage = 1 + (Level - 1);
            attackDamageScaling = AttackDamageScaling + (0.39 * (context.Level - 1));
            attackDamageScaling /= 100;

            output = baseDamage + (attackDamageScaling * context.TotalAttackDamage);
            output = output * context.CritDamage;
            output = output * (1 + context.BonusDamage);

            return output;
        }
        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }
    }
 
    public class Starcall : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public bool Active { get; set; }
        public double BaseDamage { get; set; }
        public double BaseDamageGrowth { get; set; }
        public double AbilityPowerScaling { get; set; }
        public double BaseHeal { get; set; }
        public double BaseHealGrowth { get; set; }
        public double HealAbilityPowerScaling { get; set; }


        public double CalculateDamage(DamageCalculationContext context)
        {
            int level = Level;
            double output = 0.0, baseDamage = BaseDamage, abilityPowerScaling = AbilityPowerScaling;

            baseDamage += BaseDamageGrowth * (level - 1);
            abilityPowerScaling /= 100;

            output += baseDamage + (abilityPowerScaling * context.BonusAbilityPower);

            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            int level = Level;
            double output = 0.0, baseHeal = BaseHeal, abilityPowerScaling = HealAbilityPowerScaling;

            baseHeal += BaseHealGrowth * (level - 1);
            abilityPowerScaling /= 100;

            output = baseHeal + (abilityPowerScaling * context.BonusAbilityPower);
            return output;
        }

        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double ToggleAbility(ChampionKit champion)
        {
            if(Active)
            {
                Active = false;
                return Convert.ToInt32(Active);
            }
            else
            {
                Active = true;
                return Convert.ToInt32(Active);
            }
        }

    }
    #endregion


    #region Varus

    public class LivingVengeance : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public bool Active { get; set; }
        public double AttackSpeedBuff { get; set; }
        public double AttackSpeedScaling { get; set; }
        public int LevelThreshold { get; set; }


        public double CalculateDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double ToggleAbility(ChampionKit champion)
        {
            int scalingMultiplier = 0;
            double attackSpeedBuff = AttackSpeedBuff, bonus = 0;


            if (Active)
            {
                champion.BuiltAttackSpeed -= attackSpeedBuff;
                
                bonus = champion.BonusAttackSpeed * (AttackSpeedScaling / 100);
                champion.BuiltAttackDamage -= bonus;
                champion.BonusAbilityPower -= bonus;
                Active = false;
                return Convert.ToInt32(Active);

            }
            else
            {
                bonus = champion.BonusAttackSpeed * (AttackSpeedScaling / 100);
                champion.BuiltAttackDamage += bonus;
                champion.BonusAbilityPower += bonus;
                champion.BuiltAttackSpeed += attackSpeedBuff;
                Active = true;
                return Convert.ToInt32(Active);
            }
        }

    }

    public class PiercingArrow : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public bool Active { get; set; }
        public double BaseDamage { get; set; }
        public double BaseDamageGrowth { get; set; }
        public double AttackDamageScaling { get; set; }
        public double AttackDamageScalingGrowth { get; set; }

        public double CalculateDamage(DamageCalculationContext context)
        {
            int level = Level;
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (level - 1);
            attackDamageScaling += AttackDamageScalingGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output += baseDamage + (attackDamageScaling * context.BonusAttackDamage);

            output = output * (context.BonusDamage + 1);
            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseDamage, attackDamageScaling = AttackDamageScaling;

            baseDamage += BaseDamageGrowth * (Level - 1);
            attackDamageScaling /= 100;

            output += baseDamage + (attackDamageScaling * context.BonusAttackDamage);

            if (CritDamage == 0)
                output = output * context.CritDamage;
            else
                output = output * CritDamage;
            output = output * (context.BonusDamage + 1);
            return output;
        }
        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double ToggleAbility(ChampionKit champion)
        {
            if (Active)
            {
                Active = false;
                return Convert.ToInt32(Active);

            }
            else
            {
                Active = true;
                return Convert.ToInt32(Active);
            }
        }
    }

    public class BlightedQuiver : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion
        public bool Active { get; set; }
        public double BaseOnHitDamage {  get; set; }
        public double BaseOnHitDamageGrowth {  get; set; }
        public double OnHitDamageScaling { get; set; }
        public double MaxHealthBaseDamage { get; set; }
        public double MaxHealthBaseDamageGrowth { get; set; }
        public double MaxHealthDamageScaling { get; set; }
        public double MissingHealthBaseDamage { get; set; }
        public double MissingHealthBaseDamageGrowth { get; set; }


        public double CalculateDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = BaseOnHitDamage, scaling = OnHitDamageScaling;

            baseDamage += BaseOnHitDamageGrowth * (Level - 1);
            scaling /= 100;

            output = baseDamage + (scaling * context.BonusAbilityPower);
            

            return output;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            double output = 0.0, baseDamage = MaxHealthBaseDamage, scaling = MaxHealthDamageScaling;


            baseDamage += MaxHealthBaseDamageGrowth * (Level - 1);
            scaling *= (context.BonusAbilityPower / 100);

            output = baseDamage + scaling;

            return output;
        }

        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            double baseDamage = MissingHealthBaseDamage, output = 0.0;

            baseDamage += MissingHealthBaseDamageGrowth * (Level - 1);
            output = 100 * baseDamage;

            return output / 100;
        }

        public double ToggleAbility(ChampionKit champion)
        {
            if (Active)
            {
                Active = false;
                return Convert.ToInt32(Active);
            }
            else
            {
                Active = true;
                return Convert.ToInt32(Active);
            }
        }
    }
    #endregion


    /*  
      public class LivingVengeance : IAbility
    {
        #region Defaults
        public string Name { get; set; }
        public string DamageType { get; set; }
        public bool CanCrit { get; set; }
        public int ImageIndex { get; set; }
        public int Level { get; set; }
        public int HitCount { get; set; }
        public double CritDamage { get; set; }
        #endregion

        public double CalculateDamage(DamageCalculationContext context)
        {
            return -1;
        }
        public double CalculateCritDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double CalculateSecondaryDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double CalculateTertiaryDamage(DamageCalculationContext context)
        {
            return -1;
        }

        public double ToggleAbility(ChampionKit champion)
        {
            return -1;
        }

    }
    */
}
