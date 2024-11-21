using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Text.Json;
using wip_LeagueThing;

namespace wip_LeagueThing
{
    public partial class Comparison : Form
    {


        ChampionKit MainChampion = new ChampionKit();
        ChampionKit SecondaryChampion = new ChampionKit();
        bool twoChamps = false;

        List<System.Windows.Forms.ListView> abilitiesIcons1 = new List<System.Windows.Forms.ListView>();
        List<System.Windows.Forms.ListView> abilitiesIcons2 = new List<System.Windows.Forms.ListView>();

        List<System.Windows.Forms.TextBox> abilitiesDamage1 = new List<System.Windows.Forms.TextBox>();
        List<System.Windows.Forms.TextBox> abilitiesDamage2 = new List<System.Windows.Forms.TextBox>();

        List<System.Windows.Forms.TextBox> abilitiesDamageCompare1 = new List<System.Windows.Forms.TextBox>();
        List<System.Windows.Forms.TextBox> abilitiesDamageCompare2 = new List<System.Windows.Forms.TextBox>();

        List<System.Windows.Forms.Button> abilitiesToggle1 = new List<System.Windows.Forms.Button>();
        List<System.Windows.Forms.Button> abilitiesToggle2 = new List<System.Windows.Forms.Button>();

        bool[] mainChampionActiveAbilities = new bool[5];
        bool[] secondaryChampionActiveAbilities = new bool[5];

        bool secondChampLoaded = false;

        List<ShopItems> shopItems = new List<ShopItems>();

        Color physicalDamageColor = Color.FromArgb(255, 140, 52);
        Color magicalDamageColor = Color.FromArgb(0, 176, 240);





        public Comparison(string[] championName)
        {
            InitializeComponent();
            MainChampion = new ChampionKit().LoadChampionFromJson(championName[0]);
            if (championName[1] != null)
            {
                SecondaryChampion = new ChampionKit().LoadChampionFromJson(championName[1]);
            }
            else
            {
                SecondaryChampion = MainChampion;
            }
            twoChamps = true;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //This listings are made so it's easier to then write the data on the proper fields 
            //Makes it so when i loop through champion abilities i know which textboxes should get that ability information

            #region Listing icons
            abilitiesIcons1.Add(lstview_Passive1);
            abilitiesIcons2.Add(lstview_Passive2);
            abilitiesIcons1.Add(lstview_Q1);
            abilitiesIcons2.Add(lstview_Q2);
            abilitiesIcons1.Add(lstview_W1);
            abilitiesIcons2.Add(lstview_W2);
            abilitiesIcons1.Add(lstview_E1);
            abilitiesIcons2.Add(lstview_E2);
            abilitiesIcons1.Add(lstview_R1);
            abilitiesIcons2.Add(lstview_R2);
            #endregion

            #region Listing textboxes
            abilitiesDamage1.Add(txtbox_AADamage1);
            abilitiesDamage2.Add(txtbox_AADamage2);

            abilitiesDamage1.Add(txtbox_PassiveDamage1);
            abilitiesDamage2.Add(txtbox_PassiveDamage2);

            abilitiesDamage1.Add(txtbox_QDamage1);
            abilitiesDamage2.Add(txtbox_QDamage2);

            abilitiesDamage1.Add(txtbox_WDamage1);
            abilitiesDamage2.Add(txtbox_WDamage2);

            abilitiesDamage1.Add(txtbox_EDamage1);
            abilitiesDamage2.Add(txtbox_EDamage2);

            abilitiesDamage1.Add(txtbox_RDamage1);
            abilitiesDamage2.Add(txtbox_RDamage2);

            abilitiesDamageCompare1.Add(txtbox_AADamageCompare1);
            abilitiesDamageCompare2.Add(txtbox_AADamageCompare2);

            abilitiesDamageCompare1.Add(txtbox_PassiveDamageCompare1);
            abilitiesDamageCompare2.Add(txtbox_PassiveDamageCompare2);

            abilitiesDamageCompare1.Add(txtbox_QDamageCompare1);
            abilitiesDamageCompare2.Add(txtbox_QDamageCompare2);

            abilitiesDamageCompare1.Add(txtbox_WDamageCompare1);
            abilitiesDamageCompare2.Add(txtbox_WDamageCompare2);

            abilitiesDamageCompare1.Add(txtbox_EDamageCompare1);
            abilitiesDamageCompare2.Add(txtbox_EDamageCompare2);

            abilitiesDamageCompare1.Add(txtbox_RDamageCompare1);
            abilitiesDamageCompare2.Add(txtbox_RDamageCompare2);
            #endregion

            #region Listing Buttons
            abilitiesToggle1.Add(btn_TogglePassive1);
            abilitiesToggle2.Add(btn_TogglePassive2);
            abilitiesToggle1.Add(btn_ToggleQ1);
            abilitiesToggle2.Add(btn_ToggleQ2);
            abilitiesToggle1.Add(btn_ToggleW1);
            abilitiesToggle2.Add(btn_ToggleW2);
            abilitiesToggle1.Add(btn_ToggleE1);
            abilitiesToggle2.Add(btn_ToggleE2);
            abilitiesToggle1.Add(btn_ToggleR1);
            abilitiesToggle2.Add(btn_ToggleR2);
            #endregion


            LoadItemsFromJson();
            MainChampion.inventory.Add(shopItems[4]);
            TempItems(MainChampion);
            cmbBox_Level1.SelectedIndex = 0;

            if (twoChamps)
            {
                cmbBox_Level2.SelectedIndex = 0;
                secondChampLoaded = true;
            }


        }

        #region LevelChanged
        //Whenever main champion changes level
        //Updates stats based on the level up
        private void LevelSelected(object sender, EventArgs e)
        {
            UpdateShownStats(MainChampion, txtbox_Stats1, cmbBox_Level1, lstview_Icons1, txtBox_Champion1Name, abilitiesIcons1, abilitiesToggle1);
            
            if (secondChampLoaded)
                LevelUpHeal(MainChampion);
            CalculateChampCompareDamage(MainChampion, SecondaryChampion, abilitiesDamageCompare1);
            CalculateChampCompareDamage(SecondaryChampion, MainChampion, abilitiesDamageCompare2);
        }


        //Whenever secondary champion changes level
        //Updates stats based on the level up
        private void LevelSelectedSecondary(object sender, EventArgs e)
        {
            UpdateShownStats(SecondaryChampion, txtbox_Stats2, cmbBox_Level2, lstView_Icons2, txtbox_Champion2Name, abilitiesIcons2, abilitiesToggle2);
            
            if (secondChampLoaded)
                LevelUpHeal(SecondaryChampion);
            CalculateChampCompareDamage(MainChampion, SecondaryChampion, abilitiesDamageCompare1);
            CalculateChampCompareDamage(SecondaryChampion, MainChampion, abilitiesDamageCompare2);
        }
        #endregion

