using MFilesAdminStudio.Services;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Logging
{
    public class LoggingService : ILoggingService
    {
        private readonly Logger internalLogger;
        private readonly string module;

        public LoggingService(string module)
        {
            internalLogger = LogManager.GetCurrentClassLogger();

            this.module = module;
        }

        public void Info(string message, [CallerMemberName]string memberName = null)
        {
            internalLogger.Info($"[{module}] [{memberName}] {message}");
        }

        public void Warning(string message, [CallerMemberName]string memberName = null)
        {
            internalLogger.Warn($"[{module}] [{memberName}] {message}");
        }

        public void Error(Exception ex, [CallerMemberName]string memberName = null)
        {
            ex.Data.Add("module", module);
            ex.Data.Add("member", memberName);

            internalLogger.Error(ex);
        }

        public void Error(string message, Exception ex = null, [CallerMemberName]string memberName = null)
        {
            if (ex != null)
            {
                internalLogger.Error(ex, $"[{module}] [{memberName}] {message}");
            }
            else
            {
                internalLogger.Error($"[{module}] [{memberName}] {message}");
            }
        }
    }
}
