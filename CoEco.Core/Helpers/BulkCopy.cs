
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Helpers
{
    public class BulkCopy
    {
        public static void CreateOrUpdate(DataTable dt, string commandText1, string CommandText2, string tempTableName)
        {            
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    var createTempCmd = con.CreateCommand();
                    createTempCmd.CommandText = commandText1;
                    createTempCmd.CommandType = CommandType.Text;
                    createTempCmd.ExecuteNonQuery();
                    using (var bulkCopy = new SqlBulkCopy(con))
                    {
                        bulkCopy.DestinationTableName = tempTableName;
                        bulkCopy.WriteToServer(dt);
                    }

                    var mergeCmd = con.CreateCommand();
                    mergeCmd.CommandText = CommandText2;
                    mergeCmd.CommandType = CommandType.Text;
                    mergeCmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                //Logger.InsertHovalaLog(LogType.Error, "BulkCopy CreateOrUpdate Error", "Insert BulkCopy error to " + tempTableName + " , Message: " + ex.Message);
                throw ex;
            }

        }

    }
}
