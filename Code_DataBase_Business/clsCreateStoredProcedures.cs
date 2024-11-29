using Code_DataBase_SibaSbahi_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Code_DataBase_SibaSbahi_Business
{
    public class clsCreateStoredProcedures
    {
        private static string ConvertObjectToLine(clsColumn Column, string Seperator = ", ")
        {
            string StringObject = "";
            StringObject += Column.ColumnName + Seperator;
            StringObject += Column.DataType + Seperator;
            StringObject += Column.IsNull ;
            return StringObject;
        }
        private static List<string>ConvertObjectsToLines(List<clsColumn> ListColumns)
        {
            List<string> StringList = new List<string>();
            foreach(clsColumn Col in ListColumns)
            {
                string Line = ConvertObjectToLine(Col, " ");
                StringList.Add(Line);
            }
            return StringList;
        }
        public static void CreateAddingObjectStoredProcedure(List<clsColumn> ColumnList, string DataVaseName, string tableName)
        {
            List<string> StringList = ConvertObjectsToLines(ColumnList);
             CreateStoredProcedures_Data.CreateAddingObjectStoredProcedure
            (StringList, DataVaseName, tableName);
        }
        public static void CreateUpdatingObjectStoredProcedures(List<clsColumn> ColumnList, string DataVaseName, string tableName)
        {
            List<string> StringList = ConvertObjectsToLines(ColumnList);
             CreateStoredProcedures_Data.CreateUpdatingObjectStoredProcedures
            (StringList, DataVaseName, tableName);
        }
        public static void CreateDeletingObjectStoredProcedures(List<clsColumn> ColumnList, string DataVaseName, string tableName)
        {
            List<string> StringList = ConvertObjectsToLines(ColumnList);
             CreateStoredProcedures_Data.CreateDeletingObjectStoredProcedures
            (StringList, DataVaseName, tableName);
        }
        public static void CreateReadingObjectByIDStoredProcedures(List<clsColumn> ColumnList, string DataVaseName, string tableName)
        {
            List<string> StringList = ConvertObjectsToLines(ColumnList);
             CreateStoredProcedures_Data.CreateReadingObjectByIDStoredProcedures
            (StringList, DataVaseName, tableName);
        }
        public static void CreateAllRecordsStoredProcedures(List<clsColumn> ColumnList, string DataVaseName, string tableName)
        {
            List<string> StringList = ConvertObjectsToLines(ColumnList);
             CreateStoredProcedures_Data.CreateAllRecordsStoredProcedures
            (StringList, DataVaseName, tableName);
        }      
        public static void CreateIsObjectFoundByIDStoredProcedures(List<clsColumn> ColumnList, string DataVaseName, string tableName)
        {
            List<string> StringList = ConvertObjectsToLines(ColumnList);
             CreateStoredProcedures_Data.CreateIsObjectFoundByIDStoredProcedures
            (StringList, DataVaseName, tableName);
        }
        
    }
}