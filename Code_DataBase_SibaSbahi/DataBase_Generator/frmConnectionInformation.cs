using Code_DataBase_SibaSbahi.Global_Class;
using Code_DataBase_SibaSbahi_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Code_DataBase_SibaSbahi.DataBase_Generator
{
    public partial class frmConnectionInformation : Form
    {
        public frmConnectionInformation()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        clsCheckingConnection _ConnectionValues;
        private void btnCreateNow_Click(object sender, EventArgs e)
        {
            _ConnectionValues.ServerID = stbServerID.Text;
            _ConnectionValues.userName = stbUserid.Text;
            _ConnectionValues.password = stbPassword.Text;
            _ConnectionValues.DataBaseName = stbDataBaseName.Text;

            if (clsCreatingData.IsDataBaseExists(stbDataBaseName.Text))
            {
                clsGlobalClasses.ConnectionValues = _ConnectionValues;
                frmDataBaseGenerator frm = new frmDataBaseGenerator(stbKeyPath.Text, stbDataBaseName.Text);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Data Base Is Not Exictes", "Sorry");
                linkLabel1.Visible = true;
            }
        }
       
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddData frm = new frmAddData();
            frm.ShowDialog();
        }
        private void frmConnectionInformation_Load_1(object sender, EventArgs e)
        {
            _ConnectionValues = new clsCheckingConnection();
            stbServerID.Text = ".";
            stbUserid.Text = "sa";
            stbPassword.Text = "sa123456";
            stbKeyPath.Text = "Current";
            linkLabel1.Visible = false;
        }
    }
}