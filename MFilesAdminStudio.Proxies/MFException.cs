using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Proxies
{
    public static class MFException
    {
        [Obsolete]
        private static readonly IDictionary<string, string> comExceptions = new Dictionary<string, string>
        {
            ["(0x80040061)"] = "Vault is offline",
            ["(0x8004001A)"] = "Authentication failed",
        };

        public static bool CaptureMessage(Exception exception, out string message)
        {
            if (exception is COMException)
            {
                //message = comExceptions
                //    .Where(w => exception.Message.Contains(w.Key))
                //    .OrderBy(o => exception.Message.IndexOf(o.Key))
                //    .DefaultIfEmpty(new KeyValuePair<string, string>(null, exception.Message))
                //    .First()
                //    .Value;

                message =
                    exception.Message == null ? "An unhandled M-Files error occured" :
                    exception.Message.IndexOf("\r\n\r\n") != -1 ? exception.Message.Substring(0, exception.Message.IndexOf("\r\n\r\n")) :
                    exception.Message.Length > 103 ? exception.Message.Substring(0, 100) + "..." : exception.Message;
            }
            else
            {
                message = exception.Message;
            }

            return true;
        }
    }
}
