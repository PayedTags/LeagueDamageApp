using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace wip_LeagueThing
{
    public static class Calculate
    {
        public static double MitigatedBasicAttack(ChampionKit damageDealer, ChampionKit damageTaker)
        {
            double basicAttackDamage = 0.0, effectiveArmor = 0.0, damageMultiplier = 1;
            effectiveArmor = EffectiveArmor(damageDealer, damageTaker.TotalArmor);

            basicAttackDamage = damageDealer.TotalAttackDamage;

            basicAttackDamage /= 1 + (effectiveArmor / 100);

            damageMultiplier += damageDealer.BonusDamage;

            basicAttackDamage *= damageMultiplier;

            //apply extra reductions after mitigation

            return basicAttackDamage;
        }

        public static double MitigateMagicDamage(ChampionKit damageDealer, double magicDamage, ChampionKit damageTaker)
        {
            double output = magicDamage, resistences = 0.0;
            resistences = EffectiveMagicResist(damageDealer, damageTaker.TotalMagicResist);
            output /= 1 + (resistences / 100);
            return output;
        }

        public static double MitigatePhysicalDamage(ChampionKit damageDealer, double physicalDamage, ChampionKit damageTaker)
        {
            double output = physicalDamage, resistences = 0.0;
            resistences = EffectiveArmor(damageDealer, damageTaker.TotalArmor);
            output /= 1 + (resistences / 100);
            return output;
        }


        public static double MitigatedCritBasicAttack(ChampionKit damageDealer, ChampionKit damageTaker)
        {
            double basicAttackCritDamage = 0.0, effectiveArmor = 0.0, damageMultiplier = 1;
            effectiveArmor = EffectiveArmor(damageDealer, damageTaker.TotalArmor);

            basicAttackCritDamage = damageDealer.TotalAttackDamage;

            basicAttackCritDamage *= damageDealer.CritDamage;

            basicAttackCritDamage /= 1 + (effectiveArmor / 100);

            damageMultiplier += damageDealer.BonusDamage;

            basicAttackCritDamage *= damageMultiplier;
            //apply extra reductions after mitigation

            return basicAttackCritDamage;
        }

        public static double EffectiveArmor(ChampionKit champion, double armor)
        {
            double effectiveArmor = 0.0;
            effectiveArmor = armor * (1 - (champion.PercentageArmorPen / 100));
            effectiveArmor -= champion.FlatArmorPen;
            if (effectiveArmor < 0)
                effectiveArmor = 0;

            return effectiveArmor;
        }

        public static double EffectiveMagicResist(ChampionKit champion, double magicRegist)
        {
            double effectiveMagicResist = 0.0;
            effectiveMagicResist = magicRegist * (1 - (champion.PercentageMagicPen / 100));
            effectiveMagicResist -= champion.FlatMagicPen;
            if (effectiveMagicResist < 0)
                effectiveMagicResist = 0;

            return effectiveMagicResist;
        }

        //Calculates an ability's damage per hit 
        public static double MitigateAbilityHitDamage(ChampionKit damageDealer, IAbility ability, DamageCalculationContext context, ChampionKit damageTaker)
        {
            double damageDealt = 0.0, effectiveResistence = 0.0, damageMultiplier = 1;
            damageDealt = ability.CalculateDamage(context);
            if (damageDealt == -1)
                return -1;
            //check if manamune or any ability damage amps are active
            switch(ability.DamageType)
            {
                case "Physical":
                    {
                        effectiveResistence = EffectiveArmor(damageDealer, damageTaker.TotalArmor);
                        damageDealt /= 1 + (effectiveResistence / 100);

                        damageMultiplier += (damageDealer.BonusDamage + damageDealer.AbilityBonusDamage);

                        damageDealt *= damageMultiplier;
                        //check for any damage reductions like randuins or leona w
                        return damageDealt;
                    };
                case "Magical":
                    {
                        effectiveResistence = EffectiveMagicResist(damageDealer, damageTaker.TotalMagicResist);
                        damageDealt /= 1 + (effectiveResistence / 100);

                        damageMultiplier += (damageDealer.BonusDamage + damageDealer.AbilityBonusDamage);

                        damageDealt *= damageMultiplier;
                        //check for any damage reductions like randuins or leona w
                        return damageDealt;
                    };
                case "Heal":
                    {
                        damageDealt = HealingAmmount(damageDealt, damageDealer.inventory, context);
                        return damageDealt;
                    };
                case "Shield":
                    {
                        damageDealt = ShieldingAmmount(damageDealt, damageDealer.inventory, context);
                        return damageDealt;
                    };
                default:
                    break;
            }
            return -2;
        }

        //Calculates an ability's damage per hit on crits
        public static double MitigateAbilityCritHitDamage(ChampionKit damageDealer, IAbility ability, DamageCalculationContext context, ChampionKit damageTaker)
        {
            double damageDealt = 0.0, effectiveResistence = 0.0, damageMultiplier = 1;
            damageDealt = ability.CalculateCritDamage(context);
            switch (ability.DamageType)
            {
                case "Physical":
                    {
                        effectiveResistence = EffectiveArmor(damageDealer, damageTaker.TotalArmor);
                        damageDealt /= 1 + (effectiveResistence / 100);

                        damageMultiplier += (damageDealer.BonusDamage + damageDealer.AbilityBonusDamage);

                        damageDealt *= damageMultiplier;
                        //check for any damage reductions like randuins or leona w
                        return damageDealt;
                    };
                case "Magical":
                    {
                        effectiveResistence = EffectiveArmor(damageDealer, damageTaker.TotalMagicResist);
                        damageDealt /= 1 + (effectiveResistence / 100);

                        damageMultiplier += (damageDealer.BonusDamage + damageDealer.AbilityBonusDamage);

                        damageDealt *= damageMultiplier;
                        //check for any damage reductions like randuins or leona w
                        return damageDealt;
                    };
            }
            return -2;
        }

        //Calculates ability total damage (all hits)
        public static double MitigateAbilityTotalDamage(ChampionKit damageDealer, IAbility ability, DamageCalculationContext context, ChampionKit damageTaker)
        {
            double output = 0.0;
            for (int i = 0; i < ability.HitCount;i++)
            {
                output += MitigateAbilityHitDamage(damageDealer, ability, context, damageTaker);
            }
            return output;
        }

        //Calculates ability total damage on crits (all hits)
        public static double MitigateAbilityTotalCritDamage(ChampionKit damageDealer, IAbility ability, DamageCalculationContext context, ChampionKit damageTaker)
        {
            double output = 0.0;
            for (int i = 0; i < ability.HitCount; i++)
            {
                output += MitigateAbilityCritHitDamage(damageDealer, ability, context, damageTaker);
            }
            return output;
        }

        //Calculates ability total damage with simulated crit chance per hit
        public static double AverageMitigatedAbilityDamage(ChampionKit damageDealer, IAbility ability, DamageCalculationContext context, ChampionKit damageTaker)
        {
            Random random = new Random();
            double output = 0.0, roll = 0.0;
            for (int i = 0; i < ability.HitCount; i++)
            {
                roll = random.NextDouble();
                if (roll <= (damageDealer.CritChance / 100) && ability.CanCrit)
                {
                    output += MitigateAbilityCritHitDamage(damageDealer, ability, context, damageTaker);
                }
                else
                {
                    output += MitigateAbilityHitDamage(damageDealer, ability, context, damageTaker);
                }
            }

            return output;
        }

       public static double MaxHealthAbilityDamage(ChampionKit damageDealer, IAbility ability, double percentage, DamageCalculationContext context, ChampionKit damageTaker)
        {
            double damageDealt = 0.0, effectiveResistence = 0.0, damageMultiplier = 0.0;
            switch (ability.DamageType)
            {
                case "Physical":
                    {
                        effectiveResistence = EffectiveArmor(damageDealer, damageTaker.TotalArmor);
                        damageDealt = context.EnemyMaxHealth * (percentage / 100);
                        damageDealt /= 1 + (effectiveResistence / 100);

                        damageMultiplier += (damageDealer.BonusDamage + damageDealer.AbilityBonusDamage);

                        damageDealt *= 1 + damageMultiplier;
                        //check for any damage reductions like randuins or leona w
                        return damageDealt;
                    };
                case "Magical":
                    {
                        effectiveResistence = EffectiveArmor(damageDealer, damageTaker.TotalMagicResist);
                        damageDealt = context.EnemyMaxHealth * (percentage / 100);
                        damageDealt /= 1 + (effectiveResistence / 100);

                        damageMultiplier += (damageDealer.BonusDamage + damageDealer.AbilityBonusDamage);

                        damageDealt *= 1 + damageMultiplier;
                        //check for any damage reductions like randuins or leona w
                        return damageDealt;
                    };
                default:
                    return -2;
            }
        }

        public static double MissingHealthAbilityDamage(ChampionKit damageDealer, IAbility ability, double percentage, DamageCalculationContext context, ChampionKit damageTaker)
        {
            double damageDealt = 0.0, effectiveResistence = 0.0, damageMultiplier = 0.0;
            switch (ability.DamageType)
            {
                case "Physical":
                    {
                        effectiveResistence = EffectiveArmor(damageDealer, damageTaker.TotalArmor);
                        damageDealt = (context.EnemyMaxHealth - context.EnemyCurrentHealth) * (percentage / 100);
                        damageDealt /= 1 + (effectiveResistence / 100);

                        damageMultiplier += (damageDealer.BonusDamage + damageDealer.AbilityBonusDamage);

                        damageDealt *= 1 + damageMultiplier;
                        //check for any damage reductions like randuins or leona w
                        return damageDealt;
                    };
                case "Magical":
                    {
                        effectiveResistence = EffectiveArmor(damageDealer, damageTaker.TotalMagicResist);
                        damageDealt = (context.EnemyMaxHealth - context.EnemyCurrentHealth) * (percentage / 100);
                        damageDealt /= 1 + (effectiveResistence / 100);

                        damageMultiplier += (damageDealer.BonusDamage + damageDealer.AbilityBonusDamage);

                        damageDealt *= 1 + damageMultiplier;
                        //check for any damage reductions like randuins or leona w
                        return damageDealt;
                    };
                default:
                    return -2;
            }
        }

        //Calculates each ability's hit with simulated crit chance to then output a string of each hit damage
        //Used for logging
        public static string AverageMitigatedAbilityHitDamageOutput(ChampionKit damageDealer, IAbility ability, DamageCalculationContext context, ChampionKit damageTaker)
        {
            Random random = new Random();
            string text = "";
            double roll = 0.0;
            text += "\r\n" + ability.Name + " Damage: ";
            if (ability.HitCount > 1)
            {
                for (int i = 0; i < ability.HitCount; i++)
                {
                    double hitDamage = 0;
                    roll = random.NextDouble();
                    if (roll <= (damageDealer.CritChance / 100) && ability.CanCrit)
                    {
                        hitDamage = MitigateAbilityCritHitDamage(damageDealer, ability, context, damageTaker);
                        text += "\r\n      " + hitDamage.ToString("0.00") + "!!!";
                    }
                    else
                    {
                        hitDamage = MitigateAbilityHitDamage(damageDealer, ability, context, damageTaker);
                        text += "\r\n      " + hitDamage.ToString("0.00");
                    }
                }
            }
            else
            {
                double hitDamage = 0;
                roll = random.NextDouble();
                if (roll <= (damageDealer.CritChance / 100) && ability.CanCrit)
                {
                    hitDamage = MitigateAbilityCritHitDamage(damageDealer, ability, context, damageTaker);
                    text += hitDamage.ToString("0.00") + "!!!";
                }
                else
                {
                    hitDamage = MitigateAbilityHitDamage(damageDealer, ability, context, damageTaker);
                    text += hitDamage.ToString("0.00");
                }   
            }
            return text;
        }

        #region Extra info
        public static double ExtraDamageFromArmorPen(ChampionKit damageDealer, ChampionKit damageTaker)
        {
            double prePen = 100, postPen = 100, bonus = 0.0;
            double effectiveArmor = 0.0;

            effectiveArmor = EffectiveArmor(damageDealer, damageTaker.TotalArmor);
            postPen /= 1 + (effectiveArmor / 100);

            prePen /= 1 + (damageTaker.TotalArmor / 100);

            bonus = postPen / prePen;
            bonus -= 1;
            bonus *= 100;

            return bonus;
        }

        public static double ExtraDamageFromMagicPen(ChampionKit damageDealer, ChampionKit damageTaker)
        {
            double prePen = 100, postPen = 100, bonus = 0.0;
            double effectiveMagicResist = 0.0;

            effectiveMagicResist = EffectiveMagicResist(damageDealer, damageTaker.TotalMagicResist);
            postPen /= 1 + (effectiveMagicResist / 100);

            prePen /= 1 + (damageTaker.TotalMagicResist / 100);

            bonus = postPen / prePen;
            bonus -= 1;
            bonus *= 100;

            return bonus;
        }

        public static int CritHitsCount( double damageDealt, IAbility ability, DamageCalculationContext context)
        {
            double damagePerHit = ability.CalculateDamage(context);
            if (damageDealt > damagePerHit * ability.HitCount)
            {
                if(ability.CritDamage != 0)
                    return Convert.ToInt32((damageDealt - (ability.HitCount * damagePerHit)) / (damagePerHit * (ability.CritDamage - 1)));
                else
                    return Convert.ToInt32((damageDealt - (ability.HitCount * damagePerHit)) / (damagePerHit * (context.CritDamage - 1)));

            }
            else
                return 0;
        }
        //wip
        public static double CalculatePercentageDamageFromCrits()
        {
            return 0;
        }

        public static double CalculateDamageFromCrits(IAbility ability, int critHits, DamageCalculationContext context)
        {
            double damagePerHit = ability.CalculateDamage(context);
            double critHit = 0.0;
            if (ability.CritDamage != 0)
                critHit = damagePerHit * ability.CritDamage;
            else
                critHit = damagePerHit * context.CritDamage;

            return (critHit * critHits) - (damagePerHit * critHits) ;
        }
        #endregion

        //check for revitalize later
        public static double HealingAmmount(double heal, List<ShopItems> inventory, DamageCalculationContext context)
        {
            bool moonstone = false;
            double healOutput = heal, moonstoneBonus = 0.0;
            foreach (ShopItems item in inventory)
            {
                if (item.Name == "Moonstone")
                {
                    moonstone = true;
                    break;
                }
            }
            healOutput *= 1 + (context.HealShieldPower / 100);
            if(moonstone)
            {
                moonstoneBonus = healOutput * 0.3;
                moonstoneBonus *= 1 + (context.HealShieldPower / 100);
                healOutput += moonstoneBonus;
            }

            return healOutput; 
        }

        //check for revitalize later
        public static double ShieldingAmmount(double shield, List<ShopItems> inventory, DamageCalculationContext context)
        {
            bool moonstone = false;
            double shieldOutput = shield, moonstoneBonus = 0.0;
            foreach (ShopItems item in inventory)
            {
                if (item.Name == "Moonstone")
                {
                    moonstone = true;
                    break;
                }
            }
            shieldOutput *= 1 + (context.HealShieldPower / 100);
            if (moonstone)
            {
                moonstoneBonus = shieldOutput * 0.35;
                moonstoneBonus *= 1 + (context.HealShieldPower / 100);
                shieldOutput += moonstoneBonus;
            }

            return shieldOutput;
        }

    }
}
