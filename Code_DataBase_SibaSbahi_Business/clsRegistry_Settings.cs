using Code_DataBase_SibaSbahi_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Code_DataBase_SibaSbahi_Business
{
    public class clsRegistry_Settings
    {
        public static bool StoreIntoRegestry(string KeyPath, Dictionary<string, string> ValueNames, string ApplicationName)
        {
            bool IsStored = Registry_Settings_Data.StoreIntoRegistry(KeyPath, ValueNames, ApplicationName);
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

        public static Dictionary<string, string> GetValueFromRegestry(string KeyPath, List<string> ValuesNames, string ApplicationName)
        {
            return Registry_Settings_Data.GetValueNameFromRegistry(KeyPath, ValuesNames, ApplicationName);
        }

        public bool IsValuseFound(string ValueName, string KeyPath)
        {
            return Registry_Settings_Data.IsValueFound(ValueName, KeyPath);
        }


        public Dictionary<string, bool> AreValuesFound(string ValueName, List<string>ValueNames)
        {
            return Registry_Settings_Data.AreValuesFound(ValueName, ValueNames);
        }

        public static bool DeleteValueFromRegistry(string KeyPath, string ValueName, string ApplicationName)
        {
            bool IsDeleted = Registry_Settings_Data.DeleteValueFromRegistry(KeyPath, ValueName, ApplicationName);
            return IsDeleted;
        }

        public static bool DeleteValueFromRegistry(string KeyPath, List<string> ValueNames, string ApplicationName)
        {
            bool IsDeleted = Registry_Settings_Data.DeleteValueFromRegistry(KeyPath, ValueNames, ApplicationName);
            return IsDeleted;
        }
    }
}
