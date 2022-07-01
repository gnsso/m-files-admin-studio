using MFilesAdminStudio.Services.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Services
{
    public interface ILicenseService
    {
        LicenseCheckResponseModel CheckLicense(LicenseCheckRequestModel request);
    }
}
