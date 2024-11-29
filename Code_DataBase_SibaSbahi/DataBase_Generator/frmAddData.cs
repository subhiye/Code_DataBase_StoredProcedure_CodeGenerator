﻿using Code_DataBase_SibaSbahi_Business;
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
    public partial class frmAddData : Form
    {
        public frmAddData()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreateNow_Click(object sender, EventArgs e)
        {
            string databaseName = stbDataBaseName.Text;
            if (clsCreatingData.AddDataBaseToSqlDataBaseStudio(databaseName))
            {
                MessageBox.Show("Your Data Base Created Successfully.", $"Wow Amazing ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data Base Faild Creating.", $"Sorry ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}