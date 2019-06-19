using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEco.BO.ExcelUploadFiles
{
    public class Enums
    {
        public enum ExcelMembersCol
        {
            TZ = 1,
            FirstName = 2,
            LastName = 3,
            Email = 4,
            PhoneNumber = 5,
            Role = 6, 
            UnitID = 7,
            PermissionsProfileID = 8,

        }
        public enum ExcelFileType
        {
            CoecoMembers
        }

        public enum FileReturnMsg
        {
            IncorrectTZ,
            MainUploadFileFailed,
            FileWasHandle,
            DuplicatedRowsProblem,
            FileTypeIncorrect,
            FileExtensionIncorrect,
            FileSameContentIsContinue,
            NewFile,
            NoFileToUpload,
            ErrorUploadFile,
            FileSameContentError,
            ShowErrorList,
            FileNotFound,
            ErrorUploadFileFailed,
            FileStructureIsInvalid,
            UnitDoesNotExist,
            PermissionsProfileDoesNotExist
        }
    }
}