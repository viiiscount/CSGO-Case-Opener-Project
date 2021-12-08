using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Project
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            //redirect mouse clicks and cursor hovers to custom event handlers.
            CS__GO_Weapon_Case.Click += PicBoxClick;
            eSports_2013_Case.Click += PicBoxClick;
            Operation_Bravo_Case.Click += PicBoxClick;
            CS__GO_Weapon_Case_2.Click += PicBoxClick;
            eSports_2013_Winter_Case.Click += PicBoxClick;
            Winter_Offensive_Weapon_Case.Click += PicBoxClick;
            CS__GO_Weapon_Case_3.Click += PicBoxClick;
            Operation_Phoenix_Weapon_Case.Click += PicBoxClick;
            Huntsman_Weapon_Case.Click += PicBoxClick;
            Operation_Breakout_Weapon_Case.Click += PicBoxClick;
            eSports_2014_Summer_Case.Click += PicBoxClick;
            Operation_Vanguard_Weapon_Case.Click += PicBoxClick;
            Chroma_Case.Click += PicBoxClick;
            Chroma_2_Case.Click += PicBoxClick;
            Falchion_Case.Click += PicBoxClick;
            Shadow_Case.Click += PicBoxClick;
            Revolver_Case.Click += PicBoxClick;
            Operation_Wildfire_Case.Click += PicBoxClick;
            Chroma_3_Case.Click += PicBoxClick;
            Gamma_Case.Click += PicBoxClick;
            Gamma_2_Case.Click += PicBoxClick;
            Glove_Case.Click += PicBoxClick;
            Spectrum_Case.Click += PicBoxClick;
            Operation_Hydra_Case.Click += PicBoxClick;
            Spectrum_2_Case.Click += PicBoxClick;
            Clutch_Case.Click += PicBoxClick;
            Horizon_Case.Click += PicBoxClick;
            Danger_Zone_Case.Click += PicBoxClick;
            Prisma_Case.Click += PicBoxClick;
            CS20_Case.Click += PicBoxClick;
            Shattered_Web_Case.Click += PicBoxClick;
            Prisma_2_Case.Click += PicBoxClick;
            Fracture_Case.Click += PicBoxClick;
            Operation_Broken_Fang_Case.Click += PicBoxClick;
            Snakebite_Case.Click += PicBoxClick;
            Operation_Riptide_Case.Click += PicBoxClick;

            CS__GO_Weapon_Case.MouseHover += PicBoxHover;
            eSports_2013_Case.MouseHover += PicBoxHover;
            Operation_Bravo_Case.MouseHover += PicBoxHover;
            CS__GO_Weapon_Case_2.MouseHover += PicBoxHover;
            eSports_2013_Winter_Case.MouseHover += PicBoxHover;
            Winter_Offensive_Weapon_Case.MouseHover += PicBoxHover;
            CS__GO_Weapon_Case_3.MouseHover += PicBoxHover;
            Operation_Phoenix_Weapon_Case.MouseHover += PicBoxHover;
            Huntsman_Weapon_Case.MouseHover += PicBoxHover;
            Operation_Breakout_Weapon_Case.MouseHover += PicBoxHover;
            eSports_2014_Summer_Case.MouseHover += PicBoxHover;
            Operation_Vanguard_Weapon_Case.MouseHover += PicBoxHover;
            Chroma_Case.MouseHover += PicBoxHover;
            Chroma_2_Case.MouseHover += PicBoxHover;
            Falchion_Case.MouseHover += PicBoxHover;
            Shadow_Case.MouseHover += PicBoxHover;
            Revolver_Case.MouseHover += PicBoxHover;
            Operation_Wildfire_Case.MouseHover += PicBoxHover;
            Chroma_3_Case.MouseHover += PicBoxHover;
            Gamma_Case.MouseHover += PicBoxHover;
            Gamma_2_Case.MouseHover += PicBoxHover;
            Glove_Case.MouseHover += PicBoxHover;
            Spectrum_Case.MouseHover += PicBoxHover;
            Operation_Hydra_Case.MouseHover += PicBoxHover;
            Spectrum_2_Case.MouseHover += PicBoxHover;
            Clutch_Case.MouseHover += PicBoxHover;
            Horizon_Case.MouseHover += PicBoxHover;
            Danger_Zone_Case.MouseHover += PicBoxHover;
            Prisma_Case.MouseHover += PicBoxHover;
            CS20_Case.MouseHover += PicBoxHover;
            Shattered_Web_Case.MouseHover += PicBoxHover;
            Prisma_2_Case.MouseHover += PicBoxHover;
            Fracture_Case.MouseHover += PicBoxHover;
            Operation_Broken_Fang_Case.MouseHover += PicBoxHover;
            Snakebite_Case.MouseHover += PicBoxHover;
            Operation_Riptide_Case.MouseHover += PicBoxHover;
        }

        private void PicBoxClick(object sender, EventArgs e)
        {
            //get name of case from the picture box that was clicked, convert it to string
            PictureBox picBox = sender as PictureBox;
            string nameOfCase = picBox.Name;

            //send name to frmOpener
            frmOpener frmOpener = new frmOpener(nameOfCase);
            frmOpener.Show();
        }

        private void PicBoxHover(object sender, EventArgs e)
        {
            //get name of case from the picture box, convert it to string
            PictureBox picBox = sender as PictureBox;
            string nameOfCase = picBox.Name;

            //open the correct .json file, deserialize the file and place data into a datatable
            DataSet dataSet = Deserialize(nameOfCase);
            DataTable skinTable = dataSet.Tables[nameOfCase];

            //replace _ with spaces, then display the case name in the tooltip
            string realName = nameOfCase.Replace("__", ":").Replace('_', ' ');
            ttPopup.ToolTipTitle = realName + " Contains: ";

            //get the names and rarities and add them to lists using LINQ
            List<string> Names = skinTable.Rows.OfType<DataRow>().Select(dr => (string)dr["Name"]).ToList();
            List<string> Rarity = skinTable.Rows.OfType<DataRow>().Select(dr => (string)dr["Rarity"]).ToList();

            //zip the lists together then display them in the tooltip
            var Skins = Rarity.Zip(Names, (first, second) => first + " - " + second);
            ttPopup.SetToolTip(picBox, String.Join("\n", Skins));
        }

        public DataSet Deserialize(string nameOfCase)
        {
            DataSet dataSet = new DataSet();
            try
            {
                //deserialize .json file
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"json files\" + nameOfCase + ".json");
                string json = File.ReadAllText(fileName);
                dataSet = JsonConvert.DeserializeObject<DataSet>(json);
            }
            catch
            {
                string message = "Cannot Load JSON Files. \nThe program will now close.";
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
            return dataSet;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            //open search
            FrmSearch frmSearch = new FrmSearch();
            frmSearch.Show();
        }
    }
}
