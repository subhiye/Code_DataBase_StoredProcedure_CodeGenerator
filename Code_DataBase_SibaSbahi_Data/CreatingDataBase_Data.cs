using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_DataBase_SibaSbahi_Data
{
    public class CreatingDataBase_Data
    {
        public static string connectionString = "Server=.;Database=master; User Id=sa ;Password = sa123456;";

        public static bool IsDataBaseExists(string databaseName)
        {
            bool isDataBaseExist = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string checkDbQuery = $"SELECT database_id FROM sys.databases WHERE Name = '{databaseName}'";
                    using (SqlCommand checkCommand = new SqlCommand(checkDbQuery, connection))
                    {
                        var result = checkCommand.ExecuteScalar();
                        if (result != null)
                        {
                            isDataBaseExist = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return isDataBaseExist;
        }

        public static bool IsTableExists(string databaseName, string TableName)
        {
            bool isExist = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string Query = $@"USE {databaseName}; GO SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
                                   WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = {TableName};";
                    using (SqlCommand checkCommand = new SqlCommand(Query, connection))
                    {
                        var result = checkCommand.ExecuteScalar();
                        if (result != null)
                        {
                            isExist = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return isExist;
        }

        public static bool AddTableIntoDataBase(string databaseName, string tableName, StringBuilder columnsBuilder)
        {
            bool isAdded = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string createTableQuery = $@"USE {databaseName}; CREATE TABLE {tableName} ({columnsBuilder})";

                    using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        isAdded = true;
                        Console.WriteLine($"Table '{tableName}' created successfully.");
                    }
                }
            }
            catch (SqlException ex)
            {
                isAdded = false;
            }
            catch (Exception ex)
            {
                isAdded = false;
            }
            return isAdded;
        }

        public static bool AddTableWithForiegnKeys (string databaseName, string tableName, StringBuilder columnsBuilder)
        {
            bool isAdded = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string createTableQuery = $@"USE {databaseName}; 
                    CREATE TABLE {tableName} ({columnsBuilder})";

                    using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        isAdded = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                isAdded = false;
            }
            catch (Exception ex)
            {
                isAdded = false;
            }
            return isAdded;
        }

        public static bool AddDataBaseToSqlDataBaseStudio(string databaseName)
        {
            bool isAdded = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    if (!IsDataBaseExists(databaseName))
                    {
                        string createDbQuery = $"CREATE DATABASE {databaseName}";
                        using (SqlCommand command = new SqlCommand(createDbQuery, connection))
                        {
                            command.ExecuteNonQuery();
                            isAdded = true;
                        }
                    }
                    else
                    {
                        isAdded = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                isAdded = false;
            }
            catch (Exception ex)
            {
                isAdded = false;
            }
            return isAdded;
        }

        public static List<string> GetDataBaseList(ref List<string> DataBaseList)
        {
            string query = "SELECT name FROM master.sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DataBaseList.Add(reader["name"].ToString());
                }
            }
            return DataBaseList;
        }
    }

}