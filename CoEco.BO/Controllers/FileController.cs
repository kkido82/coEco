using Breeze.ContextProvider.EF6;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using CoEco.Data;
using CoEco.Services.Services;
using CoEco.BO.Services;
using CoEco.Services;
using CoEco.BO.ExcelUploadFiles;
using CoEco.BO.ExcelUploadFiles.Helper;
using static CoEco.BO.ExcelUploadFiles.Enums;
using static CoEco.Core.Enums.Enums;

namespace CoEco.BO.Controllers
{
    public class FileController : Controller
    {

        private readonly IDownloadFileService _downloadFileService;
        private readonly IAuthRepository _authRepository;
        private readonly IFileService _fileService;
        private readonly IDataAccessService _dataAccessService;
        private readonly IFileHandle _fileHandle;
        private readonly ILoggerService _loggerService;
        public FileController(IDownloadFileService downloadFileService, IAuthRepository authRepository,
            IDataAccessService dataAccessService, IFileService fileService, IFileHandle fileHandle, ILoggerService loggerService)
        {
            _downloadFileService = downloadFileService;
            _authRepository = authRepository;
            _dataAccessService = dataAccessService;
            _fileService = fileService;
            _fileHandle = fileHandle;
            _loggerService = loggerService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetUsersAdminExport()
        {
            var result = await _authRepository.List(string.Empty, string.Empty, string.Empty, string.Empty, 0, 100);
            var usersAdmin = result.Users;

            using (var pck = new ExcelPackage())
            {
                var ws = pck.Workbook.Worksheets.Add("UsersAdmin");

                var headings = new[] { "שם פרטי", "שם משפחה", "מייל", "טלפון", "תעודת זהות", "פעיל" };

                for (var i = 0; i < headings.Count(); i++)
                {
                    ws.Cells[1, i + 1].Value = headings[i];
                }

                for (var i = 0; i < usersAdmin.Count; i++)
                {
                    var user = usersAdmin[i];

                    for (var j = 0; j < headings.Count(); j++)
                    {
                        ws.Cells[i + 2, 1].Value = user.FirstName;
                        ws.Cells[i + 2, 2].Value = user.LastName;
                        ws.Cells[i + 2, 3].Value = user.Email;
                        ws.Cells[i + 2, 4].Value = user.PhoneNumber;
                        ws.Cells[i + 2, 5].Value = user.UserName;
                        ws.Cells[i + 2, 6].Value = $"{(user.Disabled ? "לא" : "כן")}";
                    }
                }

                using (var rng = ws.Cells["A1:BZ1"])
                {
                    rng.Style.Font.Bold = true;
                }

                var ms = new System.IO.MemoryStream();
                pck.SaveAs(ms);

                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UsersAdmin.xlsx");
            }

        }
        [HttpPost]
        [Authorize]
        public ActionResult UploadExcelMembersFile(string fileType)
        {

            Dictionary<int, string> returnMsgDictinary = new Dictionary<int, string>();
            FileHandleDetails fileHandleDetails = new FileHandleDetails();
            SetDictinaryReturnMsg(returnMsgDictinary);
            var context = System.Web.HttpContext.Current.Request;
            string fileName = null;
            string pathDirectory = null;
            string validateMsg;

            //Core.Services.Logger.InsertHovalaLog(LogType.Debug, "UploadExcelhovalaFile", System.Web.HttpContext.Current.User.Identity.Name);

            if (context.Files != null && context.Files.Count > 0)
            {
                try
                {
                    HttpPostedFile file = context.Files["file"];
                    if (file != null)
                    {
                        //This one take a lot of time
                        if (Enum.IsDefined(typeof(ExcelFileType), fileType) == false)
                        {
                            fileHandleDetails.FileUploadMsg = returnMsgDictinary[(int)FileReturnMsg.FileTypeIncorrect];
                            return Content(new JavaScriptSerializer().Serialize(fileHandleDetails));
                        }
                        if (CheckIsUploadFileExcelFormat(file) == false)
                        {
                            fileHandleDetails.FileUploadMsg = returnMsgDictinary[(int)FileReturnMsg.FileExtensionIncorrect];
                            return Content(new JavaScriptSerializer().Serialize(fileHandleDetails));
                        }
                        //If file is OK saving it to folder 
                        switch ((ExcelFileType)Enum.Parse(typeof(ExcelFileType), fileType))
                        {
                            case ExcelFileType.CoecoMembers:
                                pathDirectory = ConfigurationManager.AppSettings["SaveUploadedMembersFilePath"];
                                break;
                        }


                        fileName = CheckIsFilesSameByByteAndSave(fileType, file, pathDirectory);

                        if (fileName == null)
                        {
                            fileName = SaveUploadExcelFile(System.Web.HttpContext.Current.User.Identity.Name, file, pathDirectory, fileType);
                        }

                        validateMsg = _fileHandle.ReadMembersExcelFile(file.FileName, pathDirectory,fileName, System.Web.HttpContext.Current.User.Identity.Name,fileHandleDetails);

                        if (validateMsg == FileReturnMsg.DuplicatedRowsProblem.ToString())
                        {
                            return SetDataForReturnAndDeleteFileWithLog(returnMsgDictinary[(int)FileReturnMsg.DuplicatedRowsProblem], fileHandleDetails, false, true, fileName);
                        }
                        else if (validateMsg == FileReturnMsg.FileWasHandle.ToString())
                        {
                            validateMsg = returnMsgDictinary[(int)FileReturnMsg.FileWasHandle];
                        }
                        else if (validateMsg == FileReturnMsg.MainUploadFileFailed.ToString())
                        {
                            validateMsg = returnMsgDictinary[(int)FileReturnMsg.MainUploadFileFailed];
                        }

                        return SetDataForReturnAndDeleteFileWithLog(validateMsg, fileHandleDetails, true);

                    }


                    else
                    {
                        return SetDataForReturnAndDeleteFileWithLog(returnMsgDictinary[(int)FileReturnMsg.NoFileToUpload], fileHandleDetails, false);
                    }
                }
                catch (Exception ex)
                {
                    _loggerService.InsertLogMessage(LogType.Error.ToString(),$"Failed on UploadExcelMembersFile {ex.ToString()}", System.Web.HttpContext.Current.User.Identity.Name);
                    return SetDataForReturnAndDeleteFileWithLog(returnMsgDictinary[(int)FileReturnMsg.ErrorUploadFile], fileHandleDetails, false, true, fileName);
                }
            }
            return SetDataForReturnAndDeleteFileWithLog(returnMsgDictinary[(int)FileReturnMsg.NoFileToUpload], fileHandleDetails, false);

        }
        private void SetDictinaryReturnMsg(Dictionary<int, string> returnMsgDictinary)
        {
            returnMsgDictinary.Add((int)FileReturnMsg.DuplicatedRowsProblem, "קובץ כולל שורות כפולות");
            returnMsgDictinary.Add((int)FileReturnMsg.NewFile, "קובץ חדש האם להמשיך?");
            returnMsgDictinary.Add((int)FileReturnMsg.FileSameContentIsContinue, "קובץ עם תוכן זהה כבר קיים האם להמשיך?");
            returnMsgDictinary.Add((int)FileReturnMsg.FileSameContentError, "קובץ עם תוכן זהה כבר קיים!");
            returnMsgDictinary.Add((int)FileReturnMsg.FileWasHandle, "קובץ התקבל וטופל!");
            returnMsgDictinary.Add((int)FileReturnMsg.FileExtensionIncorrect, "סיומת קובץ לא תקין!");
            returnMsgDictinary.Add((int)FileReturnMsg.ShowErrorList, "קיימות שגיאות בקובץ, לצפייה לחץ על הקישור או אשר העלאה.");
            returnMsgDictinary.Add((int)FileReturnMsg.NoFileToUpload, "אין קובץ להעלאה!");
            returnMsgDictinary.Add((int)FileReturnMsg.ErrorUploadFile, "שגיאה בהעלאת קובץ!");
            returnMsgDictinary.Add((int)FileReturnMsg.FileNotFound, "לא נמצא קובץ להעלאה!");
            returnMsgDictinary.Add((int)FileReturnMsg.FileTypeIncorrect, "סוג קובץ לא קיים!");
            returnMsgDictinary.Add((int)FileReturnMsg.ErrorUploadFileFailed, " שים לב, טעינת קובץ שגיאות נכשלה. ");
            returnMsgDictinary.Add((int)FileReturnMsg.MainUploadFileFailed, "קובץ התקבל וטופל. שים לב, קיימת שגיאה בעלית הקובץ לבסיס נתונים. ");
            returnMsgDictinary.Add((int)FileReturnMsg.IncorrectTZ, "תז לא תקינה!");
            returnMsgDictinary.Add((int)FileReturnMsg.FileStructureIsInvalid, "מבנה הקובץ לא תקין");
        }

        private bool CheckIsUploadFileExcelFormat(HttpPostedFile file)
        {
            bool isFileExcelExtention = false;
            string userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            List<string> filesExtensions = new List<string>() { "xlsx" };
            string FileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
            if (filesExtensions.Contains(FileExtension))
            {
                isFileExcelExtention = true;
            }
            return isFileExcelExtention;
        }
        private string CheckIsFilesSameByByteAndSave(string fileType, HttpPostedFile file, string pathDirectory)
        {
            bool isFilesSame = false;
            string pathOfSavedFile = null; //no file saved

            DirectoryInfo saveDirectory = new DirectoryInfo(pathDirectory);
            if (!Directory.Exists(pathDirectory))
                Directory.CreateDirectory(pathDirectory);
            FileInfo[] allFilesInDirectoryBeforeUpload = saveDirectory.GetFiles();
            pathOfSavedFile = SaveUploadExcelFile(System.Web.HttpContext.Current.User.Identity.Name, file, pathDirectory, fileType);
            FileInfo uploadedFile = GetFileInfoFromDisk(pathOfSavedFile);
            foreach (var fileToCheckIsEqual in allFilesInDirectoryBeforeUpload)
            {
                isFilesSame = _fileHandle.IsFilesAreEqual(uploadedFile, fileToCheckIsEqual);
                if (isFilesSame)
                {
                    System.IO.File.Delete(pathOfSavedFile);
                    pathOfSavedFile = null;
                    break;
                }
            }
            return pathOfSavedFile;
        }
        private ActionResult SetDataForReturnAndDeleteFileWithLog(string fileUploadMessageString, FileHandleDetails fileHandleDetails, bool isFileContentOk, bool isDelete = false, string fileName = null)
        {
            if (isDelete)
            {
                if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            }
            fileHandleDetails.FileUploadMsg = fileUploadMessageString;
            fileHandleDetails.FileContentOk = isFileContentOk;
            var userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            return Content(new JavaScriptSerializer().Serialize(fileHandleDetails));


        }
        private string SaveUploadExcelFile(string userIdentity, HttpPostedFile file, string directoryToSave, string fileTypeName)
        {

            string fileName = fileTypeName + "_" + userIdentity + DateTime.Now.ToString("_ddMMyyyy_HHmmss_") + file.FileName;
            string path = Path.Combine(directoryToSave, fileName);
            file.SaveAs(path);
            return path;
        }
        private FileInfo GetFileInfoFromDisk(string path)
        {
            FileInfo file = null;
            file = new System.IO.FileInfo(path);
            return file;
        }


    }
}