        #region UpdateChampStats
        //Function responsible for actually writing the stats into the UI
        //Also calls a function to calculate the simplified damage of the champions which itself writes the damage calculated
        private void UpdateShownStats(ChampionKit champion, System.Windows.Forms.TextBox textBoxStats, System.Windows.Forms.ComboBox comboBoxLevel,
            System.Windows.Forms.ListView listViewIcon, System.Windows.Forms.TextBox textBoxName, List<System.Windows.Forms.ListView> abilityIcon,
            List<System.Windows.Forms.Button> abilityToggle)
        {
            textBoxStats.Text = "";

            //deactivate buffs before level up
            bool[] actives = new bool[champion.Abilities.Count];
            for (int i = 0; i < champion.Abilities.Count; i++)
            {
                if (CheckIfActive(champion, champion.Abilities[i]))
                {
                    actives[i] = true;
                    champion.Abilities[i].ToggleAbility(champion);
                }
            }

            champion.Level = comboBoxLevel.SelectedIndex + 1;
            if (secondChampLoaded)
                UpdateChampTotalStats(champion);
            else
                UpdateChampTotalStatsHp(champion);

            //reactivate them so they are correct
            for (int i = 0; i < actives.Count(); i++)
            {
                if (actives[i])
                {
                    champion.Abilities[i].ToggleAbility(champion);
                }
            }
            if (secondChampLoaded)
                UpdateChampTotalStats(champion);
            else
                UpdateChampTotalStatsHp(champion);

            #region Update stats textbox
            textBoxStats.Text += "Health: " + champion.MaxHealth.ToString();
            if (champion.TotalAbilityPower > 0)
                textBoxStats.Text += "\r\nAbility Power: " + champion.TotalAbilityPower.ToString("0.00");
            textBoxStats.Text += "\r\nAttack Damage: " + champion.TotalAttackDamage.ToString("0.00");
            textBoxStats.Text += "\r\nAttackSpeed: " + champion.TotalAttackSpeed.ToString("0.000");
            textBoxStats.Text += "\r\nCrit Chance: " + champion.CritChance.ToString() + "%";
            textBoxStats.Text += "\r\nCrit Damage: " + (champion.CritDamage * 100).ToString() + "%";
            textBoxStats.Text += "\r\nDamage Bonus: " + champion.BonusDamage.ToString() + "%";
            if (champion.FlatArmorPen > 0)
                textBoxStats.Text += "\r\nLethality: " + champion.FlatArmorPen.ToString();
            if (champion.PercentageArmorPen > 0)
                textBoxStats.Text += "\r\nPercentage Armor Pen: " + champion.PercentageArmorPen.ToString() + "%";
            if (champion.FlatMagicPen > 0)
                textBoxStats.Text += "\r\nFlat Magic Pen: " + champion.FlatMagicPen.ToString();
            if (champion.PercentageMagicPen > 0)
                textBoxStats.Text += "\r\nPercentage Magic Pen: " + champion.PercentageMagicPen.ToString() + "%";
            textBoxStats.Text += "\r\nArmor: " + champion.TotalArmor.ToString("0.0");
            textBoxStats.Text += "\r\nMagic Resist: " + champion.TotalMagicResist.ToString("0.0");
            #endregion

            listViewIcon.Items[0].ImageIndex = champion.Icon;
            textBoxName.Text = champion.Name;

            for (int i = 0; i < champion.Abilities.Count; i++)
            {
                abilityIcon[i].Items[0].ImageIndex = champion.Abilities[i].ImageIndex;
                if (CheckToggleAbility(champion, champion.Abilities[i]))
                {
                    abilityToggle[i].Visible = true;
                }
            }
            if (IsMainChampion(champion))
                CalculateSimpleChampDamage(champion, abilitiesDamage1);
            else
                CalculateSimpleChampDamage(champion, abilitiesDamage2);
            if (!IsMainChampion(champion) && !secondChampLoaded)
            {
                secondChampLoaded = true;
                UpdateBothHealthBars();

            }
        }


        //Update all the champion stats
        private void UpdateChampTotalStats(ChampionKit champion)
        {
          

            #region AttackDamage
            champion.BonusAttackDamage = champion.AttackDamageGrowth * (champion.Level - 1);
            champion.BonusAttackDamage += champion.BuiltAttackDamage;
            champion.TotalAttackDamage = champion.BaseAttackDamage + champion.BonusAttackDamage;
            champion.TotalAttackDamage = champion.TotalAttackDamage * (1 + (champion.AttackDamageModifier / 100));
            #endregion

            #region AttackSpeed
            double totalAttackSpeed = 0.0;
            champion.BonusAttackSpeed = champion.AttackSpeedGrowth * (champion.Level - 1);
            champion.BonusAttackSpeed *= 100;
            champion.BonusAttackSpeed += champion.BuiltAttackSpeed;
            totalAttackSpeed = champion.AttackSpeedRatio * (champion.BonusAttackSpeed / 100);
            totalAttackSpeed += champion.BaseAttackSpeed;
            champion.TotalAttackSpeed = totalAttackSpeed;
            #endregion

            #region AbilityPower
            //add items bonuses here
            champion.TotalAbilityPower = champion.BonusAbilityPower * (1 + (champion.AbilityPowerModifier / 100));
            #endregion

            #region Health
            champion.BonusHealth = champion.HealthGrowth * (champion.Level - 1);
            champion.BonusHealth += champion.BuiltHealth;
            champion.MaxHealth = champion.BaseHealth + champion.BonusHealth;
            champion.MaxHealth = champion.MaxHealth * (1 + (champion.HealthModifier / 100));
            #endregion

            #region Armor
            champion.BonusArmor = champion.ArmorGrowth * (champion.Level - 1);
            champion.BonusArmor += champion.BuiltArmor;
            champion.TotalArmor = champion.BaseArmor + champion.BonusArmor;
            champion.TotalArmor = champion.TotalArmor * (1 + (champion.ArmorModifier / 100));
            #endregion

            #region MagicResit
            champion.BonusMagicResist = champion.MagicResistGrowth * (champion.Level - 1);
            champion.BonusMagicResist += champion.BuiltMagicResist;
            champion.TotalMagicResist = champion.BaseMagicResist + champion.BonusMagicResist;
            champion.TotalMagicResist = champion.TotalMagicResist * (1 + (champion.MagicResistModifier / 100));
            #endregion

             
        }


        //Update all the champion stats + heal them fully
        //Used for when they are first loaded so they load in with Health
        private void UpdateChampTotalStatsHp(ChampionKit champion)
        {


            #region AttackDamage
            champion.BonusAttackDamage = champion.AttackDamageGrowth * (champion.Level - 1);
            champion.BonusAttackDamage += champion.BuiltAttackDamage;
            champion.TotalAttackDamage = champion.BaseAttackDamage + champion.BonusAttackDamage;
            champion.TotalAttackDamage = champion.TotalAttackDamage * (1 + (champion.AttackDamageModifier / 100));
            #endregion

            #region AttackSpeed
            double totalAttackSpeed = 0.0;
            champion.BonusAttackSpeed = champion.AttackSpeedGrowth * (champion.Level - 1);
            champion.BonusAttackSpeed *= 100;
            champion.BonusAttackSpeed += champion.BuiltAttackSpeed;
            totalAttackSpeed = champion.AttackSpeedRatio * (champion.BonusAttackSpeed / 100);
            totalAttackSpeed += champion.BaseAttackSpeed;
            champion.TotalAttackSpeed = totalAttackSpeed;
            #endregion

            #region AbilityPower
            champion.TotalAbilityPower = champion.BonusAbilityPower * (1 + (champion.AbilityPowerModifier / 100));
            #endregion

            #region Health
            champion.BonusHealth = champion.HealthGrowth * (champion.Level - 1);
            champion.BonusHealth += champion.BuiltHealth;
            champion.MaxHealth = champion.BaseHealth + champion.BonusHealth;
            champion.MaxHealth = champion.MaxHealth * (1 + (champion.HealthModifier / 100));
            champion.CurrentHealth = champion.MaxHealth;
            #endregion

            #region Armor
            champion.BonusArmor = champion.ArmorGrowth * (champion.Level - 1);
            champion.BonusArmor += champion.BuiltArmor;
            champion.TotalArmor = champion.BaseArmor + champion.BonusArmor;
            champion.TotalArmor = champion.TotalArmor * (1 + (champion.ArmorModifier / 100));
            #endregion

            #region MagicResit
            champion.BonusMagicResist = champion.MagicResistGrowth * (champion.Level - 1);
            champion.BonusMagicResist += champion.BuiltMagicResist;
            champion.TotalMagicResist = champion.BaseMagicResist + champion.BonusMagicResist;
            champion.TotalMagicResist = champion.TotalMagicResist * (1 + (champion.MagicResistModifier / 100));
            #endregion
        }


        //Fully heal specified champion
        private void FullHeal(ChampionKit champion)
        {
            champion.CurrentHealth = champion.MaxHealth;
            UpdateHealthbar(champion);
        }


