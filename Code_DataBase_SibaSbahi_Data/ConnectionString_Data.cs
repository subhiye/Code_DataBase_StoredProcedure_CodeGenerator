using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_DataBase_SibaSbahi_Data
{
    public class ConnectionString_Data
    {
        public static string ConnectionStringLineCode(string serverID, string databaseName, string userName, string password)
        {
            return $"Server={serverID};Database={databaseName};User Id={userName};Password={password};";
        }
    }
}
