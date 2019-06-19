using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static CoEco.Core.Enums.Enums;

namespace CoEco.BO.Services
{
    public interface IFileValidation
    {
        string CheckTZFromExecl(string TZ);
        string CheckUnitOrProfilePermmisionIsExists(string ID, List<int> IDs, string tableType);

    }
    public class FileValidation : IFileValidation
    {
        public string CheckTZFromExecl(string TZ)
        {
            if (TZ.Length != 9)
            {
                return "מספר הספרות שונה מ9";
            }

            var tz = 0;
            int.TryParse(TZ, out tz);
            if (tz == 0)
            {
                return "תז לא תקינה, מכילה תווים שאינם מספרים";
            }
            return string.Empty;
        }
        public string CheckUnitOrProfilePermmisionIsExists(string ID, List<int> IDs, string tableType)
        {
            var intID = 0;
            int.TryParse(ID, out intID);
            if (intID == 0)
            {
                if (tableType == TableName.Unit.ToString())
                {
                    return "מספר יחידה לא תקין, מכיל תווים שאינם ספרות";
                }
                else // PermissionsProfile table
                {
                    return "פרופיל הרשאות לא תקין, מכיל תווים שאינם ספרות";
                }
            }
            if (!IDs.Contains(intID))
            {
                if (tableType == TableName.Unit.ToString())
                {
                    return "מספר יחידה לא תקין, לא קיים במערכת";
                }
                else // PermissionsProfile table
                {
                    return "פרופיל הרשאות לא תקין, לא קיים במערכת";
                }
            }
            return string.Empty;

        }
    }
}