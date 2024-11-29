using Code_DataBase_SibaSbahi.Global_Class;
using Code_DataBase_SibaSbahi_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
namespace Code_DataBase_SibaSbahi.DataBase_Generator
{
    public partial class frmDataBaseGenerator : Form
    {
        string TableName = "", IdName = "", _KeyPath ="" , _DataBaseName="";
        public frmDataBaseGenerator(string KeyPath, string DataBaseName)
        {
            InitializeComponent();
            _DataBaseName = DataBaseName;
        }
        List<clsCreatingData> ColumnsList = new List<clsCreatingData>();
        private void FillDataGrideViewColumns()
        {
            if (guna2DataGridView1.Columns.Count == 0)
            {
                guna2DataGridView1.Columns.Add("ColumnName", "Column");
                guna2DataGridView1.Columns.Add("ColumnDataType", "Type");
                guna2DataGridView1.Columns.Add("ColumnNullOrNot", "Null");
                guna2DataGridView1.Columns.Add("IdentityOrNot", "Identity");
                guna2DataGridView1.Columns.Add("PrimaryKeyNot", "Primary");
                guna2DataGridView1.Columns.Add("ForeignKeyOrNot", "Foreign");
            }
        }
        private void GetTablesList(List<string> ValuesNames)
        {
            DataTable dtTables = clsTable.GetAllTables(_DataBaseName, ValuesNames);

            // Clear existing rows and columns
            guna2DataGridView2.Rows.Clear();
            guna2DataGridView2.Columns.Clear();

            // Automatically use columns from DataTable
            guna2DataGridView2.DataSource = dtTables;
        }
        List<string> valuesnames = new List<string>();
        private List<string> fillListString(List<string> vlauesList)
        {
            vlauesList.Add(clsGlobalClasses.ConnectionValues.ServerID);
            vlauesList.Add(clsGlobalClasses.ConnectionValues.DataBaseName);
            vlauesList.Add(clsGlobalClasses.ConnectionValues.userName);
            vlauesList.Add(clsGlobalClasses.ConnectionValues.password);
            return vlauesList;
        }
        private clsCreatingData FillObjectColunn(clsCreatingData column)
        {
            column.ColumnName = stbColumnName.Text;
            column.ColumnDataType = guna2ComboBox1.SelectedItem.ToString();
            if (guna2RadioButton1.Checked) column.IsNull = true;
            column.IsIdentity = gcbIsIdentity.Checked ? true : false;
            column.IsPrimaryKey = guna2RadioButton5.Checked ? true : false;
            column.IsForeignkey = guna2RadioButton4.Checked ? true : false;
            return column;
        }
        public enum StoredProcedureType
        {
            AddObject,
            UpdateObject,
            DeleteObject,
            ReadObjectByID,
            ReadObjectByNationalNo,
            ReadObjectByFullName,
            GetAllRecords,
            IsObjectFoundByNationalNumber,
            IsObjectFoundByID,
            IsObjectFoundByFullName,
            IsObjectFoundByNamePassword
        }
        public void ExecuteStoredProcedures(List<clsColumn> columnsList, string dataBaseName, string tableName)
        {
            // Loop through each value in the StoredProcedureType enum
            foreach (var procedure in Enum.GetValues(typeof(StoredProcedureType)))
            {
                // Use a switch statement to call the appropriate function based on the checked state of the corresponding checkbox
                switch (procedure)
                {
                    case StoredProcedureType.AddObject:
                        if (guna2CheckBox6.Checked)
                            clsCreateStoredProcedures.CreateAddingObjectStoredProcedure(columnsList, dataBaseName, tableName);
                        break;
                    case StoredProcedureType.UpdateObject:
                        if (guna2CheckBox8.Checked)
                            clsCreateStoredProcedures.CreateUpdatingObjectStoredProcedures(columnsList, dataBaseName, tableName);
                        break;
                    case StoredProcedureType.DeleteObject:
                        if (guna2CheckBox4.Checked)
                            clsCreateStoredProcedures.CreateDeletingObjectStoredProcedures(columnsList, dataBaseName, tableName);
                        break;
                    case StoredProcedureType.ReadObjectByID:
                        if (guna2CheckBox5.Checked)
                            clsCreateStoredProcedures.CreateReadingObjectByIDStoredProcedures(columnsList, dataBaseName, tableName);
                        break;
                    case StoredProcedureType.GetAllRecords:
                        if (guna2CheckBox9.Checked)
                            clsCreateStoredProcedures.CreateAllRecordsStoredProcedures(columnsList, dataBaseName, tableName);
                        break;

                    case StoredProcedureType.IsObjectFoundByID:
                        if (guna2CheckBox3.Checked)
                            clsCreateStoredProcedures.CreateIsObjectFoundByIDStoredProcedures(columnsList, dataBaseName, tableName);
                        break;
                }
            }
        }
        private void guna2GradientButton2_Click_1(object sender, EventArgs e)
        {
            string DataBaseName = _DataBaseName;
            string tableName = stbTableName.Text;
            List<string> listRegistryDetails = new List<string>
            {
                clsGlobalClasses.ConnectionValues.ServerID,
                clsGlobalClasses.ConnectionValues.DataBaseName,
                clsGlobalClasses.ConnectionValues.userName,
                clsGlobalClasses.ConnectionValues.password
            };

            List<clsColumn> ColumnsList = clsColumn.GetColumnsFromDataBase(_DataBaseName, stbTableName.Text, _KeyPath, listRegistryDetails);

            if (ColumnsList != null)
            {
                ExecuteStoredProcedures(ColumnsList, DataBaseName, tableName);
                MessageBox.Show("Your Stored Procedures Are Created Successfuly.");
            }
            else
            {
                MessageBox.Show("No columns found or unable to fetch columns.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void guna2CheckBox11_CheckedChanged_1(object sender, EventArgs e)
        {
            if (guna2CheckBox11.Checked)
            {
                guna2GroupBox1.Enabled = false;
                guna2GroupBox2.Enabled = true;
            }
            else
            {
                guna2GroupBox1.Enabled = true;
                guna2GroupBox2.Enabled = false;
            }
        }
        private void btnAddColumn_Click(object sender, EventArgs e)
        {
            clsCreatingData ColumnDetails = new clsCreatingData();
            ColumnDetails = FillObjectColunn(ColumnDetails);
            ColumnsList.Add(ColumnDetails);

            guna2DataGridView1.Rows.Add(ColumnDetails.ColumnName, ColumnDetails.ColumnDataType, ColumnDetails.IsNull,
            ColumnDetails.IsIdentity, ColumnDetails.IsPrimaryKey, ColumnDetails.IsForeignkey);

            stbColumnName.Text = "";
            guna2ComboBox1.SelectedIndex = -1;
            guna2RadioButton1.Checked = false;
            guna2RadioButton2.Checked = false;
            gcbIsIdentity.Checked = false;
        }
        private void stbColumnName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(stbColumnName.Text))
            {
                errorProvider1.SetError(stbColumnName, "no fill this please");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(stbColumnName, "");
            }
        }
        private void guna2GradientButton1_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(stbTableName.Text))
            {
                errorProvider1.SetError(stbTableName,
                "Fill This Field Please To Generate Your Code");
                stbTableName.Focus();
                return;
            }

