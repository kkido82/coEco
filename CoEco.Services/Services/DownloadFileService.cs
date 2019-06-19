using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Services.Services
{
    public class DownloadFileService : IDownloadFileService
    {
        private readonly IDataAccessService _service;
        public DownloadFileService(IDataAccessService service)
        {
            _service = service;
        }
        public byte[] GetBytes(string fileName)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(fileName);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(fileName);
            return data;
        }
    }
}
