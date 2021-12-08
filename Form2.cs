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
            string colour = "window";

            //set the name of the form to name of the case
            string realName = nameOfCase.Replace("__", ":").Replace('_', ' ');
            this.Text = realName;
            double casePrice = SetCasePrice(realName);

            //open the correct .json file, deserialize the file and place data into a datatable
            DataSet dataSet = Deserialize(nameOfCase);
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
                    colour = "DodgerBlue";
                }
                else if (row["Rarity"].ToString() == "Restricted")
                {
                    colour = "MediumOrchid";
                }
                else if (row["Rarity"].ToString() == "Classified")
                {
                    colour = "HotPink";

                }
                else if (row["Rarity"].ToString() == "Covert")
                {
                    colour = "Tomato";
                }

                //get name of skin from datatable then add it to the label
                string skinName = row["Name"].ToString();
                labels[i].Show();
                labels[i].Text = skinName;
                
                //get picture filepath then resize and add it to the picturebox
                string picName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\Directory\"
                + skinName.Replace('|', '-') + ".png").Replace("Directory", nameOfCase);
                pictures[i].Show();
                try
                {
                    pictures[i].Image = Image.FromFile(picName);
                }
                catch
                {
                    string message = "Cannot Load Icons. \nThe program will now close.";
                    string title = "Error";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.OK)
                    {
                        System.Environment.Exit(0);
                    }
                }
                pictures[i].SizeMode = PictureBoxSizeMode.StretchImage;

                //set colour
                labels[i].ForeColor = System.Drawing.Color.FromName(colour);
                pictures[i].BackColor = System.Drawing.Color.FromName(colour);

                //increment i so all items done
                i++;
            }

            //print the "Rare Special Item" menu icon
            labels[i].Show();
            labels[i].ForeColor = System.Drawing.Color.Goldenrod;
            labels[i].Text = "Rare Special Item";

            pictures[i].Show();
            pictures[i].BackColor = System.Drawing.Color.Goldenrod;
            try
            {
                pictures[i].Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\knife.png"));
            }
            catch
            {
                string message = "Cannot Load Icons. \nThe program will now close.";
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
            pictures[i].SizeMode = PictureBoxSizeMode.StretchImage;

            //route roll button to custom handler
            //this is so i can send "nameOfCase" and "skinTable" to RNG()
            btnRoll.Click += (sender, EventArgs) => { BtnRoll_Click(skinTable, nameOfCase, casePrice); };
            timerAutoRoll.Tick += (sender, EventArgs) => { Timer_Tick(skinTable, nameOfCase, casePrice); };
        }

        private async void BtnRoll_Click(DataTable skinTable, string nameOfCase, double casePrice)
        {
            //disables button so you cant spam the program
            //this to stop overlapping animations and from the user interfering with the auto-roll
            btnRoll.Enabled = false;

            //checks if auto-roll/animation skip is enabled
            if (chkAutoRoll.Checked == false)
            {
                if (chkAnimationSkip.Checked == false)
                {
                    Animation();
                    await Task.Delay(3000);
                }
                RNG(skinTable, nameOfCase, casePrice);
                await Task.Delay(400);
                btnRoll.Enabled = true;
            }
            else
            {
                timerAutoRoll.Start();
                this.ControlBox = false;
            }
        }

        private async void Timer_Tick(DataTable skinTable, string nameOfCase, double casePrice)
        {
            //auto-roll feature
            if (chkAutoRoll.Checked == true)
            {
                if (chkAnimationSkip.Checked == false)
                {
                    timerAutoRoll.Interval = 4000;
                    Animation();
                    await Task.Delay(3000);
                }
                else
                {
                    timerAutoRoll.Interval = 800;
                    await Task.Delay(800);
                }
                RNG(skinTable, nameOfCase, casePrice);
            }
            else
            {
                timerAutoRoll.Stop();
                btnRoll.Enabled = true;
                this.ControlBox = true;
            }
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
                string message;
                if (nameOfCase.Contains("exotics"))
                {
                    message = "Cannot Load JSON Files for the exotic weapons. \nThe program will now close.";
                }
                else
                {
                    message = "Cannot Load JSON Files. \nThe program will now close.";
                }
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

        public async void Animation()
        {
            try
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
            catch
            {
                string message = "Cannot Load Animation. \nThe program will now close.";
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
        }

        public void RNG(DataTable skinTable, string nameOfCase, double casePrice)
        {
            //set up variables
            List<string> Names = new List<string>();
            DataSet dataSet = new DataSet();
            DataTable listTable = new DataTable();
            DataTable exoticTable = new DataTable();
            DataTable finishTable = new DataTable();

            bool exotic = false;
            string skinName = null;
            string finishName = null;
            string quality = null;
            string colour = "window";
            double skinPrice = 0.00;    

            //set up RNG
            Random r = new Random();
            int gen = r.Next(0, 10000);
            double priceRandom = r.NextDouble() * 0.5 + 0.5;
            if (gen < 7992)
            {
                //set colour to blue
                colour = "DodgerBlue";
                skinPrice = Math.Round(1.90 - (1.4 * priceRandom), 2);

                //get the names of skins with rarity "Mil-Spec" and add them to the list
                Names = skinTable.Rows.OfType<DataRow>()
                    .Where(dr => dr.Field<string>("Rarity") == "Mil-Spec")
                    .Select(dr => (string)dr["Name"]).ToList();
            }
            else if (gen < 9590)
            {
                //set colour to purple
                colour = "MediumOrchid";
                skinPrice = Math.Round(10.00 - (8 * priceRandom), 2);

                //get the names of skins with rarity "Restricted" and add them to the list
                Names = skinTable.Rows.OfType<DataRow>()
                    .Where(dr => dr.Field<string>("Rarity") == "Restricted")
                    .Select(dr => (string)dr["Name"]).ToList();
            }
            else if (gen < 9910)
            {
                //set colour to pink
                colour = "HotPink";
                skinPrice = Math.Round(32.00 - (24 * priceRandom), 2);

                //get the names of skins with rarity "Classified" and add them to the list
                Names = skinTable.Rows.OfType<DataRow>()
                    .Where(dr => dr.Field<string>("Rarity") == "Classified")
                    .Select(dr => (string)dr["Name"]).ToList();
            }
            else if (gen < 9974)
            {
                //set colour to red
                colour = "Tomato";
                skinPrice = Math.Round(130.00 - (100 * priceRandom), 2);

                //get the names of skins with rarity "Covert" and add them to the list
                Names = skinTable.Rows.OfType<DataRow>()
                    .Where(dr => dr.Field<string>("Rarity") == "Covert")
                    .Select(dr => (string)dr["Name"]).ToList();
            }
            else
            {
                //set colour to gold
                colour = "Goldenrod";
                skinPrice = Math.Round(1100 - (1000 * priceRandom), 2);

                //deserialize the .json files that contain exotic data
                string jsonName = @"exotics\Case_Exotic_List";
                dataSet = Deserialize(jsonName);
                listTable = dataSet.Tables["Case_Exotic_List"];

                jsonName = @"exotics\Exotics";
                dataSet = Deserialize(jsonName);
                exoticTable = dataSet.Tables["Exotics"];

                jsonName = @"exotics\Finishes";
                dataSet = Deserialize(jsonName);
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
            skinPrice = SetSkinPrice(skinName, finishName, quality, skinPrice, exotic);
            listSkins.Items.Insert(0, skinName + " - $" + skinPrice);
            picReward.BackColor = System.Drawing.Color.FromName(colour);
            lblName.ForeColor = System.Drawing.Color.FromName(colour);
            listSkins.Items[0].BackColor = System.Drawing.Color.FromName(colour);
            CalcProfit(skinPrice, casePrice);
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
            string picName;
            if (exotic == false)
            {
                //set icon
                picName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\Directory\"
                + skinName.Replace('|', '-') + ".png").Replace("Directory", nameOfCase);          
            }
            else
            {
                //set exotic photo
                picName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"icons\Exotics\Knives\" + skinName + ".png");
            }

            try
            {
                picReward.Image = Image.FromFile(picName);
                picReward.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch
            {
                string message = "Cannot Load Icons. \nThe program will now close.";
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
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
                result *= 0.07;
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
                if (StatTrakGen == 0 && skinName.Contains("Gloves") == false)
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

        public double SetCasePrice(string realName)
        {
            string id = realName;
            double price;
            double cost;

            //pull API data from last year to try and get a good price for an item
            //some items (very expensive items) do not have price data since they don't sell often
            id += "&time=365";

            //pull from API
            string json = API(id);
            PriceAPI casePrice = JsonConvert.DeserializeObject<PriceAPI>(json);

            //if no price is there/cannot connect to API, it is handled and static values are used instead.

            if (casePrice.success == false)
            {
                price = 1.00;
            }
            else
            {
                price = casePrice.average_price;
            }
            cost = price + 2.50;

            //display
            lblCasePrice.Text = "Case Price: $" + price.ToString();
            lblCost.Text = "Cost Per Open: $" + cost;
            return price;
        }

        public double SetSkinPrice(string skinName, string finishName, string quality, double importPrice, bool exotic)
        {
            string id;
            double price;

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
            id += "&time=365";

            //pull from API
            string json = API(id);
            PriceAPI skinPrice = JsonConvert.DeserializeObject<PriceAPI>(json);

            //if no price is there/cannot connect to API, it is handled and static values are used instead.
            if (skinPrice.success == false)
            {
                price = importPrice;
            }
            else
            {
                price = skinPrice.average_price;
            }

            //display
            lblPrice.Text = "Skin Price: $" + price;
            return price;
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
                jsonString = "{\"success\":\"false\"}";
            }
            return jsonString;
        }

        public void CalcProfit(double skinPrice, double casePrice)
        {
            //calculate profit and store the result in a label
            double oldprofit = Convert.ToDouble(lblProfit.Text.Remove(0, 9));
            double profit = oldprofit + skinPrice - (casePrice + 2.50);
            if (profit < 0)
            {
                //make colour red if profit is negative
                lblProfit.ForeColor = System.Drawing.Color.Tomato;
            }
            else
            {
                //make colour green if profit is positive
                lblProfit.ForeColor = System.Drawing.Color.LimeGreen;
            }
            lblProfit.Text = "Profit: $" + profit;
        }
    }
}