            List<string> ListRegistryDetails = new List<string>
            {
                clsGlobalClasses.ConnectionValues.ServerID,
                clsGlobalClasses.ConnectionValues.DataBaseName,
                clsGlobalClasses.ConnectionValues.userName,
                clsGlobalClasses.ConnectionValues.password
            };

            if (!clsColumn.IsTableFound(stbTableName.Text, _KeyPath, ListRegistryDetails))
            {
                MessageBox.Show("There Is No Table Called ", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<clsColumn> columns = clsColumn.GetColumnsFromDataBase(_DataBaseName, stbTableName.Text, _KeyPath, ListRegistryDetails);

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
            }
            else
            {
                MessageBox.Show("No columns found or unable to fetch columns.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDataBaseGenerator_Load(object sender, EventArgs e)
        {
            FillDataGrideViewColumns();
            fillListString(valuesnames);
            GetTablesList(valuesnames);
        }
        private void tbTableName_TextChanged_1(object sender, EventArgs e)
        {
            string text = tbTableName.Text.Trim();
            stbColumnName.Text = text + "ID";
        }
        private void btnCreateColumn_Click_2(object sender, EventArgs e)
        {
            string tableName = tbTableName.Text;

            if (clsCreatingData.AddTable(_DataBaseName, tableName, ColumnsList))
            {
                MessageBox.Show("Your Table Created Successfully Congratulations .", "Wow", MessageBoxButtons.OK, MessageBoxIcon.Information);
                guna2DataGridView1.ClearSelection();
            }
            else
            {
                MessageBox.Show("Your Table Created Successfully Congratulations .", "Wow", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}