        //Function to give the heal from leveling up
        private void LevelUpHeal(ChampionKit champion)
        {
            double oldMaxHp = 0.0, currentMaxHp = champion.MaxHealth, levelsUp = 0.0, healAmmount = 0.0;
            string temp = "";
            //get old max health
            if (champion == MainChampion)
                temp = lbl_HealthBarValue1.Text.Split('/')[1];
            else
                temp = lbl_HealthBarValue2.Text.Split('/')[1];
            temp = temp.Split("(")[0];
            oldMaxHp = Convert.ToDouble(temp);

            //if he actually leveled up
            if (oldMaxHp < champion.MaxHealth)
            {
                currentMaxHp -= oldMaxHp;
                levelsUp = currentMaxHp / champion.HealthGrowth;

                healAmmount = champion.HealthGrowth * levelsUp;
                champion.CurrentHealth += healAmmount;
                if (IsMainChampion(champion))
                    LogLevelUp(champion, healAmmount, richtxtbox_Logs1);
                else
                    LogLevelUp(champion, healAmmount, richtxtbox_Logs2);
            }
            else if (oldMaxHp > champion.MaxHealth)
                champion.CurrentHealth = champion.MaxHealth;
            UpdateHealthbar(champion);
        }


        //Update values on the champion's Health bar
        private void UpdateHealthbar(ChampionKit champion)
        {
            double temp = 0.0;
            if (IsMainChampion(champion))
            {
                temp = (champion.CurrentHealth / champion.MaxHealth) * 100.0;
                prgrBar_HealthBar1.Value = Convert.ToInt32(temp);
                lbl_HealthBarValue1.Text = champion.CurrentHealth.ToString("0.0") + "/" + champion.MaxHealth.ToString("0") + "(" + prgrBar_HealthBar1.Value.ToString("0.0") + "%)";
            }
            else
            {
                temp = (champion.CurrentHealth / champion.MaxHealth) * 100.0;
                prgrBar_HealthBar2.Value = Convert.ToInt32(temp);
                lbl_HealthBarValue2.Text = champion.CurrentHealth.ToString("0.0") + "/" + champion.MaxHealth.ToString("0") + "(" + prgrBar_HealthBar2.Value.ToString("0.0") + "%)";
            }
        }


        //Update both champion's Health Bar
        private void UpdateBothHealthBars()
        {
            double temp = 0.0;

            temp = (MainChampion.CurrentHealth / MainChampion.MaxHealth) * 100.0;
            prgrBar_HealthBar1.Value = Convert.ToInt32(temp);
            lbl_HealthBarValue1.Text = MainChampion.CurrentHealth.ToString("0.0") + "/" + MainChampion.MaxHealth.ToString() + "(" + prgrBar_HealthBar1.Value.ToString("0.0") + "%)";

            temp = (SecondaryChampion.CurrentHealth / SecondaryChampion.MaxHealth) * 100.0;
            prgrBar_HealthBar2.Value = (Convert.ToInt32(temp));
            lbl_HealthBarValue2.Text = SecondaryChampion.CurrentHealth.ToString("0.0") + "/" + SecondaryChampion.MaxHealth.ToString() + "(" + prgrBar_HealthBar2.Value.ToString("0.0") + "%)";

        }

        #endregion

        #region UpdateLogs
        //Logs on the proper textbox that the champion leveled up and how much he healed if they healed from it
        public void LogLevelUp(ChampionKit champion, double healAmmount, RichTextBox richTextBox)
        {
            richTextBox.SelectedText = champion.Name + " leveled up" + "\n";
            if (healAmmount > 0)
            {
                richTextBox.SelectionColor = Color.Green;
                richTextBox.SelectedText = "And healed " + healAmmount.ToString() + "hp" + "\n";
            }
        }

        public void LogDamageDealt(ChampionKit damageDealer, IAbility ability, double damageDealt, RichTextBox richTextBox)
        {
            richTextBox.SelectedText = ability.Name + " Damage: " + "\n";

            richTextBox.SelectionColor = GetDamageTypeColor(ability.DamageType);
            richTextBox.SelectedText = damageDealt.ToString("0.00") + "\n";

        }

        public void LogHealAmmount(ChampionKit damageDealer, IAbility ability, double healingAmmount, RichTextBox richTextBox)
        {
            richTextBox.SelectedText = ability.Name + " Healing: " + "\n";

            richTextBox.SelectionColor = Color.Green;
            richTextBox.SelectedText = healingAmmount.ToString("0.00") + "\n";
        }

        private void LogBasicAttackDamageOnHit(ChampionKit damageDealer, double basicAttackDamage, double onHitDamage, RichTextBox richTextBox)
        {
            richTextBox.SelectedText = "Basic Attack Damage: " + "\n";

            richTextBox.SelectionColor = physicalDamageColor;
            richTextBox.SelectedText = basicAttackDamage.ToString("0.00");

            richTextBox.SelectionColor = magicalDamageColor;
            richTextBox.SelectedText += " + " + onHitDamage.ToString("0") + "\n";
        }

        public void LogBasicAttackDamage(ChampionKit damageDealer, double damageDealt, RichTextBox richTextBox)
        {
            richTextBox.SelectedText = "Basic Attack Damage: " + "\n";

            richTextBox.SelectionColor = physicalDamageColor;
            richTextBox.SelectedText = damageDealt.ToString("0.00") + "\n";
        }

        public void LogMixedDamage(string physicalDamageSource, double physicalDamage, double magicalDamage, RichTextBox richTextBox)
        {
            richTextBox.SelectedText = physicalDamageSource + " Damage: " + "\n";

            richTextBox.SelectionColor = physicalDamageColor;
            richTextBox.SelectedText = physicalDamage.ToString("0.0") + " " + magicalDamage.ToString("0.0") + "\n";

        }
        public void LogMixedDamage(string physicalDamageSource, double physicalDamage, double magicalDamage, string magicDamageSource, RichTextBox richTextBox)
        {
            richTextBox.SelectedText = physicalDamageSource + "(" + magicDamageSource + ") Damage: " + "\n";

            richTextBox.SelectionColor = physicalDamageColor;
            richTextBox.SelectedText = physicalDamage.ToString("0.0") + " ";

            richTextBox.SelectionColor = magicalDamageColor;
            richTextBox.SelectedText += magicalDamage.ToString("0.0") + "\n";
        }

        public void LogToggleAbilityDamage(ChampionKit damageDealer, double basicAttackDamage, IAbility ability, double damageDealt, RichTextBox richTextBox)
        {

            double output = 0.0;
            output = basicAttackDamage + damageDealt;

            richTextBox.SelectedText = "Basic Attack(" + ability.Name + ")" + " Damage:" + "\n";

            richTextBox.SelectionColor = physicalDamageColor;
            richTextBox.SelectedText = output.ToString("0.00");

            richTextBox.SelectionColor = Color.Black;
            richTextBox.SelectedText += "(";

            richTextBox.SelectionColor = GetDamageTypeColor(ability.DamageType);
            richTextBox.SelectedText += damageDealt.ToString("0.00");

            richTextBox.SelectionColor = Color.Black;
            richTextBox.SelectedText += ")" + "\n";
        }


        public Color GetDamageTypeColor(string damageType)
        {
            switch (damageType)
            {
                case "Physical":
                    return physicalDamageColor;
                case "Magical":
                    return magicalDamageColor;
                default:
                    return Color.Black;

            }
        }

        #endregion

        #region Specifics
        //Checks if specified champion is the one on top
        //Very usefull to know where to write down the data on the ui
        public bool IsMainChampion(ChampionKit champion)
        {
            return champion == MainChampion;
        }


        //Get required data so that abilities can calculate the damage they deal
        private DamageCalculationContext LoadChampionContext(ChampionKit champion)
        {
            DamageCalculationContext context = new DamageCalculationContext();

            context.BaseAttackDamage = champion.BaseAttackDamage;
            context.BonusAttackDamage = champion.BuiltAttackDamage;
            context.TotalAttackDamage = champion.TotalAttackDamage;
            context.BonusAbilityPower = champion.TotalAbilityPower;
            context.BonusAttackSpeed = champion.BonusAttackSpeed;
            context.BonusHealth = champion.BonusHealth;
            context.MaxHealth = champion.MaxHealth;
            context.Level = champion.Level;
            context.CritDamage = champion.CritDamage;
            context.CritChance = champion.CritChance;
            context.BonusDamage = champion.BonusDamage;
            context.AbilityBonusDamage = champion.AbilityBonusDamage;
            context.Abilities = champion.Abilities;
            return context;
        }


