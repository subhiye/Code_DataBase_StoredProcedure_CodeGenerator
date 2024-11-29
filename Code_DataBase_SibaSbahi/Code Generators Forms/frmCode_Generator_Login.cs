using Code_DataBase_SibaSbahi_Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
namespace Code_DataBase_SibaSbahi.Code_Generators_Forms
{
    public partial class frmCode_Generator_Login : Form
    {
        List<string> DataBaseList = new List<string>();

        public frmCode_Generator_Login()
        {
            InitializeComponent();
            DataBaseList = clsCreatingData.GetDataBaseList(DataBaseList);
        }
        private void guna2GradientButton1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCode_Generator_Login_Load(object sender, EventArgs e)
        {
            txtServerID.Text = ".";
            txtUserName.Text = "sa";
            txtPassword.Text = "sa123456";
            txtKeyPath.Text = "Current";
            txtDataBaseName.Focus();
            foreach(string s in DataBaseList)
            {
                listBoxDatabases1.Items.Add(s);
            }      
        }
        private void btnSql_Click_1(object sender, EventArgs e)
        {
            clsUtil.OpeningSqlApp();
        }
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            clsCheckingConnection Data = clsCheckingConnection.ValidateDatabaseConnection(txtServerID.Text, txtDataBaseName.Text, txtUserName.Text,
            txtPassword.Text);
            if (Data != null)
            {
                clsRegistry_Settings.StoreIntoRegistry(txtKeyPath.Text, lblDataBase.Text, txtDataBaseName.Text, txtDataBaseName.Text);
                clsRegistry_Settings.StoreIntoRegistry(txtKeyPath.Text, lblPassword.Text, txtPassword.Text, txtDataBaseName.Text);
                clsRegistry_Settings.StoreIntoRegistry(txtKeyPath.Text, lblServerName.Text, txtServerID.Text, txtDataBaseName.Text);
                clsRegistry_Settings.StoreIntoRegistry(txtKeyPath.Text, lblUserID.Text, lblUserID.Text, txtDataBaseName.Text);

                frmCode_Genrator_Main frm = new frmCode_Genrator_Main(txtDataBaseName.Text, txtServerID.Text, txtUserName.Text, txtPassword.Text, txtKeyPath.Text);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Unable to connect to the database. Please check your credentials and try again.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }
       
        private void listBoxDatabases1_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxDatabases1.SelectedItem != null)
            {
                txtDataBaseName.Text = listBoxDatabases1.SelectedItem.ToString(); 
            }
        }
    }
}