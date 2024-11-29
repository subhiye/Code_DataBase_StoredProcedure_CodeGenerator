using Code_DataBase_SibaSbahi.Code_Generators_Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Code_DataBase_SibaSbahi
{
    public partial class frmStartPage : Form
    {
        public frmStartPage()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            frmCodeGeneratorMainPage frm = new frmCodeGeneratorMainPage();
            frm.ShowDialog();
        }
    }
}