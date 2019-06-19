using CoEco.Data;
using CoEco.Services.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace CoEco.Services.Services
{
    public interface IFileService
    {
        byte[] GetByteArrayFromStream(Stream stream, int contentLength);
        bool IsFileValid(string fileExt, List<string> allowedExtensions = null);
    }
    public class FileService : IFileService
    {
        private readonly IDownloadFileService _downloadFileService;
        private readonly IDataAccessService _service;

        public FileService(IDataAccessService service, IDownloadFileService downloadFileService)
        {
            _service = service;
            _downloadFileService = downloadFileService;
        }
        public byte[] GetByteArrayFromStream(Stream stream, int contentLength)
        {
            using (var binaryReader = new BinaryReader(stream))
            {
                return binaryReader.ReadBytes(contentLength);
            }
        }

        public bool IsFileValid(string fileExt, List<string> allowedExtensions = null)
        {
            if (string.IsNullOrWhiteSpace(fileExt))
                return false;

            if (allowedExtensions == null)
                allowedExtensions = new List<string>(new string[] { ".pdf", ".jpg", ".doc", ".docx" });


            return allowedExtensions.Contains(fileExt);
        }



        private string GetFullPath(string tz, string fileName, string pathDirectory)
        {
            if (!Directory.Exists(pathDirectory))
                Directory.CreateDirectory(pathDirectory);

            //bulild the final directory
            var path = GetFileFolderPath(int.Parse(tz), new StringBuilder(pathDirectory));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return Path.Combine(path, fileName);
        }
        private string GetFileFolderPath(int id, StringBuilder sb)
        {
            //sb.Append(saverApp + @"\");
            sb.Append(BuildPath(id, 1000));
            //sb.Append(id + "\\");           

            return sb.ToString();
        }

        //here is the actual order
        private static string BuildPath(int idCurrent, int iSkip)
        {
            var iFrom = Convert.ToInt32(Math.Floor((double)idCurrent / iSkip) * iSkip);
            var iTo = iFrom + iSkip - 1;

            return (iFrom > 0 ? iFrom : 1) + "-" + iTo + "\\";
        }
    }
}
