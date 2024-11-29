using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static Code_DataBase_SibaSbahi_Data.Column_Data;

namespace Code_DataBase_SibaSbahi_Data
    {
        public class CreateStoredProcedures_Data
        {
            public static string connectionString = "Server=.;Database=master; User Id=sa ;Password = sa123456;";

            public static bool CreateAddingObjectStoredProcedure(List<string> ColumnsList, string DataBaseName, string TableName)
            {
                bool IsCreated = false;
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();

                        // Extract column names, parameters, and values using LINQ, skipping the first element
                        var columns = string.Join(", ", ColumnsList.Skip(1).Select(c => c.Split(' ')[0]));
                        var parameters = string.Join(", ", ColumnsList.Skip(1).Select(c =>
                        {
                            var parts = c.Split(' ');
                            return $"@{parts[0]} {parts[1]}";
                        }));
                        var values = string.Join(", ", ColumnsList.Skip(1).Select(c => $"@{c.Split(' ')[0]}"));

                        // Debugging output
                        Console.WriteLine("Columns: " + columns);
                        Console.WriteLine("Parameters: " + parameters);
                        Console.WriteLine("Values: " + values);

                        // Split the SQL commands into separate statements
                        string useDatabase = $@"USE [{DataBaseName}];";
                        string dropProcedure = $@"
        IF OBJECT_ID('dbo.SP_Add{TableName}', 'P') IS NOT NULL
            DROP PROCEDURE dbo.SP_Add{TableName};";
                        string createProcedure = $@"
        CREATE PROCEDURE dbo.SP_Add{TableName}
            @NewID INT OUTPUT,
            {parameters}
        AS
        BEGIN
            INSERT INTO {TableName} ({columns})
            VALUES ({values});
            SET @NewID = SCOPE_IDENTITY();
        END;";

                        // Execute each statement separately
                        using (SqlCommand useCommand = new SqlCommand(useDatabase, con))
                        {
                            useCommand.ExecuteNonQuery();
                        }

                        using (SqlCommand dropCommand = new SqlCommand(dropProcedure, con))
                        {
                            dropCommand.ExecuteNonQuery();
                        }

                        using (SqlCommand createCommand = new SqlCommand(createProcedure, con))
                        {
                            createCommand.ExecuteNonQuery();
                            IsCreated = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
                return IsCreated;
            }

            public static bool CreateUpdatingObjectStoredProcedures(List<string> ColumnsList, string DataBaseName, string TableName)
            {
                bool IsCreated = false;
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();

                        // Assuming the first column in the list is the primary key
                        string primaryKey = ColumnsList[0].Split(' ')[0];
                        string primaryKeyDataType = ColumnsList[0].Split(' ')[1];

                        // Skip the primary key for the SET clause and parameters
                        string setClause = string.Join(", ", ColumnsList.Skip(1).Select(c => $"{c.Split(' ')[0]} = @{c.Split(' ')[0]}"));
                        string parameters = string.Join(", ", ColumnsList.Skip(1).Select(c =>
                        {
                            var parts = c.Split(' ');
                            return $"@{parts[0]} {parts[1]}";
                        }));

                        // Execute the USE statement separately
                        string useDatabase = $@"USE [{DataBaseName}];";
                        using (SqlCommand useCommand = new SqlCommand(useDatabase, con))
                        {
                            useCommand.ExecuteNonQuery();
                        }

                        // Drop the existing procedure if it exists
                        string dropProcedure = $@"IF OBJECT_ID('dbo.Update{TableName}', 'P') IS NOT NULL DROP PROCEDURE dbo.Update{TableName};";
                        using (SqlCommand dropCommand = new SqlCommand(dropProcedure, con))
                        {
                            dropCommand.ExecuteNonQuery();
                        }

                        // SQL query to create the stored procedure
                        string createProcedure = $@"
                CREATE PROCEDURE dbo.Update{TableName}
                    @{primaryKey} {primaryKeyDataType}, {parameters}
                AS
                BEGIN
                    UPDATE {TableName}
                    SET {setClause}
                    WHERE {primaryKey} = @{primaryKey};
                END;";

                        // Execute the create procedure command
                        using (SqlCommand createCommand = new SqlCommand(createProcedure, con))
                        {
                            createCommand.ExecuteNonQuery();
                            IsCreated = true;
                        }
                    }
                }
                catch (Exception ex) { }
                return IsCreated;
            }
            public static bool CreateDeletingObjectStoredProcedures(List<string> ColumnsList, string DataBaseName, string TableName)
            {
                bool IsCreated = false;
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string columnName = ColumnsList[0].Split(' ')[0];

                        // Separate the USE statement
                        string useDatabase = $@"USE [{DataBaseName}];";
                        using (SqlCommand useCommand = new SqlCommand(useDatabase, con))
                        {
                            useCommand.ExecuteNonQuery();
                        }

                        // Separate the IF OBJECT_ID statement
                        string dropProcedure = $@"IF OBJECT_ID('dbo.Delete{TableName}', 'P') IS NOT NULL DROP PROCEDURE dbo.Delete{TableName};";
                        using (SqlCommand dropCommand = new SqlCommand(dropProcedure, con))
                        {
                            dropCommand.ExecuteNonQuery();
                        }

                        // Create the procedure
                        string createProcedure = $@"
                CREATE PROCEDURE dbo.Delete{TableName}
                    @{columnName} INT
                AS
                BEGIN
                    DELETE FROM {TableName}
                    WHERE {columnName} = @{columnName};
                END;";
                        using (SqlCommand createCommand = new SqlCommand(createProcedure, con))
                        {
                            createCommand.ExecuteNonQuery();
                            IsCreated = true;
                        }
                    }
                }
                catch (Exception ex) { }
                return IsCreated;
            }
            public static bool CreateReadingObjectByIDStoredProcedures(List<string> ColumnsList, string DataBaseName, string TableName)
            {
                bool IsCreated = false;
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string columnName = ColumnsList[0].Split(' ')[0];

                        // Execute the USE statement separately
                        string useDatabase = $@"USE [{DataBaseName}];";
                        using (SqlCommand useCommand = new SqlCommand(useDatabase, con))
                        {
                            useCommand.ExecuteNonQuery();
                        }

                        // Drop the existing procedure if it exists
                        string dropProcedure = $@"IF OBJECT_ID('dbo.Get{TableName}ById', 'P') IS NOT NULL DROP PROCEDURE dbo.Get{TableName}ById;";
                        using (SqlCommand dropCommand = new SqlCommand(dropProcedure, con))
                        {
                            dropCommand.ExecuteNonQuery();
                        }

                        // Create the procedure
                        string createProcedure = $@"
                CREATE PROCEDURE dbo.Get{TableName}ById
                    @{columnName} INT
                AS
                BEGIN
                    SELECT * FROM {TableName}
                    WHERE {columnName} = @{columnName};
                END;";
                        using (SqlCommand createCommand = new SqlCommand(createProcedure, con))
                        {
                            createCommand.ExecuteNonQuery();
                            IsCreated = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                return IsCreated;
            }
            public static bool CreateReadingObjectByNationalNoStoredProcedures(List<string> ColumnsList, string DataBaseName, string TableName)
            {
                bool IsCreated = false;
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string nationalNoColumn = ColumnsList.First(c => c.Contains("NationalNumber")).Split(' ')[0];

                        // Execute the USE statement separately
                        string useDatabase = $@"USE [{DataBaseName}];";
                        using (SqlCommand useCommand = new SqlCommand(useDatabase, con))
                        {
                            useCommand.ExecuteNonQuery();
                        }

                        // Drop the existing procedure if it exists
                        string dropProcedure = $@"IF OBJECT_ID('dbo.Get{TableName}ByNationalNo', 'P') IS NOT NULL DROP PROCEDURE dbo.Get{TableName}ByNationalNo;";
                        using (SqlCommand dropCommand = new SqlCommand(dropProcedure, con))
                        {
                            dropCommand.ExecuteNonQuery();
                        }

                        // Create the procedure
                        string createProcedure = $@"
                CREATE PROCEDURE dbo.Get{TableName}ByNationalNo
                    @NationalNo NVARCHAR(MAX)
                AS
                BEGIN
                    SELECT * FROM {TableName}
                    WHERE {nationalNoColumn} = @NationalNo;
                END;";
                        using (SqlCommand createCommand = new SqlCommand(createProcedure, con))
                        {
                            createCommand.ExecuteNonQuery();
                            IsCreated = true;
                        }
                    }
                }
                catch (Exception ex) { }
                return IsCreated;
            }
            public static bool CreateReadingObjectByFullNameStoredProcedures(List<string> ColumnsList, string DataBaseName, string TableName)
            {
                bool IsCreated = false;
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string firstNameColumn = ColumnsList.First(c => c.Contains("FirstName")).Split(' ')[0];
                        string lastNameColumn = ColumnsList.First(c => c.Contains("LastName")).Split(' ')[0];

                        // Execute the USE statement separately
                        string useDatabase = $@"USE [{DataBaseName}];";
                        using (SqlCommand useCommand = new SqlCommand(useDatabase, con))
                        {
                            useCommand.ExecuteNonQuery();
                        }

                        // Drop the existing procedure if it exists
                        string dropProcedure = $@"IF OBJECT_ID('dbo.Get{TableName}ByFullName', 'P') IS NOT NULL DROP PROCEDURE dbo.Get{TableName}ByFullName;";
                        using (SqlCommand dropCommand = new SqlCommand(dropProcedure, con))
                        {
                            dropCommand.ExecuteNonQuery();
                        }

                        // Create the procedure
                        string createProcedure = $@"
                CREATE PROCEDURE dbo.Get{TableName}ByFullName
                    @FullName NVARCHAR(MAX)
                AS
                BEGIN
                    SELECT * FROM {TableName}
                    WHERE {firstNameColumn} + ' ' + {lastNameColumn} = @FullName;
                END;";
                        using (SqlCommand createCommand = new SqlCommand(createProcedure, con))
                        {
                            createCommand.ExecuteNonQuery();
                            IsCreated = true;
                        }
                    }
                }
                catch (Exception ex) { }
                return IsCreated;
            }
            public static bool CreateAllRecordsStoredProcedures(List<string> ColumnsList, string DataBaseName, string TableName)
            {
                bool IsCreated = false;
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();

                        // Execute the USE statement separately
                        string useDatabase = $@"USE [{DataBaseName}];";
                        using (SqlCommand useCommand = new SqlCommand(useDatabase, con))
                        {
                            useCommand.ExecuteNonQuery();
                        }

                        // Drop the existing procedure if it exists
                        string dropProcedure = $@"IF OBJECT_ID('dbo.GetAll{TableName}', 'P') IS NOT NULL DROP PROCEDURE dbo.GetAll{TableName};";
                        using (SqlCommand dropCommand = new SqlCommand(dropProcedure, con))
                        {
                            dropCommand.ExecuteNonQuery();
                        }

                        // Create the procedure
                        string createProcedure = $@"
                CREATE PROCEDURE dbo.GetAll{TableName}
                AS
                BEGIN
                    SELECT * FROM {TableName};
                END;";
                        using (SqlCommand createCommand = new SqlCommand(createProcedure, con))
                        {
                            createCommand.ExecuteNonQuery();
                            IsCreated = true;
                        }
                    }
                }
                catch (Exception ex) { }
                return IsCreated;
            }
            public static bool CreateIsObjectFoundByNationalNumberStoredProcedures(List<string> ColumnsList, string DataBaseName, string TableName)
            {
                bool IsCreated = false;
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string nationalNoColumn = ColumnsList.First(c => c.Contains("NationalNumber")).Split(' ')[0];

                        // Execute the USE statement separately
                        string useDatabase = $@"USE [{DataBaseName}];";
                        using (SqlCommand useCommand = new SqlCommand(useDatabase, con))
                        {
                            useCommand.ExecuteNonQuery();
                        }

                        // Drop the existing procedure if it exists
                        string dropProcedure = $@"IF OBJECT_ID('dbo.Is{TableName}FoundByNationalNo', 'P') IS NOT NULL DROP PROCEDURE dbo.Is{TableName}FoundByNationalNo;";
                        using (SqlCommand dropCommand = new SqlCommand(dropProcedure, con))
                        {
                            dropCommand.ExecuteNonQuery();
                        }

                        // Create the procedure
                        string createProcedure = $@"
                CREATE PROCEDURE dbo.Is{TableName}FoundByNationalNo
                    @NationalNo NVARCHAR(MAX)
                AS
                BEGIN
                    SELECT 1 FROM {TableName}
                    WHERE {nationalNoColumn} = @NationalNo;
                END;";
                        using (SqlCommand createCommand = new SqlCommand(createProcedure, con))
                        {
                            createCommand.ExecuteNonQuery();
                            IsCreated = true;
                        }
                    }
                }
                catch (Exception ex) { }
                return IsCreated;
            }
            public static bool CreateIsObjectFoundByIDStoredProcedures(List<string> ColumnsList, string DataBaseName, string TableName)
            {
                bool IsCreated = false;
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string idColumn = ColumnsList.First(c => c.Contains("ID")).Split(' ')[0];

                        // Execute the USE statement separately
                        string useDatabase = $@"USE [{DataBaseName}];";
                        using (SqlCommand useCommand = new SqlCommand(useDatabase, con))
                        {
                            useCommand.ExecuteNonQuery();
                        }

                        // Drop the existing procedure if it exists
                        string dropProcedure = $@"IF OBJECT_ID('dbo.Is{TableName}FoundByID', 'P') IS NOT NULL DROP PROCEDURE dbo.Is{TableName}FoundByID;";
                        using (SqlCommand dropCommand = new SqlCommand(dropProcedure, con))
                        {
                            dropCommand.ExecuteNonQuery();
                        }

                        // Create the procedure
                        string createProcedure = $@"
                CREATE PROCEDURE dbo.Is{TableName}FoundByID
                    @{idColumn} INT
                AS
                BEGIN
                    SELECT 1 FROM {TableName}
                    WHERE {idColumn} = @{idColumn};
                END;";
                        using (SqlCommand createCommand = new SqlCommand(createProcedure, con))
                        {
                            createCommand.ExecuteNonQuery();
                            IsCreated = true;
                        }
                    }
                }
            catch (Exception ex) { }
            return IsCreated;
        }
    }
}