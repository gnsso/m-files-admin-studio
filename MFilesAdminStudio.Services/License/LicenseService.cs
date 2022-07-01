using MFilesAdminStudio.Services;
using MFilesAdminStudio.Services.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.License
{
    public class LicenseService : ILicenseService
    {
        public LicenseCheckResponseModel CheckLicense(LicenseCheckRequestModel request)
        {
            // todo
            return new LicenseCheckResponseModel
            {
                Succeeded = true,
                IsExpired = false,
                ExpiresAt = DateTime.MaxValue,
                LicenseData = new
                {
                    IsSingleModule = true,
                    SingleModuleName = "VB Script Explorer"
                }
            };
        }
    }
}
