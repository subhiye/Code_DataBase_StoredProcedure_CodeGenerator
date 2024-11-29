using Code_DataBase_SibaSbahi.Code_Generators_Forms;
using Code_DataBase_SibaSbahi.DataBase_Generator;
using System;
using System.Data;
using System.Windows.Forms;

namespace Code_DataBase_SibaSbahi
{
    public partial class frmCodeGeneratorMainPage : Form
    {
        public frmCodeGeneratorMainPage()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                frmCode_Generator_Login frm = new frmCode_Generator_Login();
                frm.ShowDialog();
            }
            else
            {
                frmConnectionInformation frm = new frmConnectionInformation();
                frm.ShowDialog();
            }
        }

        private void frmCodeGeneratorMainPage_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
