using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.IO;
using Code_DataBase_SibaSbahi_Business;
using System.Security.Cryptography;
using Guna.UI2.WinForms;
using static Code_DataBase_SibaSbahi.DataBase_Generator.frmDataBaseGenerator;
namespace Code_DataBase_SibaSbahi.Code_Generators_Forms
{
    public partial class frmCode_Genrator_Main : Form
    {
        enum EnStatus { HaveStoredProcedure = 0, NotHave = 1 }
        EnStatus Status;
        public frmCode_Genrator_Main(string DataBaseName, string ServerID, string UserName, string Password, string KeyPath)
        {
            InitializeComponent();
            _DataBaseName = DataBaseName;
            _ServerID = ServerID;
            _UserName = UserName;
            _Password = Password;
            _KeyPath = KeyPath;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string _DataBaseName = "", _ServerID = "", _UserName = "", _Password = "", _KeyPath = "";
        private string GetBusinessLayerCode(List<clsColumn> Columns, string ClassName)
        {
            if (txtTableName.Text == "People" || txtTableName.Text == "people")
            {
                txtTableName.Text = "Person";
            }
            return clsGenerate_Business_Layer_Class.GenerateAllBusinessLayerMethods(txtTableName.Text, Columns);
        }
        private static string GetConfigValue(string databaseName, string serverID, string userName, string password, string keyPath)
        {
            string myString = $"Server={serverID};Database={databaseName};User Id={userName};Password={password};";
            return myString;
        }
        public static string AddOrUpdateAppSetting(string key, string value)
        {
            string ConnectionString = "";
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            { }
            return ConnectionString;
        }
        private string GetDataLayerCode(List<clsColumn> Columns, string ClassName)
        {
            if (txtTableName.Text == "People" || txtTableName.Text == "people")
            {
                txtTableName.Text = "Person";
            }
            return clsGenerat_Data_Layer_Class.GetDataLayerAllFunctionsCode(txtTableName.Text, _DataBaseName, Columns);
        }
        private void StoreFileToBusinessLayer(string BusinessLayerClassCode, string className, string filePath)
        {
            clsGenerate_Business_Layer_Class.CreateClassFile(BusinessLayerClassCode, className, filePath);
        }
        private void StoreFileToDataLayer(string dataLayerClassCode, string className, string filePath)
        {
            clsGenerate_Business_Layer_Class.CreateClassFile(dataLayerClassCode, className, filePath);
        }
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (txtBusinessString.Text.Length > 0)
            {
                Clipboard.SetText(txtBusinessString.Text);
                MessageBox.Show("Your Function Is Copied Successfully.", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Your Function Is Empty. Generate A Function To Copy.", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 
        private bool IsFileExist(string filePath)
        {
            return File.Exists(filePath);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            clsUtil.OpeningVisualStudio();
        }
        private void btnAdding_Click_1(object sender, EventArgs e)
        {
            clsUtil.OpeningSqlApp();
        }
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            txtBusinessString.Clear();
            txtDataString.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            txtTableName.Clear();
        }
        private void frmCode_Genrator_Main_Load(object sender, EventArgs e)
        {
            rbNotHave.Checked = true;
            Status = EnStatus.NotHave;
        }
        private void rbHaveStored_CheckedChanged(object sender, EventArgs e)
        {
            Status = EnStatus.HaveStoredProcedure;
        }
        private void rbNotHave_CheckedChanged(object sender, EventArgs e)
        {
            Status = EnStatus.NotHave;

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ExecuteStoredProcedures(List<clsColumn> columnsList, string dataBaseName, string tableName)
        {
            clsCreateStoredProcedures.CreateAddingObjectStoredProcedure(columnsList, dataBaseName, tableName);
            clsCreateStoredProcedures.CreateUpdatingObjectStoredProcedures(columnsList, dataBaseName, tableName);
            clsCreateStoredProcedures.CreateDeletingObjectStoredProcedures(columnsList, dataBaseName, tableName);
            clsCreateStoredProcedures.CreateReadingObjectByIDStoredProcedures(columnsList, dataBaseName, tableName);
            clsCreateStoredProcedures.CreateAllRecordsStoredProcedures(columnsList, dataBaseName, tableName);
            clsCreateStoredProcedures.CreateIsObjectFoundByIDStoredProcedures(columnsList, dataBaseName, tableName);
        }
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTableName.Text))
            { 
                errorProvider1.SetError(txtTableName, "Fill This Field Please To Generate Your Code"); 
                txtTableName.Focus();
                return; 
            }
            MessageBox.Show("Make Sure That You Created A Project For The Business , Data Layer Before Generating Any Code", "Be Careful", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            MessageBox.Show(@"Make Sure That the name of the database is the same as the name of yoru project ,
                             Otherwise you will not be able to get tow fils written from 
                             the program and you will have to copy the files and paste them into your program ",
                             "Be Careful", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            List<string> listRegistryDetails = new List<string>
            {
                _ServerID,
                _DataBaseName,
                _UserName,
                _Password
            };

            string BusinessFileName = _DataBaseName + "_Business", DataFileName = _DataBaseName + "_Data";

            //C: \Users\siba1\source\repos\Hospital_System\Hospital_System_Business
            string FilePathForBusiness = clsUtil.GetFilePath() + "\\" + _DataBaseName  + "\\" + BusinessFileName + "\\";
            
            //C: \Users\siba1\source\repos\Hospital_System\Hospital_System_Data
            string FilePathForData = clsUtil.GetFilePath() + "\\" + _DataBaseName + "\\" + DataFileName + "\\" ;

            // Check if files already exist
            if (IsFileExist(FilePathForBusiness) || IsFileExist(FilePathForData))
            {
                MessageBox.Show("File already exists. Please delete the existing file before generating a new one.", "File Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button4.Enabled = true;
                return;
            }

            List<clsColumn> columns = clsColumn.GetColumnsFromDataBase(_DataBaseName, txtTableName.Text, _KeyPath, listRegistryDetails);

            if (columns != null)
            {
                string ApplicationName = _DataBaseName;

                

                string DataLayerClassCode = GetDataLayerCode(columns, ApplicationName);
                string BusinessLayerClassCode = GetBusinessLayerCode(columns, ApplicationName);

                txtDataString.Text = DataLayerClassCode;
                txtBusinessString.Text = BusinessLayerClassCode;

                string ClassName = txtTableName.Text;

                if (txtTableName.Text == "People" || txtTableName.Text == "people")
                {
                    ClassName = "Person";
                }

                string ClassNameBusiness = "cls" + ClassName;
                string ClassNameData = ClassName + "_Data";

                StoreFileToBusinessLayer(BusinessLayerClassCode, ClassNameBusiness, FilePathForBusiness);
                StoreFileToDataLayer(DataLayerClassCode, ClassNameData, FilePathForData);

                if(Status == EnStatus.NotHave)
                {
                    ExecuteStoredProcedures(columns, _DataBaseName, txtTableName.Text);
                }
                else
                {
                    MessageBox.Show("We Won't Generate Generate Stored Procedures For You.", "Oops We Was Want To Help You More", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                MessageBox.Show("Class file generated successfully As A File And You Can Go To See It.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button4.Enabled = true;
                txtTableName.Text = "";

                // Clear the DataGridView as well
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
            }
            else
            {
                MessageBox.Show("No columns found or unable to fetch columns.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtTableName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTableName.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTableName, "Fill This Field Please.");
            }
            else
            {
                errorProvider1.SetError(txtTableName, "");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTableName.Text))
            {
                errorProvider1.SetError(txtTableName, "Fill This Field Please To Generate Your Code");
                txtTableName.Focus();
                return;
            }
            List<string> ListRegistryDetails = new List<string>
            {  _ServerID,_DataBaseName, _UserName,_Password};
            if (!clsColumn.IsTableFound(txtTableName.Text, _KeyPath, ListRegistryDetails))
            {
                MessageBox.Show("There Is No Table Called " + txtTableName.Text, "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<clsColumn> columns = clsColumn.GetColumnsFromDataBase(_DataBaseName, txtTableName.Text, _KeyPath, ListRegistryDetails);
            if (columns != null)
            {
                dataGridView1.DataSource = columns;
                if (dataGridView1.ColumnCount > 0)
                {
                    dataGridView1.Columns[0].HeaderText = "Column Name";
                    dataGridView1.Columns[0].Width = 150;
                    dataGridView1.Columns[1].HeaderText = "Data Type";
                    dataGridView1.Columns[1].Width = 120;
                    dataGridView1.Columns[2].HeaderText = "Is Null";
                    dataGridView1.Columns[2].Width = 80;
                }
                MessageBox.Show(@"Make Sure That Your Application And Your Data Base Are In The Same Name Or The Code Generator Will Not Works Correctly", "Hello",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBusinessString.Text = "";
                txtDataString.Text = "";
                button4.Enabled = false;
            }
            else
            {
                MessageBox.Show("No columns found or unable to fetch columns.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}