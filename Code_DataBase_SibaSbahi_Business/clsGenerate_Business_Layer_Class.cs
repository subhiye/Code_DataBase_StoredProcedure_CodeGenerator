using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Code_DataBase_SibaSbahi_Business
{
    public class clsGenerate_Business_Layer_Class
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
                case "smallint":
                case "decimal":
                    return "-1";
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                    return "\"\"";
                case "datetime":
                case "date":
                case "smalldatetime":
                    return "DateTime.Now";
                default:
                    return "";
            }
        }
        private static string GetHeader(string ClassName, string AppName)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine("using System;");
            functionBody.AppendLine("using System.Collections.Generic;");
            functionBody.AppendLine("using System.IO;");
            functionBody.AppendLine("using System.Data;");
            functionBody.AppendLine("using System.Linq;");
            functionBody.AppendLine("using System.Text;");
            functionBody.AppendLine($"namespace {AppName}_Business");
            functionBody.AppendLine("{");
            functionBody.AppendLine($"    public class cls{ClassName}");
            functionBody.AppendLine("    {");
            return functionBody.ToString();
        }
        public static void CreateClassFile(string ClassCode, string className, string filePath)
        {
            string fullPath = Path.Combine(filePath, $"{className}.cs");
            File.WriteAllText(fullPath, ClassCode);
        }
        private static string GenerateObjectClass(string ClassName, List<clsColumn> ArrayOfColumns)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine("        public enum enMode { AddNew = 0, Update = 1 };");
            functionBody.AppendLine("        public enMode Mode = enMode.AddNew;");
            foreach (var column in ArrayOfColumns)
            {
                functionBody.AppendLine($"        public {GetCSharpDataType(column.DataType)} {column.ColumnName} {{ set; get; }}");
            }

            functionBody.AppendLine($"        public cls{ClassName}()");
            functionBody.AppendLine("        {");
            foreach (var column in ArrayOfColumns)
            {
                functionBody.AppendLine($"             this.{column.ColumnName} = default({GetCSharpDataType(column.DataType)});");
            }
            functionBody.AppendLine("             Mode = enMode.AddNew;");
            functionBody.AppendLine("        }");
            // Parameterized constructor
            functionBody.AppendLine($"        public cls{ClassName}({string.Join(", ", ArrayOfColumns.Select(c => $"{GetCSharpDataType(c.DataType)} {c.ColumnName}"))})");
            functionBody.AppendLine("        {");
            foreach (var column in ArrayOfColumns)
            {
                functionBody.AppendLine($"             this.{column.ColumnName} = {column.ColumnName};");
            }
            functionBody.AppendLine("             Mode = enMode.Update;");
            functionBody.AppendLine("        }");
            functionBody.AppendLine();
            return functionBody.ToString();
        }
        private static string GenerateBusinessLayerAdding(string className, List<clsColumn> ArrayOfColumns)
        {
            string ID = "";
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine($"        private bool _AddNew{className}()");
            functionBody.AppendLine("        {");

            // Extract the primary key ID from the first column
            if (ArrayOfColumns.Count > 0)
            {
                ID = ArrayOfColumns[0].ColumnName;
            }
            // Build the parameter list for the function call
            functionBody.AppendLine($"            this.{ID} = {className}_Data.AddNew{className}(");
            for (short i = 1; i < ArrayOfColumns.Count; i++)
            {
                functionBody.AppendLine($"            this.{ArrayOfColumns[i].ColumnName},");
            }

            // Remove the trailing comma and newline
            functionBody.Length -= 3;

            functionBody.AppendLine("    );");
            functionBody.AppendLine($"            return (this.{ID} != -1);");
            functionBody.AppendLine("        }");

            return functionBody.ToString();
        }
        private static string GenerateBusinessLayerUpdating(string className, List<clsColumn> ArrayOfColumns)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine($"        private bool _Update{className}()");
            functionBody.AppendLine("        {");
            functionBody.AppendLine($"            return {className}_Data.Update{className}(");
            foreach (clsColumn column in ArrayOfColumns)
            {
                functionBody.AppendLine($"            this.{column.ColumnName},");
            }
            functionBody.Length -= 3;
            functionBody.AppendLine(");");
            functionBody.AppendLine("        }");
            functionBody.AppendLine();
            return functionBody.ToString();
        }
        private static string BusinessLayerAll(string className)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine($"        public static DataTable GetAll{className}()");
            functionBody.AppendLine("        {");
            functionBody.AppendLine($"            return {className}_Data.GetAll{className}();");
            functionBody.AppendLine("        }");
            return functionBody.ToString();
        }
        private static string BusinessLayerDeleting(string className)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine($"        public static bool Delete{className}(int ID)");
            functionBody.AppendLine("        {");
            functionBody.AppendLine($"            return {className}_Data.Delete{className}(ID);");
            functionBody.AppendLine("        }");
            return functionBody.ToString();
        }
        private static string GenerateSaveFunction(string ClassName)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine("        public bool Save()");
            functionBody.AppendLine("        {");
            functionBody.AppendLine("            switch (Mode)");
            functionBody.AppendLine("            {");
            functionBody.AppendLine($"               case enMode.AddNew:");
            functionBody.AppendLine($"                   if (_AddNew{ClassName}())");
            functionBody.AppendLine("                   {");
            functionBody.AppendLine("                       Mode = enMode.Update;");
            functionBody.AppendLine("                       return true;");
            functionBody.AppendLine("                   }");
            functionBody.AppendLine("                   else");
            functionBody.AppendLine("                   {");
            functionBody.AppendLine("                       return false;");
            functionBody.AppendLine("                   }");
            functionBody.AppendLine($"               case enMode.Update:");
            functionBody.AppendLine($"                   return _Update{ClassName}();");
            functionBody.AppendLine("           }");
            functionBody.AppendLine("           return false;");
            functionBody.AppendLine("        }");
            return functionBody.ToString();
        }
        private static string GenerateIsObjectExistByIDFunction(string ClassName)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine($"        public static bool Is{ClassName}FoundByID(int ID)");
            functionBody.AppendLine("        {");
            functionBody.AppendLine($"            return {ClassName}_Data.Is{ClassName}FoundByID(ID);");
            functionBody.AppendLine("        }");
            return functionBody.ToString();
        }
        private static string GenerateIsObjectExistByNationalNoFunction(string ClassName)
        {
            StringBuilder functionBody = new StringBuilder();
            functionBody.AppendLine($"        public static bool Is{ClassName}FoundByNationalNumber(string NationalNo)");
            functionBody.AppendLine("        {");
            functionBody.AppendLine($"            return {ClassName}_Data.Is{ClassName}FoundByNationalNumber(NationalNo);");
            functionBody.AppendLine("        }");
            return functionBody.ToString();
        }
        private static string GenerateFindFunction(string className, List<clsColumn> ArrayOfColumns)
        {
            StringBuilder variables = new StringBuilder();
            StringBuilder functionBody = new StringBuilder();
            StringBuilder refParameters = new StringBuilder();
            StringBuilder constructorParameters = new StringBuilder();
            functionBody.AppendLine($"        public static cls{className} Find(int {className}ID)");
            functionBody.AppendLine("        {");
            for (int i = 1; i < ArrayOfColumns.Count; i++)
            {
                string dataType = GetCSharpDataType(ArrayOfColumns[i].DataType);
                string defaultValue = dataType == "string" ? "\"\"" : dataType == "DateTime" ? "DateTime.MinValue" :
                                     dataType == "int" ? "-1" : "default";
                variables.AppendLine($"            {dataType} {ArrayOfColumns[i].ColumnName} = {defaultValue};");
                refParameters.AppendLine($"            ref {ArrayOfColumns[i].ColumnName},");
            }
            for (short i = 0; i<ArrayOfColumns.Count; i++)
            {
                constructorParameters.AppendLine($"            {ArrayOfColumns[i].ColumnName},");
            }
            functionBody.Append(variables.ToString());
            functionBody.AppendLine($"            bool IsFound = {className}_Data.Get{className}InfoByID(");
            functionBody.Append($"            {className}ID,");
            functionBody.Append(refParameters.ToString());
            functionBody.Length -= 3;
            functionBody.AppendLine("            );");
            functionBody.AppendLine("            if (IsFound)");
            functionBody.AppendLine("            {");
            functionBody.AppendLine($"                return new cls{className}(");
            functionBody.Append(constructorParameters.ToString());
            functionBody.Length -= 3;
            functionBody.AppendLine("               );");
            functionBody.AppendLine("           }");
            functionBody.AppendLine("           else");
            functionBody.AppendLine("           {");
            functionBody.AppendLine("               return null;");
            functionBody.AppendLine("           }");
            functionBody.AppendLine("        }");
            return functionBody.ToString();
        }

        public static string GenerateAllBusinessLayerMethods(string ClassName, List<clsColumn> arrayOfColumns)
        {
            string Header = GetHeader(ClassName, ClassName);
            string generateObject = GenerateObjectClass(ClassName, arrayOfColumns);
            string adding = GenerateBusinessLayerAdding(ClassName, arrayOfColumns);
            string updating = GenerateBusinessLayerUpdating(ClassName, arrayOfColumns);
            string finding = GenerateFindFunction(ClassName, arrayOfColumns);
            string deleting = BusinessLayerDeleting(ClassName);
            string gettingAll = BusinessLayerAll(ClassName);
            string save = GenerateSaveFunction(ClassName);
            string isFoundByID = GenerateIsObjectExistByIDFunction(ClassName);
            string isFoundByNationalNo = GenerateIsObjectExistByNationalNoFunction(ClassName);
            StringBuilder fullCode = new StringBuilder();
            fullCode.AppendLine(Header);
            fullCode.AppendLine(generateObject);
            fullCode.AppendLine(adding);
            fullCode.AppendLine(updating);
            fullCode.AppendLine(finding);
            fullCode.AppendLine(deleting);
            fullCode.AppendLine(gettingAll);
            fullCode.AppendLine(save);
            fullCode.AppendLine(isFoundByID);
            fullCode.AppendLine(isFoundByNationalNo);
            fullCode.AppendLine("   }");
            fullCode.AppendLine("}");
            return fullCode.ToString();
        }
    }
}