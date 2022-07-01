using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MFilesAdminStudio.LoginModule.ViewModels
{
    public class VaultFilterViewModel : BindableBase
    {
        private bool notifyOnFilterChanged = true;

        private bool isOnline;
        public bool IsOnline
        {
            get { return isOnline; }
            set { SetProperty(ref isOnline, value, TriggerFilterChanged); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value, TriggerFilterChanged); }
        }

        private bool isNameFocused;
        public bool IsNameFocused
        {
            get { return isNameFocused; }
            set { SetProperty(ref isNameFocused, value); }
        }

        public event EventHandler<Predicate<object>> Changed;

        public bool SetProperty<T>(ref T storage, T value, Action<string> onChanged, [CallerMemberName] string propertyName = null)
        {
            return SetProperty(ref storage, value, () => onChanged(propertyName), propertyName);
        }

        public void Clear(bool notify)
        {
            notifyOnFilterChanged = false;

            Name = "";

            notifyOnFilterChanged = true;

            if (notify)
            {
                TriggerFilterChanged(nameof(Clear));
            }
        }

        private void TriggerFilterChanged(string memberName)
        {
            IsNameFocused = false;

            if (notifyOnFilterChanged)
            {
                Changed?.Invoke(memberName, GeneratePredicate());
            }
        }

        public Predicate<object> GeneratePredicate()
        {
            return vaultObject =>
            {
                var vaultVM = vaultObject as VaultViewModel;

                if (IsOnline && !vaultVM.IsOnline)
                {
                    return false;
                }

                try
                {
                    if (!string.IsNullOrWhiteSpace(Name) && !Regex.IsMatch(vaultVM.Name, Name, RegexOptions.IgnoreCase))
                    {
                        return false;
                    }
                }
                catch
                {

                }

                return true;
            };
        }
    }
}
