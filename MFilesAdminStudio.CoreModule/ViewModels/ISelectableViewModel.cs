using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.CoreModule.ViewModels
{
    public interface ISelectableViewModel
    {
        bool IsSelected { get; set; }
    }
}
