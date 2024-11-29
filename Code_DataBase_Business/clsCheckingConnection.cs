using Code_DataBase_SibaSbahi_Data;
using System;
using System.Threading.Tasks;
namespace Code_DataBase_SibaSbahi_Business
{
    public class clsCheckingConnection
    {
        // before we start please Make sure that your data base name exactly matches your project name 
        // or the app will not generate the files which you want thanks.
        
        public string ServerID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string DataBaseName { get; set; }

        public clsCheckingConnection()
        {
            this.ServerID = "";
            this.userName = "";
            this.password = "";
            this.DataBaseName = "";
        }
        clsCheckingConnection(string ServerID, string DataBasename, string UserName, string Password)
        {
            this.ServerID = ServerID;
            this.userName = UserName;
            this.password = Password;
            this.DataBaseName = DataBasename;
        }

        public static clsCheckingConnection ValidateDatabaseConnection(string ServerID, string DataBasename, string UserName, string Password)
        {
            bool IsFound = CheckingConnection_Data.CanConnectToDatabase(ServerID, DataBasename, UserName, Password);
            if (IsFound)
            {
                return new clsCheckingConnection(ServerID, DataBasename, UserName, Password);
            }
            else
            {
                return null;
            }
        }
    }
}