using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MFilesAdminStudio.LoginModule.Events
{
    public class LoginRequestedEvent : PubSubEvent<PasswordBox>
    {
    }
}
