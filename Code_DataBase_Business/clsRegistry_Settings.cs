using Code_DataBase_SibaSbahi_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_DataBase_SibaSbahi_Business
{
    public class clsRegistry_Settings
    {
        public static bool StoreIntoRegestry(string KeyPath, string ValueName, string ValueData, string ApplicationName)
        {
            bool IsStored = Registry_Settings_Data.StoreIntoRegistry(KeyPath, ValueName, ValueData, ApplicationName);
            return IsStored;
        }

        public static bool StoreIntoRegistry(string keyPath, string value, string ValueData, string applicationName)
        {
            bool IsStored = Registry_Settings_Data.StoreIntoRegistry(keyPath, value, ValueData, applicationName);
            return IsStored;
        }

        public static string GetValueFromRegestry(string KeyPath, string ValueName, string ApplicationName)
        {
            string FullValue = Registry_Settings_Data.GetValueNameFromRegistry(KeyPath, ValueName, ApplicationName);
            return FullValue;
        }

        public bool IsValuseFound(string ValueName, string KeyPath)
        {
            return Registry_Settings_Data.IsValueFound(ValueName, KeyPath);
        }

        public static bool DeleteValueFromRegistry(string KeyPath, string ValueName, string ApplicationName)
        {
            bool IsDeleted = Registry_Settings_Data.DeleteValueFromRegistry(KeyPath, ValueName, ApplicationName);
            return IsDeleted;
        }
    }
}
