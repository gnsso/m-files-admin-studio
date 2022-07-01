using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Proxies.Models
{
    public enum PropertyScriptType
    {
        [Description("Calculated Value")]
        CalculatedValue,

        [Description("Automatic Numbering")]
        AutomaticNumbering,

        [Description("Validation")]
        Validation
    }
}