        //Get required data so that abilities can calculate the damage they deal
        //this is more specific to abilities that have damage based on an enemy stat
        private DamageCalculationContext LoadChampionCompareContext(ChampionKit damageDealer, ChampionKit damageTaker)
        {
            DamageCalculationContext context = new DamageCalculationContext();

            context.BaseAttackDamage = damageDealer.BaseAttackDamage;
            context.BonusAttackDamage = damageDealer.BuiltAttackDamage;
            context.TotalAttackDamage = damageDealer.TotalAttackDamage;
            context.BonusAbilityPower = damageDealer.TotalAbilityPower;
            context.BonusAttackSpeed = damageDealer.BonusAttackSpeed;
            context.BonusHealth = damageDealer.BonusHealth;
            context.MaxHealth = damageDealer.MaxHealth;
            context.Level = damageDealer.Level;
            context.CritDamage = damageDealer.CritDamage;
            context.CritChance = damageDealer.CritChance;
            context.BonusDamage = damageDealer.BonusDamage;
            context.AbilityBonusDamage = damageDealer.AbilityBonusDamage;
            context.Abilities = damageDealer.Abilities;

            context.EnemyMaxHealth = damageTaker.MaxHealth;
            context.EnemyCurrentHealth = damageTaker.CurrentHealth;
            context.EnemyArmor = damageTaker.TotalArmor;
            context.EnemyMagicResist = damageTaker.TotalMagicResist;
            return context;
        }

        //temp
        private void TempItems(ChampionKit champion)
        {
            for (int i = 0; i < champion.inventory.Count; i++)
            {
                champion.inventory[i].Bought(champion);
            }
        }


        //Load items from Json File
        private void LoadItemsFromJson()
        {
            string jsonData = File.ReadAllText("F:\\Projects\\wip_LeagueThing\\wip_LeagueThing\\ItemData.Json");

            JsonDocument doc = JsonDocument.Parse(jsonData);

            foreach (var item in doc.RootElement.EnumerateArray())
            {
                ShopItems tempItem;
                switch (item.GetProperty("type").GetString())
                {
                    case "AdCritFlatArmorPen":
                        tempItem = new AdCritFlatArmorPen
                        {
                            Name = item.GetProperty("name").GetString(),
                            AttackDamage = item.GetProperty("attackDamage").GetDouble(),
                            CritChance = item.GetProperty("critChance").GetDouble(),
                            FlatArmorPen = item.GetProperty("flatArmorPen").GetDouble()
                        };
                        shopItems.Add(tempItem);
                        break;
                    case "AdCritPercentageArmorPen":
                        tempItem = new AdCritPercentageArmorPen
                        {
                            Name = item.GetProperty("name").GetString(),
                            AttackDamage = item.GetProperty("attackDamage").GetDouble(),
                            CritChance = item.GetProperty("critChance").GetDouble(),
                            PercentageArmorPen = item.GetProperty("percentageArmorPen").GetDouble()
                        };
                        shopItems.Add(tempItem);
                        break;
                    case "InfinityEdge":
                        tempItem = new InfinityEdge
                        {
                            Name = item.GetProperty("name").GetString(),
                            AttackDamage = item.GetProperty("attackDamage").GetDouble(),
                            CritChance = item.GetProperty("critChance").GetDouble(),
                            CritDamage = item.GetProperty("critDamage").GetDouble()
                        };
                        shopItems.Add(tempItem);
                        break;
                    case "ApModifier":
                        tempItem = new Rabadons
                        {
                            Name = item.GetProperty("name").GetString(),
                            AbilityPower = item.GetProperty("abilityPower").GetDouble(),
                            ApModifier = item.GetProperty("apModifier").GetDouble()
                        };
                        shopItems.Add(tempItem);
                        break;
                    case "ApAhHealthManaRegen":
                        tempItem = new ApAhHealthManaRegen
                        {
                            Name = item.GetProperty("name").GetString(),
                            AbilityPower = item.GetProperty("abilityPower").GetDouble(),
                            AbilityHaste = item.GetProperty("abilityHaste").GetDouble(),
                            Health = item.GetProperty("health").GetDouble(),
                            BaseManaRegeneration = item.GetProperty("baseManaRegen").GetDouble(),
                        };
                        shopItems.Add(tempItem);
                        break;
                }

            }
        }


      

        #endregion

        #region Ability Levels
        //Levels up specified ability from specified champion
        private void LevelUpAbility(ChampionKit champion, int index)
        {
            int max = 5;
            if (index == 4)
                max = 3;
            if (champion.Abilities[index].Level < max)
            {
                champion.Abilities[index].Level = champion.Abilities[index].Level + 1;
                if (IsMainChampion(champion))
                    UpdateShownStats(champion, txtbox_Stats1, cmbBox_Level1, lstview_Icons1, txtBox_Champion1Name, abilitiesIcons1, abilitiesToggle1);
                else
                    UpdateShownStats(champion, txtbox_Stats2, cmbBox_Level2, lstView_Icons2, txtbox_Champion2Name, abilitiesIcons2, abilitiesToggle2);

                CalculateChampCompareDamage(MainChampion, SecondaryChampion, abilitiesDamageCompare1);
                CalculateChampCompareDamage(SecondaryChampion, MainChampion, abilitiesDamageCompare2);
            }
        }


        //Levels down specified ability from specified champion
        private void LevelDownAbility(ChampionKit champion, int index)
        {
            if (champion.Abilities[index].Level > 1)
            {
                champion.Abilities[index].Level = champion.Abilities[index].Level - 1;
                if (IsMainChampion(champion))
                    UpdateShownStats(champion, txtbox_Stats1, cmbBox_Level1, lstview_Icons1, txtBox_Champion1Name, abilitiesIcons1, abilitiesToggle1);
                else
                    UpdateShownStats(champion, txtbox_Stats2, cmbBox_Level2, lstView_Icons2, txtbox_Champion2Name, abilitiesIcons2, abilitiesToggle2);

                CalculateChampCompareDamage(MainChampion, SecondaryChampion, abilitiesDamageCompare1);
                CalculateChampCompareDamage(SecondaryChampion, MainChampion, abilitiesDamageCompare2);
            }
        }

        private void btn_LevelUpQ1_Click(object sender, EventArgs e)
        {
            if (mainChampionActiveAbilities[1])
            {
                btn_ToggleQ1_Click(sender, e);
                LevelUpAbility(MainChampion, 1);
                btn_ToggleQ1_Click(sender, e);
            }
            else
            {
                LevelUpAbility(MainChampion, 1);
            }
        }
        private void btn_LevelDownQ1_Click(object sender, EventArgs e)
        {
            if (mainChampionActiveAbilities[1])
            {
                btn_ToggleQ1_Click(sender, e);
                LevelDownAbility(MainChampion, 1);
                btn_ToggleQ1_Click(sender, e);
            }
            else
            {
                LevelDownAbility(MainChampion, 1);
            }
        }

        private void btn_LevelUpW1_Click(object sender, EventArgs e)
        {
            if (mainChampionActiveAbilities[2])
            {
                btn_ToggleW1_Click(sender, e);
                LevelUpAbility(MainChampion, 2);
                btn_ToggleW1_Click(sender, e);
            }
            else
            {
                LevelUpAbility(MainChampion, 2);
            }
        }
        private void btn_LevelDownW1_Click(object sender, EventArgs e)
        {
            if (mainChampionActiveAbilities[2])
            {
                btn_ToggleW1_Click(sender, e);
                LevelDownAbility(MainChampion, 2);
                btn_ToggleW1_Click(sender, e);
            }
            else
            {
                LevelDownAbility(MainChampion, 2);
            }
        }

        private void btn_LevelUpE1_Click(object sender, EventArgs e)
        {
            if (mainChampionActiveAbilities[3])
            {
                btn_ToggleE1_Click(sender, e);
                LevelUpAbility(MainChampion, 3);
                btn_ToggleE1_Click(sender, e);
            }
            else
            {
                LevelUpAbility(MainChampion, 3);
            }
        }
        private void btn_LevelDownE1_Click(object sender, EventArgs e)
        {
            if (mainChampionActiveAbilities[3])
            {
                btn_ToggleE1_Click(sender, e);
                LevelDownAbility(MainChampion, 3);
                btn_ToggleE1_Click(sender, e);
            }
            else
            {
                LevelDownAbility(MainChampion, 3);
            }
        }

