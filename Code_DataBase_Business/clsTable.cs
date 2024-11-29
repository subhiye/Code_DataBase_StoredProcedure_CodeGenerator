using Code_DataBase_SibaSbahi_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_DataBase_SibaSbahi_Business
{
    public class clsTable
    {
        public static DataTable GetAllTables(string DataBaseName , List<string>ValuesName)
        {
            return Table_Data.GetAllTables(DataBaseName, ValuesName);
        }
    }
}