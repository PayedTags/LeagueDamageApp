namespace wip_LeagueThing
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ListViewItem listViewItem1 = new ListViewItem("Xayah", 0);
            ListViewItem listViewItem2 = new ListViewItem("Samira", 1);
            ListViewItem listViewItem3 = new ListViewItem("Varus", 2);
            ListViewItem listViewItem4 = new ListViewItem("Soraka", 3);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lstview_ChampionSelect = new ListView();
            imgList_ChampionIcons = new ImageList(components);
            btn_Damage = new Button();
            btn_HealShield = new Button();
            grpbox_ChampionSelect = new GroupBox();
            btn_Select = new Button();
            grpbox_ChampionSelect.SuspendLayout();
            SuspendLayout();
            // 
            // lstview_ChampionSelect
            // 
            lstview_ChampionSelect.Items.AddRange(new ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4 });
            lstview_ChampionSelect.LargeImageList = imgList_ChampionIcons;
            lstview_ChampionSelect.Location = new Point(16, 22);
            lstview_ChampionSelect.Name = "lstview_ChampionSelect";
            lstview_ChampionSelect.Size = new Size(1218, 600);
            lstview_ChampionSelect.TabIndex = 0;
            lstview_ChampionSelect.UseCompatibleStateImageBehavior = false;
            lstview_ChampionSelect.SelectedIndexChanged += lstview_ChampionSelect_SelectedIndexChanged;
            lstview_ChampionSelect.DoubleClick += btn_Select_Click;
            // 
            // imgList_ChampionIcons
            // 
            imgList_ChampionIcons.ColorDepth = ColorDepth.Depth32Bit;
            imgList_ChampionIcons.ImageStream = (ImageListStreamer)resources.GetObject("imgList_ChampionIcons.ImageStream");
            imgList_ChampionIcons.TransparentColor = Color.Transparent;
            imgList_ChampionIcons.Images.SetKeyName(0, "XayahIcon.png");
            imgList_ChampionIcons.Images.SetKeyName(1, "SamiraIcon.png");
            imgList_ChampionIcons.Images.SetKeyName(2, "110.png");
            imgList_ChampionIcons.Images.SetKeyName(3, "SorakaÎcon.png");
            // 
            // btn_Damage
            // 
            btn_Damage.Location = new Point(12, 12);
            btn_Damage.Name = "btn_Damage";
            btn_Damage.Size = new Size(103, 23);
            btn_Damage.TabIndex = 1;
            btn_Damage.Text = "Damage";
            btn_Damage.UseVisualStyleBackColor = true;
            btn_Damage.Click += btn_Damage_Click;
            // 
            // btn_HealShield
            // 
            btn_HealShield.Location = new Point(121, 12);
            btn_HealShield.Name = "btn_HealShield";
            btn_HealShield.Size = new Size(103, 23);
            btn_HealShield.TabIndex = 2;
            btn_HealShield.Text = "Heal/Shield";
            btn_HealShield.UseVisualStyleBackColor = true;
            // 
            // grpbox_ChampionSelect
            // 
            grpbox_ChampionSelect.Controls.Add(lstview_ChampionSelect);
            grpbox_ChampionSelect.Location = new Point(12, 41);
            grpbox_ChampionSelect.Name = "grpbox_ChampionSelect";
            grpbox_ChampionSelect.Size = new Size(1240, 628);
            grpbox_ChampionSelect.TabIndex = 3;
            grpbox_ChampionSelect.TabStop = false;
            grpbox_ChampionSelect.Text = "Champion Select";
            // 
            // btn_Select
            // 
            btn_Select.Location = new Point(604, 12);
            btn_Select.Name = "btn_Select";
            btn_Select.Size = new Size(103, 23);
            btn_Select.TabIndex = 4;
            btn_Select.Text = "Select";
            btn_Select.UseVisualStyleBackColor = true;
            btn_Select.Click += btn_Select_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(btn_Select);
            Controls.Add(grpbox_ChampionSelect);
            Controls.Add(btn_HealShield);
            Controls.Add(btn_Damage);
            Name = "Form1";
            Text = "Form1";
            grpbox_ChampionSelect.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListView lstview_ChampionSelect;
        private Button btn_Damage;
        private Button btn_HealShield;
        private ImageList imgList_ChampionIcons;
        private GroupBox grpbox_ChampionSelect;
        private Button btn_Select;
    }
}