        private void btn_LevelUpR1_Click(object sender, EventArgs e)
        {
            if (mainChampionActiveAbilities[4])
            {
                btn_ToggleR1_Click(sender, e);
                LevelUpAbility(MainChampion, 4);
                btn_ToggleR1_Click(sender, e);
            }
            else
            {
                LevelUpAbility(MainChampion, 4);
            }
        }
        private void btn_LevelDownR1_Click(object sender, EventArgs e)
        {
            if (mainChampionActiveAbilities[4])
            {
                btn_ToggleR1_Click(sender, e);
                LevelDownAbility(MainChampion, 4);
                btn_ToggleR1_Click(sender, e);
            }
            else
            {
                LevelDownAbility(MainChampion, 4);
            }
        }

        private void btn_LevelUpQ2_Click(object sender, EventArgs e)
        {
            if (secondaryChampionActiveAbilities[1])
            {
                btn_ToggleQ2_Click(sender, e);
                LevelUpAbility(SecondaryChampion, 1);
                btn_ToggleQ2_Click(sender, e);
            }
            else
            {
                LevelUpAbility(SecondaryChampion, 1);
            }
        }
        private void btn_LevelDownQ2_Click(object sender, EventArgs e)
        {
            if (secondaryChampionActiveAbilities[1])
            {
                btn_ToggleQ2_Click(sender, e);
                LevelDownAbility(SecondaryChampion, 1);
                btn_ToggleQ2_Click(sender, e);
            }
            else
            {
                LevelDownAbility(SecondaryChampion, 1);
            }
        }

        private void btn_LevelUpW2_Click(object sender, EventArgs e)
        {
            if (secondaryChampionActiveAbilities[2])
            {
                btn_ToggleW2_Click(sender, e);
                LevelUpAbility(SecondaryChampion, 2);
                btn_ToggleW2_Click(sender, e);
            }
            else
            {
                LevelUpAbility(SecondaryChampion, 2);
            }
        }
        private void btn_LevelDownW2_Click(object sender, EventArgs e)
        {
            if (secondaryChampionActiveAbilities[2])
            {
                btn_ToggleW2_Click(sender, e);
                LevelDownAbility(SecondaryChampion, 2);
                btn_ToggleW2_Click(sender, e);
            }
            else
            {
                LevelDownAbility(SecondaryChampion, 2);
            }
        }

        private void btn_LevelUpE2_Click(object sender, EventArgs e)
        {
            if (secondaryChampionActiveAbilities[3])
            {
                btn_ToggleE2_Click(sender, e);
                LevelUpAbility(SecondaryChampion, 3);
                btn_ToggleE2_Click(sender, e);
            }
            else
            {
                LevelUpAbility(SecondaryChampion, 3);
            }
        }
        private void btn_LevelDownE2_Click(object sender, EventArgs e)
        {
            if (secondaryChampionActiveAbilities[3])
            {
                btn_ToggleE2_Click(sender, e);
                LevelDownAbility(SecondaryChampion, 3);
                btn_ToggleE2_Click(sender, e);
            }
            else
            {
                LevelDownAbility(SecondaryChampion, 3);
            }
        }

        private void btn_LevelUpR2_Click(object sender, EventArgs e)
        {
            if (secondaryChampionActiveAbilities[4])
            {
                btn_ToggleR2_Click(sender, e);
                LevelUpAbility(SecondaryChampion, 4);
                btn_ToggleR2_Click(sender, e);
            }
            else
            {
                LevelUpAbility(SecondaryChampion, 4);
            }
        }
        private void btn_LevelDownR2_Click(object sender, EventArgs e)
        {
            if (secondaryChampionActiveAbilities[4])
            {
                btn_ToggleR2_Click(sender, e);
                LevelDownAbility(SecondaryChampion, 4);
                btn_ToggleR2_Click(sender, e);
            }
            else
            {
                LevelDownAbility(SecondaryChampion, 4);
            }
        }

        #endregion

        #region Ability toggles
        //Calls a function and returns a value that says whether or not ability is a toggle
        private bool CheckToggleAbility(ChampionKit champion, IAbility ability)
        {
            if (ability.ToggleAbility(champion) == -1)
            {
                return false;
            }
            else
            {
                ability.ToggleAbility(champion);
                return true;
            }
        }


        //Activates toggle ability then updates champion stats incase ability gives stats
        private void ToggleAbility(ChampionKit champion, int index)
        {
            champion.Abilities[index].ToggleAbility(champion);
            UpdateChampTotalStats(champion);
            if (IsMainChampion(champion))
            {
                UpdateShownStats(champion, txtbox_Stats1, cmbBox_Level1, lstview_Icons1, txtBox_Champion1Name, abilitiesIcons1, abilitiesToggle1);
                CalculateSimpleChampDamage(champion, abilitiesDamage1);
                CalculateChampCompareDamage(MainChampion, SecondaryChampion, abilitiesDamageCompare1);
            }
            else
            {
                UpdateShownStats(champion, txtbox_Stats2, cmbBox_Level2, lstView_Icons2, txtbox_Champion2Name, abilitiesIcons2, abilitiesToggle2);
                CalculateSimpleChampDamage(champion, abilitiesDamage2);
                CalculateChampCompareDamage(SecondaryChampion, MainChampion, abilitiesDamageCompare2);
            }
        }

        //Check if a toggle ability from specific champion is activated
        private bool CheckIfActive(ChampionKit champion, IAbility ability)
        {
            bool active = false;
            active = Convert.ToBoolean(ability.ToggleAbility(champion));
            active = Convert.ToBoolean(ability.ToggleAbility(champion));

            return active;
        }


        private void btn_TogglePassive1_Click(object sender, EventArgs e)
        {
            ToggleAbility(MainChampion, 0);
            if (mainChampionActiveAbilities[0] == false)
            {
                mainChampionActiveAbilities[0] = true;
            }
            else
            {
                mainChampionActiveAbilities[0] = false;
            }
        }
        private void btn_ToggleQ1_Click(object sender, EventArgs e)
        {
            ToggleAbility(MainChampion, 1);
            if (secondaryChampionActiveAbilities[1] == false)
            {
                mainChampionActiveAbilities[1] = true;
            }
            else
            {
                mainChampionActiveAbilities[1] = false;
            }
        }
        private void btn_ToggleW1_Click(object sender, EventArgs e)
        {
            ToggleAbility(MainChampion, 2);
            if (mainChampionActiveAbilities[2] == false)
            {
                mainChampionActiveAbilities[2] = true;
            }
            else
            {
                mainChampionActiveAbilities[2] = false;
            }
        }
        private void btn_ToggleE1_Click(object sender, EventArgs e)
        {
            ToggleAbility(MainChampion, 3);
            if (mainChampionActiveAbilities[3] == false)
            {
                mainChampionActiveAbilities[3] = true;
            }
            else
            {
                mainChampionActiveAbilities[3] = false;
            }
        }
        private void btn_ToggleR1_Click(object sender, EventArgs e)
        {
            ToggleAbility(MainChampion, 4);
            if (mainChampionActiveAbilities[4] == false)
            {
                mainChampionActiveAbilities[4] = true;
            }
            else
            {
                mainChampionActiveAbilities[4] = false;
            }
        }

