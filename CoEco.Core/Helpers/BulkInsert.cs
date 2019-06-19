using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Helpers
{
    public class BulkInsert
    {
        public static bool Copy<T>(List<T> entityCollection, string destinationTableName, string tempTableName, string entityIdForLog)
        {
            bool isSucceed = true;
            var dt = DataReader.CreateDT(entityCollection);
            try
            {
                var commandTextTuple = MapCommandTextsToEntities(destinationTableName, tempTableName);
                BulkCopy.CreateOrUpdate(dt, commandTextTuple.Item1, commandTextTuple.Item2, tempTableName);
            }
            catch (Exception ex)
            {
                //Logger.InsertHovalaLog(LogType.Error, "Insert BulkCopy error to " + destinationTableName + " , " + entityIdForLog, Environment.StackTrace + "Message: " + ex.Message);
                isSucceed = false;
            }
            return isSucceed;
        }

        public static Tuple<string, string> MapCommandTextsToEntities(string destinationTableName, string tempTableName)
        {
            string commandText1 = "";
            string commandText2 = "";

            switch (destinationTableName)
            {
                case "dbo.SYS_TBL_Members":
                    commandText1 = string.Format(@"CREATE TABLE {0} (
	                                    [ID] [int] IDENTITY(1,1),
	                                    [TZ] [nvarchar](9),
	                                    [FirstName] [nvarchar](50),
	                                    [LastName] [nvarchar](50),
	                                    [Email] [nvarchar](50),
	                                    [PhoneNumber] [nvarchar](50),
	                                    [Role] [nvarchar](1000),
	                                    [UnitID] [int],
	                                    [PermissionsProfileID] [int],
	                                    [CreatedOn] [datetime],
	                                    [CreatedBy] [nvarchar](50),
	                                    [UpdatedOn] [datetime],
	                                    [UpdatedBy] [nvarchar](50),
	                                    [Disable] [bit])", tempTableName);
                    commandText2 = string.Format(@"MERGE {0} AS target " +
                            @"USING {1} AS source 
                        ON (target.TZ = source.TZ)  
                        WHEN MATCHED THEN   
                            UPDATE SET FirstName = source.FirstName, LastName = source.LastName, Email = source.Email, PhoneNumber = source.PhoneNumber, Role = source.Role, UnitID = source.UnitID,
                                        PermissionsProfileID = source.PermissionsProfileID, CreatedOn = source.CreatedOn, CreatedBy = source.CreatedBy, UpdatedOn = source.UpdatedOn, UpdatedBy = source.UpdatedBy, 
                                        Disable = source.Disable
                        WHEN NOT MATCHED THEN  
                            INSERT (TZ, FirstName, LastName, Email ,PhoneNumber, Role, UnitID, PermissionsProfileID , CreatedOn, CreatedBy, UpdatedOn, UpdatedBy, Disable)  
                            VALUES (source.TZ ,source.FirstName,source.LastName, source.Email, source.PhoneNumber, source.Role, source.UnitID, source.PermissionsProfileID, source.CreatedOn, source.CreatedBy, source.UpdatedOn, source.UpdatedBy, source.Disable);", destinationTableName, tempTableName);

                    break;
                case "dbo.SYS_TBL_MembersFileError":
                    commandText1 = string.Format(@"CREATE TABLE {0} (
	                [ID] [int] IDENTITY(1,1),
	                [TZ] [nvarchar](50),
	                [FilesUploadName] [nvarchar](max),
	                [ErrorMsg] [nvarchar](max),
	                [CreatedOn] [datetime],
	                [CreatedBy] [nvarchar](100),
	                [UpdatedOn] [datetime],
                    [UpdatedBy] [nvarchar](50),
	                [Disable] [bit])", tempTableName);
                    commandText2 = string.Format(@"MERGE {0} AS target " +
                            @"USING {1} AS source 
                        ON (0 = 1)  
                        WHEN MATCHED THEN   
                            UPDATE SET TZ = source.TZ,  FilesUploadName = source.FilesUploadName, ErrorMsg = source.ErrorMsg, CreatedOn = source.CreatedOn,CreatedBy = source.CreatedBy, UpdatedOn = source.UpdatedOn,  UpdatedBy = source.UpdatedBy, Disable = source.Disable
                        WHEN NOT MATCHED THEN  
                            INSERT (TZ, FilesUploadName, ErrorMsg, CreatedOn, CreatedBy, UpdatedOn , UpdatedBy, Disable)  
                            VALUES (source.TZ, source.FilesUploadName ,source.ErrorMsg, source.CreatedOn, source.CreatedBy, source.UpdatedOn, source.UpdatedBy, source.Disable);", destinationTableName, tempTableName);

                    break;
                default:
                    break;
            }

            return new Tuple<string, string>(commandText1, commandText2);
        }
    }
}
