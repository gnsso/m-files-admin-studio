using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Proxies.Models
{
    public enum StateScriptType
    {
        [Description("State Action")]
        StateAction,

        [Description("Pre Condition")]
        PreCondition,

        [Description("Post Condition")]
        PostCondition
    }
}
