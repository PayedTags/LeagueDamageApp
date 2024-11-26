using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wip_LeagueThing
{
    public partial class Shop : Form
    {
        List<ShopItems> shopItemsList = new List<ShopItems>();
        public int ItemBought {  get; set; }

        public Shop(List<ShopItems> shopItems)
        {
            shopItemsList = shopItems;
            InitializeComponent();
        }


        private void Shop_Load(object sender, EventArgs e)
        {
            foreach (ShopItems item in shopItemsList)
            {
                ListViewItem lstviewItem = new ListViewItem();
                lstviewItem.ImageIndex = item.ImageIndex;
                lstview_Shop.Items.Add(lstviewItem);
            }
        }

        private void lstview_Shop_DoubleClick(object sender, EventArgs e)
        {
            if (lstview_Shop.SelectedItems.Count > 0)
            {
                this.ItemBought = lstview_Shop.SelectedIndices[0];
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
