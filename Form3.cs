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
    public partial class FrmSearch : Form
    {
        public FrmSearch()
        {
            InitializeComponent();
            List<string> caseList = Search();
            BtnSearch.Click += (sender, EventArgs) => {caseList = BtnSearch_Click(sender, EventArgs); };
            BtnGo.Click += (sender, EventArgs) => { BtnGo_Click(caseList); };
        }

        private List<string> Search()
        {
            //set up search
            DataTable skinTable;
            DataSet dataSet;
            List<string> caseList = new List<string>();
            int i = 0;

            //read in all .json files
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"json files\");
            string searchValue = TxtSearch.Text;

            //for each file, search and return all matching entries
            try
            {
                foreach (string fileName in Directory.EnumerateFiles(directory, "*.json"))
                {
                    string nameOfCase = fileName.Replace(directory, "").Replace(".json", "");
                    string realName = nameOfCase.Replace("__", ":").Replace('_', ' ');
                    dataSet = Deserialize(fileName);
                    skinTable = dataSet.Tables[nameOfCase];
                    List<string> Names = skinTable.Rows.OfType<DataRow>()
                        .Where(dr => dr.Field<string>("Name").Contains(searchValue))
                        .Select(dr => (string)dr["Name"]).ToList();
                    Names.ForEach(delegate (string name)
                    {
                        caseList.Add(nameOfCase);
                        ListResults.Items.Add(name + " - " + realName);
                    });
                    i++;
                }
            }
            catch
            {
                string message = "JSON File Directory Missing. \nThe program will now close.";
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                }
            }
            lblFilesLoaded.Text = "Loaded " + i + "/36 Files";
            return caseList;
        }

        public DataSet Deserialize(string fileName)
        {
            DataSet dataSet = new DataSet();
            try
            {
                //deserialize .json file
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

        private List<string> BtnSearch_Click(object sender, EventArgs e)
        {
            ListResults.Items.Clear();
            List<string> caseList = Search();
            return caseList;
        }

        private void BtnGo_Click(List<string> caseList)
        {
            if (ListResults.SelectedItem != null)
            {
                int i = ListResults.SelectedIndex;
                frmOpener frmOpener = new frmOpener(caseList[i]);
                frmOpener.Show();
            }
        }
    }
}
