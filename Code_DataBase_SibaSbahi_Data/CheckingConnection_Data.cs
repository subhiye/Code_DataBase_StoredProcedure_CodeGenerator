using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_DataBase_SibaSbahi_Data
{
    public class CheckingConnection_Data
    {
        public static bool CanConnectToDatabase(string ServerID, string DataBaseName, string userName, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString_Data.ConnectionStringLineCode(ServerID, DataBaseName, userName, password)))
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {}
            return false;
        }

    }
}