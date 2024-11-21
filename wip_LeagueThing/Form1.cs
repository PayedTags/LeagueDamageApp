namespace wip_LeagueThing
{
    public partial class Form1 : Form
    {
        string[] selectedChampions = new string[2];
        public Form1()
        {
            InitializeComponent();
        }
        private void btn_Damage_Click(object sender, EventArgs e)
        {
            string workingDirectory = Environment.CurrentDirectory;
            MessageBox.Show(Directory.GetParent(workingDirectory).Parent.Parent.FullName + @"\Icons\ChampionIcons");
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            
            Form championSelected;
            switch (lstview_ChampionSelect.SelectedItems.Count)
            {
                case 0:
                    {
                        MessageBox.Show("Need to select atleast one champion");
                        break;
                    }
                case 1:
                    {
                        selectedChampions[1] = selectedChampions[0];
                        championSelected = new Comparison(selectedChampions);
                        championSelected.Show();
                        break;
                    }
                case 2:
                    {
                        championSelected = new Comparison(selectedChampions);
                        championSelected.Show();
                        break;
                    };
            }

        }

        private void lstview_ChampionSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstview_ChampionSelect.SelectedItems.Count == 1)
                selectedChampions[0] = lstview_ChampionSelect.SelectedItems[0].Text;
            else if(lstview_ChampionSelect.SelectedItems.Count == 2)
            {
                if (lstview_ChampionSelect.SelectedItems[1].Text == selectedChampions[0])
                    selectedChampions[1] = lstview_ChampionSelect.SelectedItems[0].Text;
                else if (lstview_ChampionSelect.SelectedItems[0].Text == selectedChampions[0])
                    selectedChampions[1] = lstview_ChampionSelect.SelectedItems[1].Text;
            }
        }
    }
}
