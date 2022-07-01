using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Services
{
    public interface ILoggingService
    {
        void Info(string message, [CallerMemberName]string memberName = null);
        void Error(Exception ex, [CallerMemberName]string memberName = null);
        void Error(string message, Exception ex = null, [CallerMemberName]string memberName = null);
        void Warning(string message, [CallerMemberName]string memberName = null);
    }
}
