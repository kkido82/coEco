using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Services.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly IDataAccessService _dataAccessService;
        public LoggerService(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }

        public void InsertLogMessage(string type, string message, string user)
        {

            _dataAccessService.InsertLogMessage(new Data.Log()
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                LogType = type,
                Disable = false,
                Message = message,
                TZ = user,
                UpdatedBy = user,
                UpdatedOn = DateTime.Now
            });
        }
    }
}
