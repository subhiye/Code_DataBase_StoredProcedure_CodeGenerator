using Code_DataBase_SibaSbahi_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_DataBase_SibaSbahi_Business
{
    public class clsColumn
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string IsNull { get; set; }
        public clsColumn(string ColumnName, string DataType, string IsNull)
        {
            this.ColumnName = ColumnName;
            this.DataType = DataType;
            this.IsNull = IsNull;
        }
        private static List<clsColumn> FillColumnObjects(List<Column_Data.clsColumn> columnsArray)
        {
            List<clsColumn> ColumnsObjectList = new List<clsColumn>();

            foreach (var column in columnsArray)
            {
                clsColumn clsCol = new clsColumn(column.ColumnName, column.DataType, column.IsNull);
                ColumnsObjectList.Add(clsCol);
            }
            return ColumnsObjectList;
        }
        public static List<clsColumn> GetColumnsFromDataBase(string DataBaseName, string TableName, string KeyPath,
            List<string> ValuesName)
        {
            List<Column_Data.clsColumn> ArryOfColumns = new List<Column_Data.clsColumn>();

            bool IsFound = Column_Data.GetColumnsNames(DataBaseName, TableName, ref ArryOfColumns, KeyPath,
                ValuesName);

            if (IsFound)
            {
                List<clsColumn> ColumnsObjectList = FillColumnObjects(ArryOfColumns);

                return ColumnsObjectList;
            }
            else
            {
                return null;
            }
        }

        
        public static bool IsTableFound(string TableName, string KeyPath, List<string> ValuesNames)
        {
            return Column_Data.IsTableFoundInDataBase(TableName, KeyPath, ValuesNames);
        }
    }
}