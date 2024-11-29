using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Code_DataBase_SibaSbahi_Data
{
    public class Table_Data
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

        public static DataTable GetAllTables(string DatabaseName, List<string> ValuesName)
        {
            string ConnectionString = GetconnectionString(ValuesName[0], ValuesName[1], ValuesName[2], ValuesName[3]);
            DataTable tableList = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    // Use the specified database
                    string useDatabase = $@"USE [{DatabaseName}];";
                    using (SqlCommand useCommand = new SqlCommand(useDatabase, con))
                    {
                        useCommand.ExecuteNonQuery();
                    }

                    // Query to get all tables
                    string query =
                                    @"SELECT 
                                    T.TABLE_NAME,
                                    C.COLUMN_NAME AS Identity_Column
                                FROM 
                                    INFORMATION_SCHEMA.TABLES T
                                JOIN 
                                    INFORMATION_SCHEMA.COLUMNS C ON T.TABLE_NAME = C.TABLE_NAME
                                WHERE 
                                    T.TABLE_TYPE = 'BASE TABLE' 
                                    AND COLUMNPROPERTY(OBJECT_ID(T.TABLE_NAME), C.COLUMN_NAME, 'IsIdentity') = 1
                                ORDER BY 
                                    T.TABLE_NAME;";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                tableList.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return tableList;
        }
    }
}