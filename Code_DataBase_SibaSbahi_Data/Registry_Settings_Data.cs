using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_DataBase_SibaSbahi_Data
{
    public class Registry_Settings_Data
    {
        private static string ChooseKeyPath(string KeyPath = "Current")
        {
            if (KeyPath == "Current")
            {
                return @"HKEY_CURRENT_USER\SOFTWARE\";
            }
            else
            {
                return @"HKEY_LOCAL_MACHINE\SOFTWARE\";
            }
        }

        public static bool StoreIntoRegistry(string KeyPath, string ValueName, string ValueData, string ApplicationName)
        {
            string KeyPathString = ChooseKeyPath(KeyPath) + ApplicationName + "DataBase\\";
            bool IsStored = false;
            try
            {
                // Open the registry key in read/write mode with explicit registry view
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    RegistryKey key = baseKey.OpenSubKey(KeyPathString, true);
                    if (key == null)
                    {
                        // Create the key if it doesn't exist
                        key = baseKey.CreateSubKey(KeyPathString);
                    }
                    // Check if the value exists
                    object existingValue = key.GetValue(ValueName);
                    if (existingValue != null)
                    {
                        // Delete the value if it exists
                        key.DeleteValue(ValueName, false);
                    }
                    // Set the new value
                    key.SetValue(ValueName, ValueData, RegistryValueKind.String);
                    IsStored = true;
                }
            }
            catch (Exception ex) { }
            return IsStored;
        }

        public static bool StoreIntoRegistry(string KeyPath, Dictionary<string, string> valueDataPairs, string ApplicationName)
        {
            string KeyPathString = ChooseKeyPath(KeyPath) + ApplicationName + "DataBase\\";
            bool IsStored = false;
            try
            {
                // Open the registry key in read/write mode with explicit registry view
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    RegistryKey key = baseKey.OpenSubKey(KeyPathString, true);
                    if (key == null)
                    {
                        // Create the key if it doesn't exist
                        key = baseKey.CreateSubKey(KeyPathString);
                    }
                    foreach (var pair in valueDataPairs)
                    {
                        // Check if the value exists
                        object existingValue = key.GetValue(pair.Key);
                        if (existingValue != null)
                        {
                            // Delete the value if it exists
                            key.DeleteValue(pair.Key, false);
                        }
                        // Set the new value
                        key.SetValue(pair.Key, pair.Value, RegistryValueKind.String);
                    }
                    IsStored = true;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }
            return IsStored;
        }

        public static string GetValueNameFromRegistry(string KeyPath, string ValueName, string ApplicationName)
        {
            string FullValue = "";
            string KeyPathString = ChooseKeyPath(KeyPath) + ApplicationName + "\\";
            try
            {
                string ValueData = Registry.GetValue(KeyPathString, ValueName, null) as string;
                if (ValueData != null)
                {
                    FullValue = ValueData;
                }
                else
                {
                    FullValue = null;
                }
            }
            catch (Exception ex) { }
            return FullValue;
        }

        public static Dictionary<string, string> GetValueNameFromRegistry(string KeyPath, List<string> ValueNames, string ApplicationName)
        {
            string KeyPathString = ChooseKeyPath(KeyPath) + ApplicationName + "DataBase\\";
            Dictionary<string, string> retrievedValues = new Dictionary<string, string>();
            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    RegistryKey key = baseKey.OpenSubKey(KeyPathString, false);
                    if (key != null)
                    {
                        foreach (string valueName in ValueNames)
                        {
                            object value = key.GetValue(valueName);
                            if (value != null)
                            {
                                retrievedValues[valueName] = value.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }
            return retrievedValues;
        }
        
        public static bool DeleteValueFromRegistry(string keyPath, string valueName, string applicationName)
        {
            bool IsDeleted = false;
            // Specify the registry key path and value name
            string keyPathString = @"SOFTWARE" + "\\" + applicationName;
            try
            {
                // Open the registry key in read/write mode with explicit registry view
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    using (RegistryKey key = baseKey.OpenSubKey(keyPathString, true))
                    {
                        if (key != null)
                        {
                            // Delete the specified value
                            key.DeleteValue(valueName);
                            IsDeleted = true;
                        }
                        else
                        {
                            IsDeleted = false;
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                IsDeleted = false;
            }
            catch (Exception ex)
            {
                IsDeleted = false;
            }
            return IsDeleted;
        }

        public static bool DeleteValueFromRegistry(string KeyPath, List<string> ValueNames, string ApplicationName)
        {
            string KeyPathString = ChooseKeyPath(KeyPath) + ApplicationName + "DataBase\\";
            bool IsDeleted = false;
            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    RegistryKey key = baseKey.OpenSubKey(KeyPathString, true);
                    if (key != null)
                    {
                        foreach (string valueName in ValueNames)
                        {
                            object existingValue = key.GetValue(valueName);
                            if (existingValue != null)
                            {
                                key.DeleteValue(valueName, false);
                            }
                        }
                        IsDeleted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }
            return IsDeleted;
        }

        public static bool IsValueFound(string ValueName, string KeyPath)
        {
            bool isFound = false;
            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    RegistryKey key = baseKey.OpenSubKey(KeyPath, true);
                    if (key != null)
                    {
                        object existingValue = key.GetValue(ValueName);
                        if (existingValue != null)
                        {
                            isFound = true;
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return isFound;
        }

        public static Dictionary<string, bool> AreValuesFound(string KeyPath, List<string> ValueNames)
        {
            Dictionary<string, bool> foundValues = new Dictionary<string, bool>();
            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    RegistryKey key = baseKey.OpenSubKey(KeyPath, true);
                    if (key != null)
                    {
                        foreach (string valueName in ValueNames)
                        {
                            object existingValue = key.GetValue(valueName);
                            foundValues[valueName] = (existingValue != null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }
            return foundValues;
        }
    }
}