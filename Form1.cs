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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            //redirect mouse clicks and cursor hovers to custom event handlers.
            Weapon_Case_1.Click += picBoxClick;
            eSports_2013_Case.Click += picBoxClick;
            Operation_Bravo_Case.Click += picBoxClick;
            Weapon_Case_2.Click += picBoxClick;
            eSports_2013_Winter_Case.Click += picBoxClick;
            Winter_Offensive_Case.Click += picBoxClick;
            Weapon_Case_3.Click += picBoxClick;
            Operation_Phoenix_Case.Click += picBoxClick;
            Huntsman_Case.Click += picBoxClick;
            Operation_Breakout_Case.Click += picBoxClick;
            eSports_2014_Summer_Case.Click += picBoxClick;
            Operation_Vanguard_Case.Click += picBoxClick;
            Chroma_Case.Click += picBoxClick;
            Chroma_2_Case.Click += picBoxClick;
            Falchion_Case.Click += picBoxClick;
            Shadow_Case.Click += picBoxClick;
            Revolver_Case.Click += picBoxClick;
            Operation_Wildfire_Case.Click += picBoxClick;
            Chroma_3_Case.Click += picBoxClick;
            Gamma_Case.Click += picBoxClick;
            Gamma_2_Case.Click += picBoxClick;
            Glove_Case.Click += picBoxClick;
            Spectrum_Case.Click += picBoxClick;
            Operation_Hydra_Case.Click += picBoxClick;
            Spectrum_2_Case.Click += picBoxClick;
            Clutch_Case.Click += picBoxClick;
            Horizon_Case.Click += picBoxClick;
            Danger_Zone_Case.Click += picBoxClick;
            Prisma_Case.Click += picBoxClick;
            CS20_Case.Click += picBoxClick;
            Shattered_Web_Case.Click += picBoxClick;
            Prisma_2_Case.Click += picBoxClick;
            Fracture_Case.Click += picBoxClick;
            Operation_Broken_Fang_Case.Click += picBoxClick;
            Snakebite_Case.Click += picBoxClick;
            Operation_Riptide_Case.Click += picBoxClick;

            Weapon_Case_1.MouseHover += picBoxHover;
            eSports_2013_Case.MouseHover += picBoxHover;
            Operation_Bravo_Case.MouseHover += picBoxHover;
            Weapon_Case_2.MouseHover += picBoxHover;
            eSports_2013_Winter_Case.MouseHover += picBoxHover;
            Winter_Offensive_Case.MouseHover += picBoxHover;
            Weapon_Case_3.MouseHover += picBoxHover;
            Operation_Phoenix_Case.MouseHover += picBoxHover;
            Huntsman_Case.MouseHover += picBoxHover;
            Operation_Breakout_Case.MouseHover += picBoxHover;
            eSports_2014_Summer_Case.MouseHover += picBoxHover;
            Operation_Vanguard_Case.MouseHover += picBoxHover;
            Chroma_Case.MouseHover += picBoxHover;
            Chroma_2_Case.MouseHover += picBoxHover;
            Falchion_Case.MouseHover += picBoxHover;
            Shadow_Case.MouseHover += picBoxHover;
            Revolver_Case.MouseHover += picBoxHover;
            Operation_Wildfire_Case.MouseHover += picBoxHover;
            Chroma_3_Case.MouseHover += picBoxHover;
            Gamma_Case.MouseHover += picBoxHover;
            Gamma_2_Case.MouseHover += picBoxHover;
            Glove_Case.MouseHover += picBoxHover;
            Spectrum_Case.MouseHover += picBoxHover;
            Operation_Hydra_Case.MouseHover += picBoxHover;
            Spectrum_2_Case.MouseHover += picBoxHover;
            Clutch_Case.MouseHover += picBoxHover;
            Horizon_Case.MouseHover += picBoxHover;
            Danger_Zone_Case.MouseHover += picBoxHover;
            Prisma_Case.MouseHover += picBoxHover;
            CS20_Case.MouseHover += picBoxHover;
            Shattered_Web_Case.MouseHover += picBoxHover;
            Prisma_2_Case.MouseHover += picBoxHover;
            Fracture_Case.MouseHover += picBoxHover;
            Operation_Broken_Fang_Case.MouseHover += picBoxHover;
            Snakebite_Case.MouseHover += picBoxHover;
            Operation_Riptide_Case.MouseHover += picBoxHover;
        }

        private void picBoxClick(object sender, EventArgs e)
        {
            //get name of case from the picture box that was clicked, convert it to string
            PictureBox picBox = sender as PictureBox;
            string nameOfCase = picBox.Name;

            //send name to frmOpener
            frmOpener frmOpener = new frmOpener(nameOfCase);
            frmOpener.Show();
        }

        private void picBoxHover(object sender, EventArgs e)
        {
            //get name of case from the picture box, convert it to string
            PictureBox picBox = sender as PictureBox;
            string nameOfCase = picBox.Name;

            //open the correct .json file, deserialize the file and place data into a datatable
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"json files\" + nameOfCase + ".json");
            string json = File.ReadAllText(fileName);
            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(json);
            DataTable skinTable = dataSet.Tables[nameOfCase];

            //replace _ with spaces, then display the case name in the tooltip
            string realName = nameOfCase.Replace('_', ' ');
            ttPopup.ToolTipTitle = realName + " Contains: ";

            //get the names and rarities and add them to lists using LINQ
            List<string> Names = skinTable.Rows.OfType<DataRow>().Select(dr => (string)dr["Name"]).ToList();
            List<string> Rarity = skinTable.Rows.OfType<DataRow>().Select(dr => (string)dr["Rarity"]).ToList();

            //zip the lists together then display them in the tooltip
            var Skins = Rarity.Zip(Names, (first, second) => first + " - " + second);
            ttPopup.SetToolTip(picBox, String.Join("\n", Skins));
        }
    }
}