        private void btn_TogglePassive2_Click(object sender, EventArgs e)
        {
            ToggleAbility(SecondaryChampion, 0);
            if (secondaryChampionActiveAbilities[0] == false)
            {
                secondaryChampionActiveAbilities[0] = true;
            }
            else
            {
                secondaryChampionActiveAbilities[0] = false;
            }
        }
        private void btn_ToggleQ2_Click(object sender, EventArgs e)
        {
            ToggleAbility(SecondaryChampion, 1);
            if (secondaryChampionActiveAbilities[1] == false)
            {
                secondaryChampionActiveAbilities[1] = true;
            }
            else
            {
                secondaryChampionActiveAbilities[1] = false;
            }
        }
        private void btn_ToggleW2_Click(object sender, EventArgs e)
        {
            ToggleAbility(SecondaryChampion, 2);
            if (secondaryChampionActiveAbilities[2] == false)
            {
                secondaryChampionActiveAbilities[2] = true;
            }
            else
            {
                secondaryChampionActiveAbilities[2] = false;
            }
        }
        private void btn_ToggleE2_Click(object sender, EventArgs e)
        {
            ToggleAbility(SecondaryChampion, 3);
            if (secondaryChampionActiveAbilities[3] == false)
            {
                secondaryChampionActiveAbilities[3] = true;
            }
            else
            {
                secondaryChampionActiveAbilities[3] = false;
            }
        }
        private void btn_ToggleR2_Click(object sender, EventArgs e)
        {
            ToggleAbility(SecondaryChampion, 4);
            if (secondaryChampionActiveAbilities[4] == false)
            {
                secondaryChampionActiveAbilities[4] = true;
            }
            else
            {
                secondaryChampionActiveAbilities[4] = false;
            }
        }

        #endregion

        #region DamageCalculations

        //Function to Calculate damage that isn't mitigated, basically damage that would be shown on the tooltip of the ability
        //Also writes the calculated damage onto the proper fields
        private void CalculateSimpleChampDamage(ChampionKit champion, List<System.Windows.Forms.TextBox> abilitiesDamage)
        {
            double abilityDamage = 0.0;

            abilitiesDamage[0].Text = "Basic Attack damage: " + CalculateBasicAttack(champion).ToString("0.0");
            if (champion.CritChance > 0)
                abilitiesDamage[0].Text += "\r\n\r\nCrit Basic Attack Damage: " + CalculateCritBasicAttack(champion).ToString("0.0");

            DamageCalculationContext context = LoadChampionContext(champion);
            for (int i = 0; i < champion.Abilities.Count; i++)
            {
                abilityDamage = champion.Abilities[i].CalculateDamage(context);
                if (abilityDamage == -1)
                    continue;
                abilityDamage *= champion.Abilities[i].HitCount;
                abilityDamage *= 1 + (champion.BonusDamage + champion.AbilityBonusDamage);
                abilitiesDamage[i + 1].Text = champion.Abilities[i].Name + " Damage: " + abilityDamage.ToString("0.0");
                if (champion.Abilities[i].CanCrit && champion.CritChance > 0 )
                {
                    abilityDamage = champion.Abilities[i].CalculateCritDamage(context);
                    abilityDamage *= champion.Abilities[i].HitCount;
                    abilityDamage *= 1 + (champion.BonusDamage + champion.AbilityBonusDamage);
                    abilitiesDamage[i + 1].Text += "\r\n\r\n" + champion.Abilities[i].Name + " Crit Damage: " + abilityDamage.ToString("0.0") + "!!!";
                }


                if (champion.Abilities[i].CalculateSecondaryDamage(context) != -1)
                {
                    abilityDamage = champion.Abilities[i].CalculateSecondaryDamage(context);
                    abilityDamage *= 1 + (champion.BonusDamage + champion.AbilityBonusDamage);
                    switch (champion.Name)
                    {
                        case "Varus":
                            {
                                abilitiesDamage[i + 1].Text += "\r\n\r\n" + abilityDamage.ToString("0.00") + "% Max Health";
                            }
                            break;
                    }
                }
                if (champion.Abilities[i].CalculateTertiaryDamage(context) != -1)
                {
                    abilityDamage = champion.Abilities[i].CalculateTertiaryDamage(context);
                    abilityDamage *= 1 + (champion.BonusDamage + champion.AbilityBonusDamage);
                    switch (champion.Name)
                    {
                        case "Varus":
                            {
                                abilitiesDamage[i + 1].Text += "\r\n" + abilityDamage.ToString("0.00") + "% Missing Health";
                            }
                            break;

                    }
                }
            }

        }


        //Functions to Calculate damage that ins't mitigated like the one above
        //The ones used in the function above to calculate Basic Attack Damage
        #region Calculate Basic Attack Dmg
        private double CalculateBasicAttack(ChampionKit champion)
        {
            double damage = 0.0;
            damage = champion.TotalAttackDamage;
            damage = damage * (1 + champion.BonusDamage);
            return damage;
        }

        private double CalculateCritBasicAttack(ChampionKit champion)
        {
            double damage = 0.0;
            damage = champion.TotalAttackDamage;
            damage = damage * champion.CritDamage;
            damage = damage * (1 + champion.BonusDamage);
            return damage;
        }
        #endregion

        #region Calculate Mitigated Damage
        //Function to write on the proper fields the mitigated damage between the specified damage dealer and the specified damage taker
        //Uses a library to actually do the calculations
        private void CalculateChampCompareDamage(ChampionKit damageDealer, ChampionKit damageTaker, List<System.Windows.Forms.TextBox> abilitiesDamageCompare)
        {
            double basicAttackDamage = 0.0, basicAttackCritDamage = 0.0, extraDamage1 = 0.0, extraDamage2 = 0.0;
            double abilityDamage = 0.0;
            double enemyEffectiveArmor = 0.0, enemyEffectiveMagicResist = 0.0;

            extraDamage1 = Calculate.ExtraDamageFromArmorPen(damageDealer, damageTaker);
            extraDamage2 = Calculate.ExtraDamageFromMagicPen(damageDealer, damageTaker);
            basicAttackDamage = Calculate.MitigatedBasicAttack(damageDealer, damageTaker);
            basicAttackCritDamage = Calculate.MitigatedCritBasicAttack(damageDealer, damageTaker);


            abilitiesDamageCompare[0].Text = "Basic Attack damage: " + basicAttackDamage.ToString("0.0");
            if (damageDealer.CritChance > 0)
                abilitiesDamageCompare[0].Text += "\r\n\r\nCrit Basic Attack Damage: " + basicAttackCritDamage.ToString("0.0") + "!!!";
            DamageCalculationContext context = LoadChampionCompareContext(damageDealer, damageTaker);
            for (int i = 0; i < damageDealer.Abilities.Count; i++)
            {
                abilityDamage = Calculate.MitigateAbilityTotalDamage(damageDealer, damageDealer.Abilities[i], context, damageTaker);
                if (abilityDamage != -1)
                    abilitiesDamageCompare[i + 1].Text = damageDealer.Abilities[i].Name + " Damage: " + abilityDamage.ToString("0.0");

                if (damageDealer.Abilities[i].CanCrit && damageDealer.CritChance > 0)
                {
                    abilityDamage = Calculate.MitigateAbilityTotalCritDamage(damageDealer, damageDealer.Abilities[i], context, damageTaker);
                    if (abilityDamage != -1)
                        abilitiesDamageCompare[i + 1].Text += "\r\n\r\n" + damageDealer.Abilities[i].Name + " Crit Damage: " + abilityDamage.ToString("0.0") + "!!!";
                }



                if (damageDealer.Abilities[i].CalculateSecondaryDamage(context) != -1)
                {
                    abilityDamage = damageDealer.Abilities[i].CalculateSecondaryDamage(context);
                    switch (damageDealer.Name)
                    {
                        case "Varus":
                            {
                                abilityDamage = Calculate.MaxHealthAbilityDamage(damageDealer, damageDealer.Abilities[i], abilityDamage, context, damageTaker);
                                if (CheckIfActive(damageDealer, damageDealer.Abilities[i - 1]))
                                    abilityDamage *= 1.5;
                                abilitiesDamageCompare[i + 1].Text += "\r\n\r\n" + abilityDamage.ToString("0.00") + " Damage";
                            }
                            break;
                        case "Soraka":
                            {
                                abilityDamage = Calculate.HealingAmmount(abilityDamage, damageDealer.inventory, context);
                                abilitiesDamageCompare[i + 1].Text += "\r\n\r\n" + abilityDamage.ToString("0.00") + " Healing";
                            }
                            break;
                        default:
                            abilitiesDamageCompare[i + 1].Text += "\r\n\r\n" + abilityDamage.ToString("0.00") + " Damage";
                            break;
                    }
                }
                if (damageDealer.Abilities[i].CalculateTertiaryDamage(context) != -1)
                {
                    abilityDamage = Calculate.MitigateMagicDamage(damageDealer, damageDealer.Abilities[2].CalculateTertiaryDamage(context), damageTaker);
                    switch (damageDealer.Name)
                    {
                        case "Varus":
                            {
                                DamageCalculationContext tempContext = context;
                                double qDamage = 0.0;
                                qDamage = damageDealer.Abilities[1].CalculateDamage(context);
                                qDamage = Calculate.MitigateAbilityHitDamage(damageDealer, damageDealer.Abilities[1], context, damageTaker);
                                tempContext.EnemyCurrentHealth -= qDamage;
                                if(tempContext.EnemyCurrentHealth < 0)
                                    tempContext.EnemyCurrentHealth = 0;

                                abilityDamage = Calculate.MissingHealthAbilityDamage(damageDealer, damageDealer.Abilities[i], abilityDamage, tempContext, damageTaker);
                                abilitiesDamageCompare[i + 1].Text += "\r\n" + abilityDamage.ToString("0.00") + " Damage";
                            }
                            break;
                        default:
                            abilitiesDamageCompare[i + 1].Text += "\r\n\r\n" + abilityDamage.ToString("0.00") + " Damage";
                            break;

                    }
                }
            }
        }


