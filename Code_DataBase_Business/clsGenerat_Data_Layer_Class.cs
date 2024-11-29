using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Code_DataBase_SibaSbahi_Business
{
    public class clsGenerat_Data_Layer_Class
    {
        private static string GetCSharpDataType(string sqlDataType)
        {
            switch (sqlDataType.ToLower())
            {
                case "int":
                    return "int";
                case "smallint":
                    return "short";
                case "bigint":
                    return "long";
                case "bit":
                    return "bool";
                case "decimal":
                case "numeric":
                    return "decimal";
                case "float":
                    return "double";
                case "real":
                    return "float";
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                    return "string";
                case "datetime":
                case "date":
                case "smalldatetime":
                    return "DateTime";
                case "uniqueidentifier":
                    return "Guid";
                default:
                    return "string";
            }
        }

        private static string IfNullGetCSharpDataType(string sqlDataType)
        {
            switch (sqlDataType.ToLower())
            {
                case "int":
                    return "null";
                case "smallint":
                    return "null";
                case "bigint":
                    return "null";
                case "bit":
                    return "bool";
                case "decimal":
                    return "null";
                case "float":
                    return "null";
                case "real":
                    return "null";
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                    return "null";
                case "datetime":
                case "date":
                case "smalldatetime":
                    return "null";
                case "uniqueidentifier":
                    return "Guid";
                default:
                    return "null";
            }
        }

        private static string GetNullableCSharpDataType(string dataType, string isNull)
        {
            string csharpType = GetCSharpDataType(dataType);
            if (dataType == "string" || isNull == "NOT NULL")
                return csharpType;
            else
                return $"{csharpType}?";
        }

        private static string GetHeader(string className, string appName)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine("using System;");
            functionBody.AppendLine("using System.Data;");
            functionBody.AppendLine("using System.Data.SqlClient;");
            functionBody.AppendLine("using System.Collections.Generic;");
            functionBody.AppendLine("using System.IO;");
            functionBody.AppendLine("using System.Linq;");
            functionBody.AppendLine("using System.Text;");
            functionBody.AppendLine("using System.Configuration;");
            functionBody.AppendLine("// Make sure to add Refrance Configuration");
            functionBody.AppendLine($"namespace {appName}_Data");
            functionBody.AppendLine("{");
            functionBody.AppendLine($"    public class {className}_Data");
            functionBody.AppendLine("    {");
            return functionBody.ToString();
        }

        private static string GenerateDataAccessSettingsClass()
        {
            string classBody = 
            @"        public static string ConnectionString = ConfigurationManager.ConnectionStrings";
            return classBody;
        }

        private static string GenerateAddingFunction(string className,  List<clsColumn> arrayOfColumns)
        {
            StringBuilder functionBody = new StringBuilder();
            string objectID = arrayOfColumns[0].ToString();

            functionBody.Append($"        public static int AddNew{className}(");
            foreach (clsColumn column in arrayOfColumns)
            {
                functionBody.Append($"{GetCSharpDataType(column.DataType)} {column.ColumnName},");
            }
            functionBody.Length -= 1; // Remove trailing comma

            functionBody.Append(")");
            functionBody.AppendLine("        {");
            functionBody.AppendLine("            int newObjectID = 0;");
            functionBody.AppendLine("            try");
            functionBody.AppendLine("            {");
            functionBody.AppendLine($"                using (SqlConnection connection = new SqlConnection(connectionString))");
            functionBody.AppendLine("                {");
            functionBody.AppendLine("                    connection.Open();");
            functionBody.AppendLine($"                    using (SqlCommand command = new SqlCommand(\"SP_AddNew{className}\", connection))");
            functionBody.AppendLine("                    {");
            functionBody.AppendLine("                        command.CommandType = CommandType.StoredProcedure;");

            // Add parameters with appropriate sizes if necessary
            foreach (clsColumn column in arrayOfColumns)
            {
                functionBody.AppendLine($"                        command.Parameters.AddWithValue(\"@{column.ColumnName}\", {column.ColumnName});");
            }

            functionBody.AppendLine($"                        SqlParameter outputIdParam = new SqlParameter(\"@New{className}ID\", SqlDbType.Int)");
            functionBody.AppendLine("                        {");
            functionBody.AppendLine("                            Direction = ParameterDirection.Output");
            functionBody.AppendLine("                        };");
            functionBody.AppendLine("                        command.Parameters.Add(outputIdParam);");

            functionBody.AppendLine("                        // Execute");
            functionBody.AppendLine("                        command.ExecuteNonQuery();");

            functionBody.AppendLine("                        // Retrieve the ID of the new object");
            functionBody.AppendLine($"                        newObjectID = (int)command.Parameters[\"@New{className}ID\"].Value;");
            functionBody.AppendLine("                    }");
            functionBody.AppendLine("                }");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("            catch (Exception ex)");
            functionBody.AppendLine("            {");
            functionBody.AppendLine("                Console.WriteLine($\"An error occurred: {ex.Message}\");");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("            return newObjectID;");
            functionBody.AppendLine("        }");

            return functionBody.ToString();
        }

        private static string GenerateGetObjectInfoByIDFunction(string tableName, string connectionString, List<clsColumn> arrayOfColumns)
        {
            StringBuilder functionBody = new StringBuilder();

            string objectID = arrayOfColumns[0].ColumnName;
            List<clsColumn> otherColumns = arrayOfColumns.Skip(1).ToList();

            functionBody.AppendLine($"        public static bool Get{tableName}InfoByID(int {objectID}, ref {string.Join(", ref ", otherColumns.Select(c => $"{GetNullableCSharpDataType(c.DataType, c.IsNull)} {c.ColumnName}"))})");
            functionBody.AppendLine("        {");
            functionBody.AppendLine("            bool isFound = false;");
            functionBody.AppendLine("            try");
            functionBody.AppendLine("            {");
            functionBody.AppendLine($"                using (SqlConnection connection = new SqlConnection(\"{connectionString}\"))");
            functionBody.AppendLine("                {");
            functionBody.AppendLine($"                    using (SqlCommand command = new SqlCommand(\"SP_Get{tableName}InfoByID\", connection))");
            functionBody.AppendLine("                    {");
            functionBody.AppendLine("                        command.CommandType = CommandType.StoredProcedure;");
            functionBody.AppendLine($"                        command.Parameters.AddWithValue(\"@{objectID}\", {objectID});");
            functionBody.AppendLine("                        connection.Open();");
            functionBody.AppendLine("                        using (SqlDataReader reader = command.ExecuteReader())");
            functionBody.AppendLine("                        {");
            functionBody.AppendLine("                            if (reader.Read())");
            functionBody.AppendLine("                            {");
            functionBody.AppendLine("                                isFound = true;");
            foreach (var column in otherColumns)
            {
                if (column.IsNull == "NULL")
                {
                    functionBody.AppendLine($"                                if (reader[\"{column.ColumnName}\"] != DBNull.Value)");
                    functionBody.AppendLine("                                {");
                    functionBody.AppendLine($"                                    {column.ColumnName} = ({GetCSharpDataType(column.DataType)})reader[\"{column.ColumnName}\"];");
                    functionBody.AppendLine("                                }");
                    functionBody.AppendLine("                                else");
                    functionBody.AppendLine("                                {");
                    functionBody.AppendLine($"                                    {column.ColumnName} = {IfNullGetCSharpDataType(column.DataType)};");
                    functionBody.AppendLine("                                }");
                }
                else
                {
                    functionBody.AppendLine($"                                {column.ColumnName} = ({GetCSharpDataType(column.DataType)})reader[\"{column.ColumnName}\"];");
                }
            }
            functionBody.AppendLine("                            }");
            functionBody.AppendLine("                        }");
            functionBody.AppendLine("                    }");
            functionBody.AppendLine("                }");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("            catch (Exception ex)");
            functionBody.AppendLine("            {");
            functionBody.AppendLine("                Console.WriteLine($\"An error occurred: {ex.Message}\");");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("            return isFound;");
            functionBody.AppendLine("        }");

            return functionBody.ToString();
        }

        private static string GenerateRetrievingFunction(string tableName, string connectionString)
        {
            StringBuilder functionBody = new StringBuilder();

            functionBody.AppendLine($"        public static DataTable GetAll{tableName}()");
            functionBody.AppendLine("        {");
            functionBody.AppendLine("            DataTable dt = new DataTable();");
            functionBody.AppendLine("            try");
            functionBody.AppendLine("            {");
            functionBody.AppendLine($"               string query = @\"SELECT * FROM {tableName}\";");
            functionBody.AppendLine($"               using (SqlConnection connection = new SqlConnection(\"{connectionString}\"))");
            functionBody.AppendLine("               {");
            functionBody.AppendLine("                   using (SqlCommand command = new SqlCommand(query, connection))");
            functionBody.AppendLine("                   {");
            functionBody.AppendLine("                       connection.Open();");
            functionBody.AppendLine("                       using (SqlDataReader reader = command.ExecuteReader())");
            functionBody.AppendLine("                       {");
            functionBody.AppendLine("                           if (reader.HasRows)");
            functionBody.AppendLine("                           {");
            functionBody.AppendLine("                               dt.Load(reader);");
            functionBody.AppendLine("                           }");
            functionBody.AppendLine("                       }");
            functionBody.AppendLine("                   }");
            functionBody.AppendLine("               }");
            functionBody.AppendLine("           }");
            functionBody.AppendLine("           catch (Exception ex)");
            functionBody.AppendLine("           {");
            functionBody.AppendLine("               Console.WriteLine($\"An error occurred: {ex.Message}\");");
            functionBody.AppendLine("           }");
            functionBody.AppendLine("           return dt;");
            functionBody.AppendLine("       }");

            return functionBody.ToString();
        }

        private static string GenerateUpdatingFunction(string className, string connectionString, List<clsColumn> arrayOfColumns)
        {
            StringBuilder functionBody = new StringBuilder();
            string primaryKey = arrayOfColumns[0].ColumnName;

            functionBody.Append($"public static bool Update{className}(");
            foreach (clsColumn column in arrayOfColumns)
            {
                functionBody.Append($"{GetCSharpDataType(column.DataType)} {column.ColumnName}, ");
            }
            functionBody.Length -= 2; // Remove trailing comma and space

            functionBody.AppendLine(")");
            functionBody.AppendLine("{");
            functionBody.AppendLine("    int rowsAffected = 0;");
            functionBody.AppendLine("    try");
            functionBody.AppendLine("    {");
            functionBody.AppendLine($"        string connectionString = \"{connectionString}\";");
            functionBody.AppendLine("        using (SqlConnection connection = new SqlConnection(connectionString))");
            functionBody.AppendLine("        {");
            functionBody.AppendLine("            connection.Open();");
            functionBody.AppendLine($"            string query = @\"UPDATE {className} SET");

            for (int i = 1; i < arrayOfColumns.Count; i++)
            {
                functionBody.AppendLine($"                {arrayOfColumns[i].ColumnName} = @{arrayOfColumns[i].ColumnName},");
            }
            functionBody.Length -= 3; // Remove trailing comma and newline
            functionBody.AppendLine($" WHERE {primaryKey} = @{primaryKey}\";");

            functionBody.AppendLine("            using (SqlCommand command = new SqlCommand(query, connection))");
            functionBody.AppendLine("            {");
            foreach (clsColumn column in arrayOfColumns)
            {
                functionBody.AppendLine($"                command.Parameters.AddWithValue(\"@{column.ColumnName}\", {column.ColumnName});");
            }

            functionBody.AppendLine("                // Execute");
            functionBody.AppendLine("                rowsAffected = command.ExecuteNonQuery();");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("        }");
            functionBody.AppendLine("    }");
            functionBody.AppendLine("    catch (Exception ex)");
            functionBody.AppendLine("    {");
            functionBody.AppendLine("        Console.WriteLine($\"An error occurred: {ex.Message}\");");
            functionBody.AppendLine("    }");
            functionBody.AppendLine("    return rowsAffected > 0;");
            functionBody.AppendLine("}");

            return functionBody.ToString();
        }

        private static string GenerateGetObjectByNationalNumberFunction(string TableName, string ConnectionString, List<clsColumn> ArrayOfColumns)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine($"        public static bool Get{TableName}InfoBy(string NationalNumber, ref {string.Join(", ref ", ArrayOfColumns.Select(c => $"{GetNullableCSharpDataType(c.DataType, c.IsNull)} {c.ColumnName}"))})");
            functionBody.AppendLine("        {");
            functionBody.AppendLine("            bool isFound = false;");
            functionBody.AppendLine("            try");
            functionBody.AppendLine("            {");
            functionBody.AppendLine($"                using (SqlConnection connection = new SqlConnection(\"{ConnectionString}\"))");
            functionBody.AppendLine("                {");
            functionBody.AppendLine($"                    using (SqlCommand command = new SqlCommand(\"SP_Get{TableName}InfoByNationalNumber\", connection))");
            functionBody.AppendLine("                    {");
            functionBody.AppendLine("                        command.CommandType = CommandType.StoredProcedure;");
            functionBody.AppendLine("                        command.Parameters.AddWithValue(\"@NationalNumber\", NationalNumber);");
            functionBody.AppendLine("                        connection.Open();");
            functionBody.AppendLine("                        using (SqlDataReader reader = command.ExecuteReader())");
            functionBody.AppendLine("                        {");
            functionBody.AppendLine("                            if (reader.Read())");
            functionBody.AppendLine("                            {");
            functionBody.AppendLine("                                isFound = true;");
            foreach (var column in ArrayOfColumns)
            {
                if (column.IsNull == "NULL")
                {
                    functionBody.AppendLine($"                        if (reader[\"{column.ColumnName}\"] != DBNull.Value)");
                    functionBody.AppendLine("                        {");
                    functionBody.AppendLine($"                            {column.ColumnName} = ({GetCSharpDataType(column.DataType)})reader[\"{column.ColumnName}\"];");
                    functionBody.AppendLine("                        }");
                    functionBody.AppendLine("                        else");
                    functionBody.AppendLine("                        {");
                    functionBody.AppendLine($"                            {column.ColumnName} = {IfNullGetCSharpDataType(column.DataType)};");
                    functionBody.AppendLine("                        }");
                }
                else
                {
                    functionBody.AppendLine($"                        {column.ColumnName} = ({GetCSharpDataType(column.DataType)})reader[\"{column.ColumnName}\"];");
                }
            }
            functionBody.AppendLine("                            }");
            functionBody.AppendLine("                       }");
            functionBody.AppendLine("                   }");
            functionBody.AppendLine("               }");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("           catch (Exception ex)");
            functionBody.AppendLine("           {");
            functionBody.AppendLine("               Console.WriteLine($\"An error occurred: {ex.Message}\");");
            functionBody.AppendLine("           }");
            functionBody.AppendLine("           return isFound;");
            functionBody.AppendLine("       }");

            return functionBody.ToString();
        }

        private static string GenerateObjectFoundByIDFunction(string tableName, string connectionString)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine($"        public static void Check{tableName}Exists(int {tableName}ID)");
            functionBody.AppendLine("        {");
            functionBody.AppendLine("            try");
            functionBody.AppendLine("            {");
            functionBody.AppendLine($"                string connectionString = \"{connectionString}\";");
            functionBody.AppendLine("                using (SqlConnection connection = new SqlConnection(connectionString))");
            functionBody.AppendLine("                {");
            functionBody.AppendLine("                    connection.Open();");
            functionBody.AppendLine($"                    using (SqlCommand command = new SqlCommand(\"SP_Check{tableName}Exists\", connection))");
            functionBody.AppendLine("                    {");
            functionBody.AppendLine("                        command.CommandType = CommandType.StoredProcedure;");
            functionBody.AppendLine();
            functionBody.AppendLine($"                        command.Parameters.AddWithValue(\"@{tableName}ID\", (object) {tableName}ID ?? DBNull.Value);");
            functionBody.AppendLine();
            functionBody.AppendLine("                        SqlParameter returnParameter = new SqlParameter(\"@ReturnVal\", SqlDbType.Int)");
            functionBody.AppendLine("                        {");
            functionBody.AppendLine("                            Direction = ParameterDirection.ReturnValue");
            functionBody.AppendLine("                        };");
            functionBody.AppendLine("                        command.Parameters.Add(returnParameter);");
            functionBody.AppendLine("                        command.ExecuteNonQuery();");
            functionBody.AppendLine();
            functionBody.AppendLine("                        bool objectExists = (int)returnParameter.Value == 1;");
            functionBody.AppendLine();
            functionBody.AppendLine($"                        Console.WriteLine($\" {tableName} Exists:  {{objectExists}}\");");
            functionBody.AppendLine();
            functionBody.AppendLine("                        connection.Close();");
            functionBody.AppendLine("                    }");
            functionBody.AppendLine("                }");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("            catch (Exception ex)");
            functionBody.AppendLine("            {");
            functionBody.AppendLine($"                Console.WriteLine($\" {tableName} doesn't exist: {{ex.Message}}\");");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("        }");

            return functionBody.ToString();
        }

        private static string GenerateDeleteFunction(string TableName, string ConnectionString, List<clsColumn> ArrayOfColumns)
        {
            StringBuilder functionBody = new StringBuilder();

            // Generate function signature
            functionBody.AppendLine($"        public static bool Delete{TableName}(int objectID)");
            functionBody.AppendLine("         {");
            functionBody.AppendLine("             bool isDeleted = false;");
            functionBody.AppendLine("             try");
            functionBody.AppendLine("             {");
            functionBody.AppendLine($"                 using(SqlConnection connection = new SqlConnection(\"{ConnectionString}\"))");
            functionBody.AppendLine("                 {");
            functionBody.AppendLine($"                    string query = \"DELETE FROM {TableName} WHERE ObjectID = @ObjectID\";");
            functionBody.AppendLine("                      connection.Open();");
            functionBody.AppendLine("                      using(SqlCommand command = new SqlCommand(query, connection))");
            functionBody.AppendLine("                      {");
            functionBody.AppendLine("                          command.Parameters.AddWithValue(\"@ObjectID\", objectID);");
            functionBody.AppendLine("                          int rowsAffected = command.ExecuteNonQuery();");
            functionBody.AppendLine("                          if (rowsAffected > 0)");
            functionBody.AppendLine("                          {");
            functionBody.AppendLine("                              isDeleted = true;");
            functionBody.AppendLine("                          }");
            functionBody.AppendLine("                      }");
            functionBody.AppendLine("                  }");
            functionBody.AppendLine("              }");
            functionBody.AppendLine("              catch (Exception ex)");
            functionBody.AppendLine("              {");
            functionBody.AppendLine("                  isDeleted = false;");
            functionBody.AppendLine("                  Console.WriteLine($\"An error occurred: {ex.Message}\");");
            functionBody.AppendLine("              }");
            functionBody.AppendLine("              return isDeleted;");
            functionBody.AppendLine("          }");

            return functionBody.ToString();
        }
        
        private static string GenerateObjectFoundByNationalNumberFunction(string tableName, string connectionString)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine($"        public static bool Is{tableName}FoundByNationalNumber(string nationalNumber)");
            functionBody.AppendLine("        {");
            functionBody.AppendLine("            bool isFound = false;");
            functionBody.AppendLine("            try");
            functionBody.AppendLine("            {");
            functionBody.AppendLine($"                using (SqlConnection connection = new SqlConnection(\"{connectionString}\"))");
            functionBody.AppendLine("                {");
            functionBody.AppendLine($"                    using (SqlCommand command = new SqlCommand(\"SP_Is{tableName}FoundByNationalNumber\", connection))");
            functionBody.AppendLine("                    {");
            functionBody.AppendLine("                        command.CommandType = CommandType.StoredProcedure;");
            functionBody.AppendLine("                        command.Parameters.AddWithValue(\"@NationalNumber\", nationalNumber);");
            functionBody.AppendLine("                        connection.Open();");
            functionBody.AppendLine("                        object result = command.ExecuteScalar();");
            functionBody.AppendLine("                        if (result != null)");
            functionBody.AppendLine("                        {");
            functionBody.AppendLine("                            isFound = true;");
            functionBody.AppendLine("                        }");
            functionBody.AppendLine("                    }");
            functionBody.AppendLine("                }");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("            catch (Exception ex)");
            functionBody.AppendLine("            {");
            functionBody.AppendLine("                Console.WriteLine($\"An error occurred: {ex.Message}\");");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("            return isFound;");
            functionBody.AppendLine("        }");

            return functionBody.ToString();
        }

        private static string GenerateGetObjectByFullNameFunction(string tableName, string connectionString, List<clsColumn> arrayOfColumns)
        {
            StringBuilder functionBody = new StringBuilder();

            // Generate function signature
            functionBody.AppendLine($"        public static bool Get{tableName}InfoByFullName(string fullName, ref {string.Join(", ref ", arrayOfColumns.Select(c => $"{GetNullableCSharpDataType(c.DataType, c.IsNull)} {c.ColumnName}"))})");
            functionBody.AppendLine("        {");
            functionBody.AppendLine("            bool isFound = false;");
            functionBody.AppendLine("            try");
            functionBody.AppendLine("            {");
            functionBody.AppendLine($"                using (SqlConnection connection = new SqlConnection(\"{connectionString}\"))");
            functionBody.AppendLine("                {");
            functionBody.AppendLine($"                    using (SqlCommand command = new SqlCommand(\"SP_Get{tableName}InfoByFullName\", connection))");
            functionBody.AppendLine("                    {");
            functionBody.AppendLine("                        command.CommandType = CommandType.StoredProcedure;");
            functionBody.AppendLine("                        command.Parameters.AddWithValue(\"@FullName\", fullName);");
            functionBody.AppendLine("                        connection.Open();");
            functionBody.AppendLine("                        using (SqlDataReader reader = command.ExecuteReader())");
            functionBody.AppendLine("                        {");
            functionBody.AppendLine("                            if (reader.Read())");
            functionBody.AppendLine("                            {");
            functionBody.AppendLine("                                isFound = true;");
            foreach (var column in arrayOfColumns)
            {
                if (column.IsNull == "NULL")
                {
                    functionBody.AppendLine($"                                if (reader[\"{column.ColumnName}\"] != DBNull.Value)");
                    functionBody.AppendLine("                                {");
                    functionBody.AppendLine($"                                    {column.ColumnName} = ({GetCSharpDataType(column.DataType)})reader[\"{column.ColumnName}\"];");
                    functionBody.AppendLine("                                }");
                    functionBody.AppendLine("                                else");
                    functionBody.AppendLine("                                {");
                    functionBody.AppendLine($"                                    {column.ColumnName} = {IfNullGetCSharpDataType(column.DataType)};");
                    functionBody.AppendLine("                                }");
                }
                else
                {
                    functionBody.AppendLine($"                                {column.ColumnName} = ({GetCSharpDataType(column.DataType)})reader[\"{column.ColumnName}\"];");
                }
            }
            functionBody.AppendLine("                            }");
            functionBody.AppendLine("                        }");
            functionBody.AppendLine("                    }");
            functionBody.AppendLine("                }");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("            catch (Exception ex)");
            functionBody.AppendLine("            {");
            functionBody.AppendLine("                Console.WriteLine($\"An error occurred: {ex.Message}\");");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("            return isFound;");
            functionBody.AppendLine("        }");

            return functionBody.ToString();
        }

        private static string GenerateObjectFoundByFullNameFunction(string tableName, string connectionString)
        {
            StringBuilder functionBody = new StringBuilder();

            functionBody.AppendLine($"        public static bool Is{tableName}FoundByFullName(string fullName)");
            functionBody.AppendLine("        {");
            functionBody.AppendLine("            bool isFound = false;");
            functionBody.AppendLine("            try");
            functionBody.AppendLine("            {");
            functionBody.AppendLine($"                using (SqlConnection connection = new SqlConnection(\"{connectionString}\"))");
            functionBody.AppendLine("                {");
            functionBody.AppendLine($"                    using (SqlCommand command = new SqlCommand(\"SP_Is{tableName}FoundByFullName\", connection))");
            functionBody.AppendLine("                    {");
            functionBody.AppendLine("                        command.CommandType = CommandType.StoredProcedure;");
            functionBody.AppendLine("                        command.Parameters.AddWithValue(\"@FullName\", fullName);");
            functionBody.AppendLine("                        connection.Open();");
            functionBody.AppendLine("                        int count = (int)command.ExecuteScalar();");
            functionBody.AppendLine("                        if (count > 0)");
            functionBody.AppendLine("                        {");
            functionBody.AppendLine("                            isFound = true;");
            functionBody.AppendLine("                        }");
            functionBody.AppendLine("                    }");
            functionBody.AppendLine("                }");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("            catch (Exception ex)");
            functionBody.AppendLine("            {");
            functionBody.AppendLine("                Console.WriteLine($\"An error occurred: {ex.Message}\");");
            functionBody.AppendLine("            }");
            functionBody.AppendLine("            return isFound;");
            functionBody.AppendLine("        }");

            return functionBody.ToString();
        }

        public static string GetDataLayerAllFunctionsCode(string tableName, string connectionString, string appName, List<clsColumn> arrayOfColumns)
        {
            StringBuilder finalCodeBuilder = new StringBuilder();
            finalCodeBuilder.Append(GetHeader(tableName, appName));
            finalCodeBuilder.AppendLine(GenerateDataAccessSettingsClass());
            finalCodeBuilder.AppendLine(GenerateRetrievingFunction(tableName, connectionString));
            finalCodeBuilder.AppendLine(GenerateAddingFunction(tableName,  arrayOfColumns) );
            finalCodeBuilder.AppendLine(GenerateUpdatingFunction(tableName, connectionString, arrayOfColumns) );
            finalCodeBuilder.AppendLine(GenerateGetObjectInfoByIDFunction(tableName, connectionString, arrayOfColumns));
            finalCodeBuilder.AppendLine(GenerateGetObjectByNationalNumberFunction(tableName, connectionString, arrayOfColumns));
            finalCodeBuilder.AppendLine(GenerateGetObjectByFullNameFunction(tableName, connectionString, arrayOfColumns) );
            finalCodeBuilder.AppendLine(GenerateDeleteFunction(tableName, connectionString, arrayOfColumns) );
            finalCodeBuilder.AppendLine(GenerateObjectFoundByIDFunction(tableName, connectionString));
            finalCodeBuilder.AppendLine(GenerateObjectFoundByNationalNumberFunction(tableName, connectionString) );
            finalCodeBuilder.AppendLine(GenerateObjectFoundByFullNameFunction(tableName, connectionString));
            finalCodeBuilder.AppendLine("       }");
            finalCodeBuilder.AppendLine("    }");
            return finalCodeBuilder.ToString();
        }
    }
}