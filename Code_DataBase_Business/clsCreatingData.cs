using Code_DataBase_SibaSbahi_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Code_DataBase_SibaSbahi_Business
{
    public class clsCreatingData
    {
        public string ColumnName { get; set; }
        public string ColumnDataType { get; set; }
        public bool IsNull { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignkey { get; set; }
        public clsCreatingData()
        {
            this.ColumnName = "";
            this.ColumnDataType = "";
            this.IsNull = false;
            this.IsIdentity = false;
            this.IsPrimaryKey = false;
            this.IsForeignkey = false;
        }
        public static bool AddDataBaseToSqlDataBaseStudio(string databaseName)
        {
            return CreatingDataBase_Data.AddDataBaseToSqlDataBaseStudio(databaseName);
        }

        public static bool AddTable(string databaseName, string tableName, List<clsCreatingData> columnsList)
        {
            bool isAdded = false;

            StringBuilder columnsBuilder = new StringBuilder();

            foreach (clsCreatingData c in columnsList)
            {
                string isIdentity = c.IsIdentity ? "IDENTITY (1,1)" : "";
                string isPrimary = c.IsPrimaryKey ? "PRIMARY KEY" : "";
                string isForeignKey = c.IsForeignkey ? "FOREIGN KEY" : "";
                string isNull = c.IsNull ? "NULL" : "NOT NULL";

                columnsBuilder.Append($"{c.ColumnName} {c.ColumnDataType} {isIdentity} {isPrimary} {isForeignKey} {isNull}, ");
            }

            if (columnsBuilder.Length > 0)
            {
                columnsBuilder.Length -= 2;
            }

            CreatingDataBase_Data.AddTableIntoDataBase(databaseName, tableName, columnsBuilder);
            return isAdded;
        }

        public static bool IsDataBaseExists(string DataBaseName)
        {
            return CreatingDataBase_Data.IsDataBaseExists(DataBaseName);
        }
    }
}