        Random random = new Random();
        //Function to damage enemy champion with a Basic Attack
        //Uses the library to do the calculations
        //Simulates crit chance
        //Checks if there are any active toggles or on-hits that apply on auto attack
        private void ChampAttack(ChampionKit damageDealer, ChampionKit damageTaker)
        {
            double damageDealt = 0.0;
            double roll = random.NextDouble();
            bool crit = false, mainChamp = IsMainChampion(damageDealer);

            if (roll <= (damageDealer.CritChance / 100))
            {
                damageDealt = Calculate.MitigatedCritBasicAttack(damageDealer, damageTaker);
                crit = true;
            }
            else
            {
                damageDealt = Calculate.MitigatedBasicAttack(damageDealer, damageTaker);
            }

            DamageCalculationContext context = LoadChampionContext(damageDealer);
            switch (damageDealer.Name)
            {
                case "Xayah":
                    {
                        if (CheckIfActive(damageDealer, damageDealer.Abilities[2]))
                        {
                            ChampAttackFollowCritBased(damageDealer, damageDealt, damageDealer.Abilities[2], context, damageTaker, crit);
                        }
                        else
                        {
                            if (mainChamp)
                                LogBasicAttackDamage(damageDealer, damageDealt, richtxtbox_Logs1);
                            else
                                LogBasicAttackDamage(damageDealer, damageDealt, richtxtbox_Logs2);
                        }
                    }
                    break;
                case "Varus":
                    {
                        double extraOnHitDamage = damageDealer.Abilities[2].CalculateDamage(context);
                        extraOnHitDamage = Calculate.MitigateMagicDamage(damageDealer, extraOnHitDamage, damageTaker);
                        if (mainChamp)
                            LogBasicAttackDamageOnHit(damageDealer, damageDealt, extraOnHitDamage, richtxtbox_Logs1);
                        else
                            LogBasicAttackDamageOnHit(damageDealer, damageDealt, extraOnHitDamage, richtxtbox_Logs2);
                        damageDealt += extraOnHitDamage;
                    }
                    break;
                default:
                    {
                        if (mainChamp)
                            LogBasicAttackDamage(damageDealer, damageDealt, richtxtbox_Logs1);
                        else
                            LogBasicAttackDamage(damageDealer, damageDealt, richtxtbox_Logs2);
                    }
                    break;
            }
            damageTaker.CurrentHealth -= damageDealt;
            if (damageTaker.CurrentHealth < 0)
                damageTaker.CurrentHealth = 0;
            UpdateBothHealthBars();
            CalculateChampCompareDamage(MainChampion, SecondaryChampion, abilitiesDamageCompare1);
            CalculateChampCompareDamage(SecondaryChampion, MainChampion, abilitiesDamageCompare2);

        }




        //Function to damage enemy champion with a Ability
        //Uses the library to do the calculations
        //Library takes care of simulating the Crit Chance
        private void ChampAttack(ChampionKit damageDealer, IAbility ability, DamageCalculationContext context, ChampionKit damageTaker)
        {
            double damageDealt = 0.0, bonusDamage = 0.0, healAmmount = 0.0;
            switch(damageDealer.Name)
            {
                case "Varus":
                    {
                        switch(ability.Name)
                        {
                            case "Piercing Arrow":
                                {
                                    damageDealt = Calculate.AverageMitigatedAbilityDamage(damageDealer, ability, context, damageTaker);
                                    context.EnemyCurrentHealth -= damageDealt;
                                    if (context.EnemyCurrentHealth < 0)
                                        context.EnemyCurrentHealth = 0;

                                    if (CheckIfActive(damageDealer, damageDealer.Abilities[2]))
                                    {


                                        bonusDamage = damageDealer.Abilities[2].CalculateTertiaryDamage(context);
                                        bonusDamage = Calculate.MissingHealthAbilityDamage(damageDealer, damageDealer.Abilities[2], bonusDamage, context, damageTaker);
                                        bonusDamage = Calculate.MitigateMagicDamage(damageDealer, bonusDamage, damageTaker);

                                        if (IsMainChampion(damageDealer))
                                            LogMixedDamage(ability.Name, damageDealt, bonusDamage, "W", richtxtbox_Logs1);
                                        else
                                            LogMixedDamage(ability.Name, damageDealt, bonusDamage, "W", richtxtbox_Logs2);


                                        damageDealt += bonusDamage;
                                        
                                        context.EnemyCurrentHealth -= damageDealt;
                                        if(context.EnemyCurrentHealth < 0)
                                            context.EnemyCurrentHealth = 0;
                                    }
                                    else
                                    {
                                        if (IsMainChampion(damageDealer))
                                            LogDamageDealt(damageDealer, ability, damageDealt, richtxtbox_Logs1);
                                        else
                                            LogDamageDealt(damageDealer, ability, damageDealt, richtxtbox_Logs2);
                                    }
                                }
                                break;
                            case "Blighted Quiver":
                                {
                                    damageDealt = Calculate.MaxHealthAbilityDamage(damageDealer, ability, ability.CalculateSecondaryDamage(context), context, damageTaker);
                                    if (CheckIfActive(damageDealer, damageDealer.Abilities[1]))
                                        damageDealt *= 1.5;
                                    if (IsMainChampion(damageDealer))
                                        LogDamageDealt(damageDealer, ability, damageDealt, richtxtbox_Logs1);
                                    else
                                        LogDamageDealt(damageDealer, ability, damageDealt, richtxtbox_Logs2);
                                }
                                break;
                            default:
                                {
                                    damageDealt = Calculate.AverageMitigatedAbilityDamage(damageDealer, ability, context, damageTaker);
                                    if (IsMainChampion(damageDealer))
                                        LogDamageDealt(damageDealer, ability, damageDealt, richtxtbox_Logs1);
                                    else
                                        LogDamageDealt(damageDealer, ability, damageDealt, richtxtbox_Logs2);

                                }
                                break;
                        }
                    }
                    break;
                case "Soraka":
                    {
                        switch(ability.DamageType)
                        {
                            case "Heal":
                                {
                                    if(ability.Name == "Wish")
                                    {
                                    
                                        healAmmount = ability.CalculateDamage(context);
                                        if (damageDealer.CurrentHealth < (damageDealer.MaxHealth * 0.4))
                                            healAmmount *= 1.5;
                                        healAmmount = Calculate.HealingAmmount(healAmmount, damageDealer.inventory, context);
                                    }
                                    else
                                    {
                                        healAmmount = ability.CalculateDamage(context);
                                        if (CheckIfActive(damageDealer, damageDealer.Abilities[1]))
                                            healAmmount += damageDealer.Abilities[1].CalculateSecondaryDamage(context);
                                        healAmmount = Calculate.HealingAmmount(healAmmount, damageDealer.inventory, context);
                                    }

                                    damageDealer.CurrentHealth += healAmmount;
                                    if (damageDealer.CurrentHealth > damageDealer.MaxHealth)
                                        damageDealer.CurrentHealth = damageDealer.MaxHealth;

                                    if (IsMainChampion(damageDealer))
                                        LogHealAmmount(damageDealer, ability, healAmmount, richtxtbox_Logs1);
                                    else
                                        LogHealAmmount(damageDealer, ability, healAmmount, richtxtbox_Logs2);

                                    UpdateBothHealthBars();
                                    CalculateChampCompareDamage(MainChampion, SecondaryChampion, abilitiesDamageCompare1);
                                    CalculateChampCompareDamage(SecondaryChampion, MainChampion, abilitiesDamageCompare2);
                                    return;
                                }
                            default:
                                {
                                    damageDealt = Calculate.AverageMitigatedAbilityDamage(damageDealer, ability, context, damageTaker);

                                    if (IsMainChampion(damageDealer))
                                        LogDamageDealt(damageDealer, ability, damageDealt, richtxtbox_Logs1);
                                    else
                                        LogDamageDealt(damageDealer, ability, damageDealt, richtxtbox_Logs2);
                                }
                                break;
                        }
                    }break;
                default:
                    {
                        damageDealt = Calculate.AverageMitigatedAbilityDamage(damageDealer, ability, context, damageTaker);

                        if (IsMainChampion(damageDealer))
                            LogDamageDealt(damageDealer, ability, damageDealt, richtxtbox_Logs1);
                        else
                            LogDamageDealt(damageDealer, ability, damageDealt, richtxtbox_Logs2);
                    }
                    break;
            }

            damageDealt += bonusDamage;
            damageTaker.CurrentHealth -= damageDealt;
            if (damageTaker.CurrentHealth < 0)
                damageTaker.CurrentHealth = 0;

            UpdateBothHealthBars();
            CalculateChampCompareDamage(MainChampion, SecondaryChampion, abilitiesDamageCompare1);
            CalculateChampCompareDamage(SecondaryChampion, MainChampion, abilitiesDamageCompare2);
        }


