using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CoEco.BO.ExcelUploadFiles.Helper;
using OfficeOpenXml;
using CoEco.Data;
using static CoEco.BO.ExcelUploadFiles.Enums;
using CoEco.Services.Services;
using CoEco.Services.Models;
using CoEco.Core.Helpers;
using static CoEco.Core.Enums.Enums;
using CoEco.BO.Services;

namespace CoEco.BO.ExcelUploadFiles
{
    public interface IFileHandle
    {
        bool IsFilesAreEqual(FileInfo first, FileInfo second);
        string ReadMembersExcelFile(string uploadedFileName, string basePathDirectory, string fileName, string userIdentity, FileHandleDetails fileHandleDetails);

    }
    public class FileHandle : IFileHandle
    {
        private readonly IDataAccessService _dataAccessService;
        private readonly ILoggerService _loggerService;
        private readonly IFileValidation _fileValidation;
        public FileHandle(IDataAccessService dataAccessService, ILoggerService loggerService, IFileValidation fileValidation)
        {
            _dataAccessService = dataAccessService;
            _loggerService = loggerService;
            _fileValidation = fileValidation;
        }
        public bool IsFilesAreEqual(FileInfo first, FileInfo second)
        {
            int BYTES_TO_READ = sizeof(Int64);
            if (first.Length != second.Length)
                return false;

            if (first.FullName == second.FullName)
                return true;

            int iterations = (int)Math.Ceiling((double)first.Length / BYTES_TO_READ);

            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                byte[] one = new byte[BYTES_TO_READ];
                byte[] two = new byte[BYTES_TO_READ];

                for (int i = 0; i < iterations; i++)
                {
                    fs1.Read(one, 0, BYTES_TO_READ);
                    fs2.Read(two, 0, BYTES_TO_READ);

                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                        return false;
                }
            }

            return true;
        }

