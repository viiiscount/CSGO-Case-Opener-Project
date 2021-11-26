using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class frmOpener : Form
    {

        public frmOpener(string nameOfCase)
        {
            InitializeComponent();

            //set the name of the form to name of the case
            string realName = nameOfCase.Replace('_', ' ');
            this.Text = realName;

            //open the correct .json file, deserialize the file and place data into a datatable
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"json files\" + nameOfCase + ".json");
            DataSet dataSet = Deserialize(fileName);
            DataTable skinTable = dataSet.Tables[nameOfCase];

            //create lists of the labels and pictureboxes that will be used
            var labels = new List<Label> { lblSkin1, lblSkin2, lblSkin3, lblSkin4, lblSkin5, lblSkin6, lblSkin7, lblSkin8,
                lblSkin9, lblSkin10, lblSkin11, lblSkin12, lblSkin13, lblSkin14, lblSkin15, lblSkin16, lblSkin17, lblSkin18 };
            var pictures = new List<PictureBox> { picSkin1, picSkin2, picSkin3, picSkin4, picSkin5, picSkin6, picSkin7, picSkin8,
                picSkin9, picSkin10, picSkin11, picSkin12, picSkin13, picSkin14, picSkin15, picSkin16, picSkin17, picSkin18};

            //foreach loop to print out the names of the skins and their icons
            int i = 0;
            foreach (DataRow row in skinTable.Rows)
            {
                //set text colour
                if (row["Rarity"].ToString() == "Mil-Spec")
                {
                    labels[i].ForeColor = System.Drawing.Color.DodgerBlue;
                    pictures[i].BackColor = System.Drawing.Color.DodgerBlue;
                }
                else if (row["Rarity"].ToString() == "Restricted")
                {
                    labels[i].ForeColor = System.Drawing.Color.MediumOrchid;
                    pictures[i].BackColor = System.Drawing.Color.MediumOrchid;
                }
                else if (row["Rarity"].ToString() == "Classified")
                {
                    labels[i].ForeColor = System.Drawing.Color.HotPink;
                    pictures[i].BackColor = System.Drawing.Color.HotPink;

                }
                else if (row["Rarity"].ToString() == "Covert")
                {
                    labels[i].ForeColor = System.Drawing.Color.Tomato;
                    pictures[i].BackColor = System.Drawing.Color.Tomato;
                }

                //get name of skin from datatable then add it to the label
                string skinName = row["Name"].ToString();
                labels[i].Show();
                labels[i].Text = skinName;

                //get picture filepath then resize and add it to the picturebox
                string picName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\Directory\"
                + skinName.Replace('|', '-') + ".png").Replace("Directory", nameOfCase);
                pictures[i].Show();
                pictures[i].Image = Image.FromFile(picName);
                pictures[i].SizeMode = PictureBoxSizeMode.StretchImage;

                //increment i so all items done
                i++;
            }

            //print the "Rare Special Item" menu icon
            labels[i].Show();
            labels[i].ForeColor = System.Drawing.Color.Goldenrod;
            labels[i].Text = "Rare Special Item";

            pictures[i].Show();
            pictures[i].BackColor = System.Drawing.Color.Goldenrod;
            pictures[i].Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\knife.png"));
            pictures[i].SizeMode = PictureBoxSizeMode.StretchImage;

            //route roll button to custom handler
            //this is so i can send "nameOfCase" and "skinTable" to RNG()
            btnRoll.Click += (sender, EventArgs) => { btnRoll_Click(sender, EventArgs, skinTable, nameOfCase); };
            timerAutoRoll.Tick += (sender, EventArgs) => { timer_Tick(sender, EventArgs, skinTable, nameOfCase); };
        }

        private async void btnRoll_Click(object sender, EventArgs e, DataTable skinTable, string nameOfCase)
        {
            //disables button so you cant spam the program
            //this to stop overlapping animations and from the user interfering with the auto-roll
            btnRoll.Enabled = false;

            //checks if auto-roll/animation skip is enabled
            if (chkAutoRoll.Checked == true)
            {
                timerAutoRoll.Start();
            }
            else if (chkAutoRoll.Checked == false && chkAnimationSkip.Checked == false)
            {
                Animation();
                await Task.Delay(3000);
                RNG(skinTable, nameOfCase);
                btnRoll.Enabled = true;
            }
            else if (chkAutoRoll.Checked == false && chkAnimationSkip.Checked == true)
            {
                RNG(skinTable, nameOfCase);
                await Task.Delay(500);
                btnRoll.Enabled = true;
            }
        }

        private async void timer_Tick(object sender, EventArgs e, DataTable skinTable, string nameOfCase)
        {
            //auto-roll feature
            if (chkAnimationSkip.Checked == true && chkAutoRoll.Checked == true)
            {
                timerAutoRoll.Interval = 1000;
                await Task.Delay(1000);
                RNG(skinTable, nameOfCase);
            }
            else if (chkAutoRoll.Checked == true)
            {
                timerAutoRoll.Interval = 4000;
                Animation();
                await Task.Delay(3000);
                RNG(skinTable, nameOfCase);
            }
            else
            {
                timerAutoRoll.Stop();
                btnRoll.Enabled = true;
            }
        }

        public DataSet Deserialize(string fileName)
        {
            //deserialize .json file
            string json = File.ReadAllText(fileName);
            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(json);
            return dataSet;
        }

        public async void Animation()
        {
            //animation using 5 pictures to do a countdown
            await Task.Delay(500);
            picReward.BackColor = System.Drawing.Color.White;
            picReward.SizeMode = PictureBoxSizeMode.StretchImage;
            picReward.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\Animation\5.png"));
            await Task.Delay(500);
            picReward.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\Animation\4.png"));
            await Task.Delay(500);
            picReward.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\Animation\3.png"));
            await Task.Delay(500);
            picReward.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\Animation\2.png"));
            await Task.Delay(500);
            picReward.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\Animation\1.png"));
        }

        public void RNG(DataTable skinTable, string nameOfCase)
        {
            //set up variables
            List<string> Names = new List<string>();
            bool exotic = false;
            string skinName;
            string finishName = null;
            string quality;
            DataSet dataSet;
            DataTable listTable = null;
            DataTable exoticTable = null;
            DataTable finishTable = null;

            //set up RNG
            Random r = new Random();
            int gen = r.Next(0, 10000);
            if (gen < 7992)
            {
                //set colour to blue
                picReward.BackColor = System.Drawing.Color.DodgerBlue;
                lblName.ForeColor = System.Drawing.Color.DodgerBlue;

                //get the names of skins with rarity "Mil-Spec" and add them to the list
                Names = skinTable.Rows.OfType<DataRow>()
                    .Where(dr => dr.Field<string>("Rarity") == "Mil-Spec")
                    .Select(dr => (string)dr["Name"]).ToList();
            }
            else if (gen < 9590)
            {
                //set colour to purple
                picReward.BackColor = System.Drawing.Color.MediumOrchid;
                lblName.ForeColor = System.Drawing.Color.MediumOrchid;

                //get the names of skins with rarity "Restricted" and add them to the list
                Names = skinTable.Rows.OfType<DataRow>()
                    .Where(dr => dr.Field<string>("Rarity") == "Restricted")
                    .Select(dr => (string)dr["Name"]).ToList();
            }
            else if (gen < 9910)
            {
                //set colour to pink
                picReward.BackColor = System.Drawing.Color.HotPink;
                lblName.ForeColor = System.Drawing.Color.HotPink;

                //get the names of skins with rarity "Classified" and add them to the list
                Names = skinTable.Rows.OfType<DataRow>()
                    .Where(dr => dr.Field<string>("Rarity") == "Classified")
                    .Select(dr => (string)dr["Name"]).ToList();
            }
            else if (gen < 9974)
            {
                //set colour to red
                picReward.BackColor = System.Drawing.Color.Tomato;
                lblName.ForeColor = System.Drawing.Color.Tomato;

                //get the names of skins with rarity "Covert" and add them to the list
                Names = skinTable.Rows.OfType<DataRow>()
                    .Where(dr => dr.Field<string>("Rarity") == "Covert")
                    .Select(dr => (string)dr["Name"]).ToList();
            }
            else
            {
                //set colour to gold
                picReward.BackColor = System.Drawing.Color.Goldenrod;
                lblName.ForeColor = System.Drawing.Color.Goldenrod;

                //deserialize the .json files that contain exotic data
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"json files\exotics\Case_Exotic_List.json");
                dataSet = Deserialize(fileName);
                listTable = dataSet.Tables["Case_Exotic_List"];

                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"json files\exotics\Exotics.json");
                dataSet = Deserialize(fileName);
                exoticTable = dataSet.Tables["Exotics"];

                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"json files\exotics\Finishes.json");
                dataSet = Deserialize(fileName);
                finishTable = dataSet.Tables["Finishes"];

                //set exotic to true so that the functions get the correct information
                exotic = true;
            }

            //get skin information
            skinName = GetSkinName(r, listTable, exoticTable, Names, nameOfCase, exotic);
            SetRewardIcon(skinName, nameOfCase, exotic);
            if (exotic == false)
            {
                quality = GetFloatAndQuality(skinTable, skinName);
            }
            else
            {
                finishName = GetExoticFinish(r, nameOfCase, listTable, finishTable);
                quality = GetFloatAndQuality(finishTable, finishName);
            }    
            skinName = GetStatTrak(r, skinName, finishName, exotic);
            SetSkinPrice(skinName, finishName, quality, exotic);
        }

        public string GetSkinName(Random r, DataTable listTable, DataTable exoticTable, List<string> Names, string nameOfCase, bool exotic)
        {
            int skinGen;
            string skinName;

            //if skin is exotic or not
            if (exotic == false)
            {
                //get random skin of correct rarity
                skinGen = r.Next(Names.Count());
                skinName = Names[skinGen];
            }
            else
            {
                //get case names and IDs to list
                List<string> caseNames = listTable.Rows.OfType<DataRow>()
                     .Select(dr => (string)dr["Name"]).ToList();
                List<double> SetIDList = listTable.Rows.OfType<DataRow>()
                        .Select(dr => (double)dr["SetID"]).ToList();
                List<double> ExoticIDList = listTable.Rows.OfType<DataRow>()
                        .Select(dr => (double)dr["ExoticID"]).ToList();

                //get the correct setID and exoticID for the case
                int index = caseNames.IndexOf(nameOfCase);
                double SetID = SetIDList[index];
                double ExoticID = ExoticIDList[index];

                //search for the exotics in a case
                List<string> exoticNames = new List<string>();
                if (SetID == 0)
                {
                    exoticNames = exoticTable.Rows.OfType<DataRow>()
                        .Where(dr => dr.Field<double>("ExoticID") == ExoticID)
                        .Select(dr => (string)dr["Name"]).ToList();
                }
                else
                {
                    exoticNames = exoticTable.Rows.OfType<DataRow>()
                        .Where(dr => dr.Field<double>("SetID") == SetID)
                        .Select(dr => (string)dr["Name"]).ToList();
                }

                //get a random exotic that is in the case
                skinGen = r.Next(exoticNames.Count());
                skinName = exoticNames[skinGen];
            }
            return skinName;
        }

        public void SetRewardIcon(string skinName, string nameOfCase, bool exotic)
        {
            if (exotic == false)
            {
                //set icon
                string picName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\Directory\"
                + skinName.Replace('|', '-') + ".png").Replace("Directory", nameOfCase);
                picReward.Image = Image.FromFile(picName);
                picReward.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                //set exotic photo
                picReward.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\Exotics\Knives\" +skinName + ".png"));
                picReward.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        public string GetFloatAndQuality(DataTable skinTable, string skinName)
        {
            //search for correct skin and get the float values for it
            string search = "Name = 'temp'".Replace("temp", skinName);
            DataRow[] Row = skinTable.Select(search);
            double FloatMin = Convert.ToDouble(Row[0]["FloatMin"]);
            double FloatMax = Convert.ToDouble(Row[0]["FloatMax"]);

            //get a weighted base float value
            //https://blog.csgofloat.com/analysis-of-float-value-and-paint-seed-distribution-in-cs-go/
            Random r = new Random();
            int gen = r.Next(0, 100);
            double result = r.NextDouble();
            if (gen <= 2)
            {
                result = result * 0.07;
            }
            else if (gen > 2 && gen <= 26)
            {
                result = result * 0.08 + 0.07;
            }
            else if (gen > 26 && gen <= 59)
            {
                result = result * 0.23 + 0.15;
            }
            else if (gen > 59 && gen <= 83)
            {
                result = result * 0.07 + 0.38;
            }
            else
            {
                result = result * 0.55 + 0.45;
            }

            //make float within the range for the skin
            result = result * (FloatMax - FloatMin) + FloatMin;
            
            //get skin quality
            string quality;
            if (result <= 0.07)
            {
                quality = "Factory New";
            }
            else if (result > 0.07 && result <= 0.15)
            {
                quality = "Minimal Wear";
            }
            else if (result > 0.15 && result <= 0.38)
            {
                quality = "Field-Tested";
            }
            else if (result > 0.38 && result <= 0.45)
            {
                quality = "Well-Worn";
            }
            else
            {
                quality = "Battle-Scarred";
            }

            //display
            lblQuality.Show();
            lblFloat.Show();
            lblQuality.Text = "Wear Value: " + quality;
            lblFloat.Text = "Float: " + result.ToString();
            return quality;
        }

        public string GetExoticFinish(Random r, string nameOfCase, DataTable listTable, DataTable finishTable)
        {
            //search and get the finishID of the case
            List<string> caseNames = listTable.Rows.OfType<DataRow>()
                    .Select(dr => (string)dr["Name"]).ToList();
            List<double> FinishIDList = listTable.Rows.OfType<DataRow>()
            .Select(dr => (double)dr["FinishID"]).ToList();

            //add the finishes to the list
            int index = caseNames.IndexOf(nameOfCase);
            double FinishID = FinishIDList[index];
            List<string> Finishes = finishTable.Rows.OfType<DataRow>()
                    .Where(dr => dr.Field<double>("FinishID") == FinishID)
                    .Select(dr => (string)dr["Name"]).ToList();

            //return a random finish
            int skinGen = r.Next(Finishes.Count());
            string finishName = Finishes[skinGen];
            return finishName;
        }

        public string GetStatTrak(Random r, string skinName, string finishName, bool exotic)
        {
            int StatTrakGen = r.Next(0, 10);
            //if skin is exotic or not
            if (exotic == true)
            {
                //set correct formatting for exotics
                if (StatTrakGen == 0)
                {
                    skinName = "★ StatTrak " + skinName;
                }
                else
                {
                    skinName = "★ " + skinName;
                }

                if (finishName != "Vanilla")
                {
                    skinName = skinName + " | " + finishName;
                }
            }
            else
            {
                //set correct formatting for skins
                if (StatTrakGen == 0)
                {
                    skinName = "StatTrak " + skinName;
                }
            }

            //display
            lblNameTitle.Show();
            lblName.Show();
            lblName.Text = skinName;
            return skinName;
        }

        public void SetSkinPrice(string skinName, string finishName, string quality, bool exotic)
        {
            string id;

            //if skin is exotic or not
            if (exotic == true && finishName == "Vanilla")
            {
                id = skinName;
            }
            else
            {
                id = skinName + " (" + quality + ")";
            }

            //pull API data from last year to try and get a good price for an item
            //some items (very expensive items) do not have price data since they don't sell often
            id = id + "&time=365";

            //pull from API
            string json = API(id);
            string priceString;
            PriceAPI skinPrice = JsonConvert.DeserializeObject<PriceAPI>(json);

            //if no price is there/cannot connect to API, it is handled
            if (skinPrice == null)
            {
                priceString = "Server Unavailable, Try Again Later.";
            }
            else
            {
                priceString = "$" + skinPrice.average_price.ToString();
            }

            //display
            lblPrice.Show();
            lblPrice.Text = "Price: " + priceString;
        }

        public string API(string id)
        {
            //connect to API
            //API I am using: https://csgobackpack.net/api/
            //This API is limited to 1000 calls per day but that is fine for my use case,
            //plus this can be switched easily to another API or a local database
            var url = "http://csgobackpack.net/api/GetItemPrice/?id=" + id;
            string jsonString;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //try to connect, but if there are any issues it returns nothing so it can be handled properly
            try
            {
                WebResponse response = request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                jsonString = sr.ReadToEnd();
                sr.Close();
                
            }
            catch
            {
                jsonString = "";
            }
            return jsonString;
        }
    }
}
