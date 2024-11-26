namespace wip_LeagueThing
{
    partial class Shop
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Shop));
            imgList_Items = new ImageList(components);
            lstview_Shop = new ListView();
            SuspendLayout();
            // 
            // imgList_Items
            // 
            imgList_Items.ColorDepth = ColorDepth.Depth32Bit;
            imgList_Items.ImageStream = (ImageListStreamer)resources.GetObject("imgList_Items.ImageStream");
            imgList_Items.TransparentColor = Color.Transparent;
            imgList_Items.Images.SetKeyName(0, "6676_marksman_t3_thecollector.png");
            imgList_Items.Images.SetKeyName(1, "3031_marksman_t3_infinityedge.png");
            imgList_Items.Images.SetKeyName(2, "3036_marksman_t3_dominikregards.png");
            imgList_Items.Images.SetKeyName(3, "3089_mage_t3_deathcap.png");
            imgList_Items.Images.SetKeyName(4, "6617_enchanter_t4_moonstonerenewer.png");
            imgList_Items.Images.SetKeyName(5, "3744_enchanter_t3_staffofflowingwater.png");
            imgList_Items.Images.SetKeyName(6, "6621_dawncore.png");
            // 
            // lstview_Shop
            // 
            lstview_Shop.Alignment = ListViewAlignment.SnapToGrid;
            lstview_Shop.AutoArrange = false;
            lstview_Shop.LargeImageList = imgList_Items;
            lstview_Shop.Location = new Point(39, 28);
            lstview_Shop.MultiSelect = false;
            lstview_Shop.Name = "lstview_Shop";
            lstview_Shop.Size = new Size(749, 410);
            lstview_Shop.SmallImageList = imgList_Items;
            lstview_Shop.TabIndex = 0;
            lstview_Shop.UseCompatibleStateImageBehavior = false;
            lstview_Shop.DoubleClick += lstview_Shop_DoubleClick;
            // 
            // Shop
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lstview_Shop);
            Name = "Shop";
            Text = "Shop";
            Load += Shop_Load;
            ResumeLayout(false);
        }

        #endregion

        private ImageList imgList_Items;
        private ListView lstview_Shop;
    }
}