        public string ReadMembersExcelFile(string uploadedFileName, string basePathDirectory, string fileName, string userIdentity, FileHandleDetails fileHandleDetails)
        {
            var keyRowValueList = new List<string>();
            var membersTableRowList = new List<Member>();
            var membersFileErrorToBulk = new List<CoecoErrorMemberBulk>();
            var unitsID = _dataAccessService.GetAllUnits().Select(x => x.ID).ToList();
            var permissionsProfilesID = _dataAccessService.GetAllPermissionsProfile().Select(x => x.ID).ToList();

            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(fileName)))
            {
                ExcelWorksheet myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                int totalRows = myWorksheet.Dimension.End.Row;
                int totalColumns = myWorksheet.Dimension.End.Column;
                int startRow = myWorksheet.Dimension.Start.Row;

                List<KeyValuePair<string, string>> errorMeshartimTableRowList = new List<KeyValuePair<string, string>>();
                for (int rowNum = startRow; rowNum <= totalRows; rowNum++) //selet starting row here
                {
                    ExcelRange range = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns];
                    if (range.Any(c => c.Value != null))
                    {



                        try
                        {
                            var errMessage = string.Empty;
                            var TZ = myWorksheet.Cells[rowNum, (int)ExcelMembersCol.TZ].Text;
                            var firstName = myWorksheet.Cells[rowNum, (int)ExcelMembersCol.FirstName].Text;
                            var lastName = myWorksheet.Cells[rowNum, (int)ExcelMembersCol.LastName].Text;
                            var email = myWorksheet.Cells[rowNum, (int)ExcelMembersCol.Email].Text;
                            var phoneNumber = myWorksheet.Cells[rowNum, (int)ExcelMembersCol.PhoneNumber].Text;
                            var role = myWorksheet.Cells[rowNum, (int)ExcelMembersCol.Role].Text;
                            var unitID = myWorksheet.Cells[rowNum, (int)ExcelMembersCol.UnitID].Text;
                            var permissionsProfileID = myWorksheet.Cells[rowNum, (int)ExcelMembersCol.PermissionsProfileID].Text;
                            errMessage = _fileValidation.CheckTZFromExecl(TZ);
                            if (errMessage == string.Empty)
                            {
                                errMessage = _fileValidation.CheckUnitOrProfilePermmisionIsExists(unitID, unitsID, TableName.Unit.ToString());
                            }
                            if (errMessage == string.Empty)
                            {
                                errMessage = _fileValidation.CheckUnitOrProfilePermmisionIsExists(permissionsProfileID, permissionsProfilesID, TableName.PermissionsProfile.ToString());
                            }
                            DateTime now = DateTime.Now;
                            if (errMessage == string.Empty)
                            {

                                keyRowValueList.Add(TZ);
                                Member memberTableRow = new Member
                                {
                                    TZ = TZ,
                                    FirstName = firstName,
                                    LastName = lastName,
                                    Email = email,
                                    PhoneNumber = phoneNumber,
                                    Role = role,
                                    UnitID = int.Parse(unitID),
                                    PermissionsProfileID = int.Parse(permissionsProfileID)
                                };
                                membersTableRowList.Add(memberTableRow);
                            }
                            else
                            {
                                membersFileErrorToBulk.Add(new CoecoErrorMemberBulk()
                                {
                                    CreatedBy = userIdentity,
                                    CreatedOn = now,
                                    Disable = false,
                                    UpdatedBy = userIdentity,
                                    UpdatedOn = now,
                                    ErrorMsg = errMessage,
                                    FilesUploadName = fileName,
                                    TZ = TZ,
                                });
                            }
                        }
                        catch (Exception ex)
                        {

                            _loggerService.InsertLogMessage(LogType.Error.ToString(), $"Failed on ReadMembersExcelFile {ex.ToString()}", userIdentity);
                        }
                    }
                }
            }

            SetDefaultValues(membersTableRowList, userIdentity);


            if (CheckDuplicated(keyRowValueList) == false)
            {


                var membersToInsertBulk = membersTableRowList.ToList().Select(d => new CoecoMemberBulk
                {
                    ID = d.ID,
                    TZ = d.TZ,
                    Email = d.Email,
                    Role = d.Role,
                    PermissionsProfileID = d.PermissionsProfileID,
                    UnitID = d.UnitID,
                    PhoneNumber = d.PhoneNumber,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy,
                    UpdatedOn = d.UpdatedOn,
                    UpdatedBy = d.UpdatedBy,
                    Disable = d.Disable,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                }).ToList();

                bool isMeshartimUploadSuceed = true;
                if (membersToInsertBulk.Any())
                {
                    isMeshartimUploadSuceed = BulkInsert.Copy<CoecoMemberBulk>(membersToInsertBulk, "dbo.SYS_TBL_Members", "#Members", "Members added: ");
                }

                if (membersFileErrorToBulk.Any())
                {
                    BulkInsert.Copy<CoecoErrorMemberBulk>(membersFileErrorToBulk, "dbo.SYS_TBL_MembersFileError", "#MembersFileError", "MembersFileError added: ");
                }

                string messageString = FileReturnMsg.FileWasHandle.ToString();
                if (!(isMeshartimUploadSuceed))
                {
                    messageString = FileReturnMsg.MainUploadFileFailed.ToString();
                }
                return messageString;
            }
            else return FileReturnMsg.DuplicatedRowsProblem.ToString();
        }

        private void SetDefaultValues(List<Member> meshartimTableRowList, string userIdentity)
        {
            meshartimTableRowList.ForEach(delegate (Member meshartimTableRow)
            {
                meshartimTableRow.Disable = false;
                meshartimTableRow.CreatedOn = DateTime.Now;
                meshartimTableRow.CreatedBy = userIdentity;
                meshartimTableRow.UpdatedOn = DateTime.Now;
                meshartimTableRow.UpdatedBy = userIdentity;
            });
        }

        private bool CheckDuplicated(List<string> keyRowValueList)
        {
            int beforeDistinctCount = keyRowValueList.Count;
            int afterDistincrCount = keyRowValueList.Distinct().ToList().Count;
            return beforeDistinctCount == afterDistincrCount ? false : true;
        }

    }
}