        //Function to damage enemy champion with toggled abilities applied from the basic attack
        //Crits if basic attack that procced it also critted
        //Uses the library to do the calculations
        private void ChampAttackFollowCritBased(ChampionKit damageDealer, double basicAttackDamage, IAbility ability, DamageCalculationContext context, ChampionKit damageTaker, bool crit)
        {
            double damageDealt = 0.0;
            if (crit)
            {
                damageDealt = Calculate.MitigateAbilityTotalCritDamage(damageDealer, ability, context, damageTaker);
            }
            else
            {
                damageDealt = Calculate.MitigateAbilityTotalDamage(damageDealer, ability, context, damageTaker);
            }

            if (IsMainChampion(damageDealer))
                LogToggleAbilityDamage(damageDealer, basicAttackDamage, ability, damageDealt, richtxtbox_Logs1);
            else
                LogToggleAbilityDamage(damageDealer, basicAttackDamage, ability, damageDealt, richtxtbox_Logs2);

            damageTaker.CurrentHealth -= damageDealt;
            if (damageTaker.CurrentHealth < 0)
                damageTaker.CurrentHealth = 0;

            UpdateBothHealthBars();
            CalculateChampCompareDamage(damageDealer, damageTaker, abilitiesDamageCompare1);
            CalculateChampCompareDamage(damageDealer, damageTaker, abilitiesDamageCompare2);
        }
        #endregion

        #endregion DamageCalculations

        #region AbilityCasts
        private void lstview_AA1_DoubleClick(object sender, MouseEventArgs e)
        {
            ChampAttack(MainChampion, SecondaryChampion);
        }

        private void lstview_Passive1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChampAttack(MainChampion, MainChampion.Abilities[0], LoadChampionCompareContext(MainChampion, SecondaryChampion), SecondaryChampion);
        }

        private void lstview_Q1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChampAttack(MainChampion, MainChampion.Abilities[1], LoadChampionCompareContext(MainChampion, SecondaryChampion), SecondaryChampion);
        }

        private void lstview_W1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChampAttack(MainChampion, MainChampion.Abilities[2], LoadChampionCompareContext(MainChampion, SecondaryChampion), SecondaryChampion);
        }

        private void lstview_E1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChampAttack(MainChampion, MainChampion.Abilities[3], LoadChampionCompareContext(MainChampion, SecondaryChampion), SecondaryChampion);
        }

        private void lstview_R1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChampAttack(MainChampion, MainChampion.Abilities[4], LoadChampionCompareContext(MainChampion, SecondaryChampion), SecondaryChampion);
        }

        private void lstview_AA2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChampAttack(SecondaryChampion, MainChampion);
        }

        private void lstview_Passive2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChampAttack(SecondaryChampion, SecondaryChampion.Abilities[0], LoadChampionCompareContext(SecondaryChampion, MainChampion), MainChampion);
        }

        private void lstview_Q2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChampAttack(SecondaryChampion, SecondaryChampion.Abilities[1], LoadChampionCompareContext(SecondaryChampion, MainChampion), MainChampion);
        }

        private void lstview_W2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChampAttack(SecondaryChampion, SecondaryChampion.Abilities[2], LoadChampionCompareContext(SecondaryChampion, MainChampion), MainChampion);
        }

        private void lstview_E2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChampAttack(SecondaryChampion, SecondaryChampion.Abilities[3], LoadChampionCompareContext(SecondaryChampion, MainChampion), MainChampion);
        }

        private void lstview_R2_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            ChampAttack(SecondaryChampion, SecondaryChampion.Abilities[4], LoadChampionCompareContext(SecondaryChampion, MainChampion), MainChampion);
        }
        #endregion

        #region Heals
        //Heals main champion then updates it's healh values on the ui
        private void btn_HealMainChampion_Click(object sender, EventArgs e)
        {
            MainChampion.CurrentHealth = MainChampion.MaxHealth;
            UpdateHealthbar(MainChampion);
            
            UpdateShownStats(MainChampion, txtbox_Stats1, cmbBox_Level1, lstview_Icons1, txtBox_Champion1Name, abilitiesIcons1, abilitiesToggle1);
            CalculateChampCompareDamage(MainChampion, SecondaryChampion, abilitiesDamageCompare1);
            CalculateChampCompareDamage(SecondaryChampion, MainChampion, abilitiesDamageCompare2);
        }


        //Heals secondary champion then updates it's healh values on the ui
        private void btn_HealSecondaryChampion_Click(object sender, EventArgs e)
        {
            SecondaryChampion.CurrentHealth = SecondaryChampion.MaxHealth;
            UpdateHealthbar(SecondaryChampion);
           
            UpdateShownStats(SecondaryChampion, txtbox_Stats2, cmbBox_Level2, lstView_Icons2, txtbox_Champion2Name, abilitiesIcons2, abilitiesToggle2);
            CalculateChampCompareDamage(MainChampion, SecondaryChampion, abilitiesDamageCompare1);
            CalculateChampCompareDamage(SecondaryChampion, MainChampion, abilitiesDamageCompare2);
        }


        //Heals both champions then updates their healh values on the ui
        private void btn_HealAll_Click(object sender, EventArgs e)
        {
            MainChampion.CurrentHealth = MainChampion.MaxHealth;
            SecondaryChampion.CurrentHealth = SecondaryChampion.MaxHealth;
            UpdateBothHealthBars();
            UpdateShownStats(MainChampion, txtbox_Stats1, cmbBox_Level1, lstview_Icons1, txtBox_Champion1Name, abilitiesIcons1, abilitiesToggle1);
            UpdateShownStats(SecondaryChampion, txtbox_Stats2, cmbBox_Level2, lstView_Icons2, txtbox_Champion2Name, abilitiesIcons2, abilitiesToggle2);
            CalculateChampCompareDamage(MainChampion, SecondaryChampion, abilitiesDamageCompare1);
            CalculateChampCompareDamage(SecondaryChampion, MainChampion, abilitiesDamageCompare2);
        }
        #endregion

        private void richtxtbox_Logs1_SelectionChanged(object sender, EventArgs e)
        {
            /*
            richtxtbox_Logs1.Select(0, 0);
            richtxtbox_Logs2.Select(0, 0);*/
        }
    }
}
