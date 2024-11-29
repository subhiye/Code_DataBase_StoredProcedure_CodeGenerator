using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_DataBase_SibaSbahi_Data
{
    public class Column_Data
    {
        public class clsColumn
        {
            public string ColumnName { get; set; }
            public string DataType { get; set; }
            public string IsNull { get; set; }
        }

        private static string GetconnectionString(string ServerID, string DataBaseName,
        string UserName, string Password)
        {
            return ConnectionString_Data.ConnectionStringLineCode(ServerID, DataBaseName, UserName, Password);
        }

        public static bool GetColumnsNames(string DataBaseName, string TableName, ref List<clsColumn> ArryOfColumns, string KeyPath, List<string> ValuesName)
        {
            bool IsFound = false;
            string ConnectionString = GetconnectionString(ValuesName[0], ValuesName[1], ValuesName[2], ValuesName[3]);
            try
            {
                using (SqlConnection Con = new SqlConnection(ConnectionString))
                {
                    Con.Open();
                    string query = @"
                SELECT COLUMN_NAME, DATA_TYPE,
                       CASE 
                           WHEN DATA_TYPE IN ('varchar', 'nvarchar', 'char', 'nchar') 
                           THEN CONCAT(DATA_TYPE, '(', CHARACTER_MAXIMUM_LENGTH, ')')
                           ELSE DATA_TYPE 
                       END AS FULL_DATA_TYPE,
                       CASE 
                           WHEN IS_NULLABLE = 'YES' THEN 'NULL'
                           ELSE 'NOT NULL' 
                       END AS IS_NULLABLE 
                FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = @TableName";

                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        cmd.Parameters.AddWithValue("@TableName", TableName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    clsColumn ColumnDetails = new clsColumn();
                                    ColumnDetails.ColumnName = reader["COLUMN_NAME"].ToString();
                                    ColumnDetails.DataType = reader["FULL_DATA_TYPE"].ToString();
                                    ColumnDetails.IsNull = reader["IS_NULLABLE"].ToString();
                                    ArryOfColumns.Add(ColumnDetails);
                                }
                                IsFound = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if necessary
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return IsFound;
        }


        public static bool GetColumnsNamesAsStringList
        (string DataBaseName, string TableName, ref List<string> ArryOfColumns,
        string KeyPath, List<string> ValuesName)
        {
            bool IsFound = false;
            string ConnectionString = GetconnectionString(ValuesName[0], ValuesName[1], ValuesName[2], ValuesName[3]);
            try
            {
                using (SqlConnection Con = new SqlConnection(ConnectionString))
                {
                    Con.Open();
                    string query = @"SELECT COLUMN_NAME, DATA_TYPE, 
                            CASE 
                                WHEN IS_NULLABLE = 'YES' THEN 'NULL'
                                ELSE 'NOT NULL' 
                            END AS IS_NULLABLE 
                            FROM INFORMATION_SCHEMA.COLUMNS 
                            WHERE TABLE_NAME = @TableName";

                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        cmd.Parameters.AddWithValue("@TableName", TableName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string Name = reader["COLUMN_NAME"].ToString();
                                    string Type = reader["DATA_TYPE"].ToString();
                                    string IsNull = reader["IS_NULLABLE"].ToString();
                                    string ColumnDetails = Name + ' ' + Type + ' ' + IsNull;
                                    ArryOfColumns.Add(ColumnDetails);
                                }
                                IsFound = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return IsFound;
        }

        public static bool IsTableFoundInDataBase(string TableName, string KeyPath, List<string> ValuesNames)
        {
            string ConnectionString = GetconnectionString(ValuesNames[0], ValuesNames[1], ValuesNames[2], ValuesNames[3]);
            bool IsTableFound = false;
            try
            {
                using (SqlConnection Conn = new SqlConnection(ConnectionString))
                {
                    Conn.Open();
                    string query = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";
                    using (SqlCommand cmd = new SqlCommand(query, Conn))
                    {
                        cmd.Parameters.AddWithValue("@TableName", TableName);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            IsTableFound = true;
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return IsTableFound;
        }
    }
}
