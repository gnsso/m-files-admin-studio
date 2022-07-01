using MFilesAdminStudio.Proxies.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MFilesAdminStudio.VBScriptExplorerModule.ViewModels
{
    public class VBScriptsListingFilterViewModel : BindableBase
    {
        private bool notifyOnFilterChanged = false;

        private VBScriptScopeViewModel selectedScope;
        public VBScriptScopeViewModel SelectedScope
        {
            get { return selectedScope; }
            set { SetProperty(ref selectedScope, value, TriggerFilterChanged); }
        }

        private List<VBScriptScopeViewModel> scopes;
        public List<VBScriptScopeViewModel> Scopes
        {
            get { return scopes; }
            set { SetProperty(ref scopes, value); }
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { SetProperty(ref searchText, value, TriggerFilterChanged); }
        }

        private bool isSearchTextFocused;
        public bool IsSearchTextFocused
        {
            get { return isSearchTextFocused; }
            set { SetProperty(ref isSearchTextFocused, value); }
        }

        private bool hideDisabledScripts;
        public bool HideDisabledScripts
        {
            get { return hideDisabledScripts; }
            set { SetProperty(ref hideDisabledScripts, value, TriggerFilterChanged); }
        }

        private bool hideGeneratedScripts;
        public bool HideGeneratedScripts
        {
            get { return hideGeneratedScripts; }
            set { SetProperty(ref hideGeneratedScripts, value, TriggerFilterChanged); }
        }

        private bool instantSearchEnabled;
        public bool InstantSearchEnabled
        {
            get { return instantSearchEnabled; }
            set { SetProperty(ref instantSearchEnabled, value); }
        }

        private bool anyAdditionalFiltersActive;
        public bool AnyAdditionalFiltersActive
        {
            get { return anyAdditionalFiltersActive; }
            set { SetProperty(ref anyAdditionalFiltersActive, value); }
        }

        public event EventHandler<Func<object, bool>> Changed;

        public DelegateCommand ToggleInstantSearchCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }

        public VBScriptsListingFilterViewModel()
        {
            var nullableScopes = Enum.GetValues(typeof(VBScriptType)).Cast<VBScriptType>().Select(s => (VBScriptType?)s).ToList();
            nullableScopes.Insert(0, null);

            Scopes = nullableScopes.Select(s => s == null ? new VBScriptScopeViewModel { ScopeName = "ALL SCRIPTS" } : new VBScriptScopeViewModel
            {
                ScopeName = typeof(VBScriptType).GetMember(s.ToString())[0].GetCustomAttribute<VBScriptTypeViewAttribute>().View,
                ScriptType = s
            }).ToList();

            SelectedScope = Scopes[0];

            notifyOnFilterChanged = true;

            SearchCommand = new DelegateCommand(() => TriggerFilterChanged(nameof(SearchText), true));

            ToggleInstantSearchCommand = new DelegateCommand(() =>
            {
                InstantSearchEnabled = !InstantSearchEnabled;

                if (InstantSearchEnabled && !string.IsNullOrWhiteSpace(SearchText))
                {
                    TriggerFilterChanged(nameof(SearchText));
                }
            });
        }

        public bool SetProperty<T>(ref T storage, T value, Action<string> onChanged, [CallerMemberName] string propertyName = null)
        {
            return SetProperty(ref storage, value, () => onChanged(propertyName), propertyName);
        }

        public void Clear(bool notify)
        {
            notifyOnFilterChanged = false;

            SelectedScope = Scopes[0];
            SearchText = "";

            notifyOnFilterChanged = true;

            if (notify)
            {
                TriggerFilterChanged(nameof(Clear));
            }
        }

        private void TriggerFilterChanged(string memberName)
        {
            TriggerFilterChanged(memberName, false);
        }

        private void TriggerFilterChanged(string memberName, bool force)
        {
            IsSearchTextFocused = false;

            AnyAdditionalFiltersActive = HideGeneratedScripts || HideDisabledScripts;

            if (notifyOnFilterChanged)
            {
                if (memberName != nameof(SearchText))
                {
                    Changed?.Invoke(memberName, GeneratePredicate());
                }
                else if (force || ((SearchText?.Length >= 3 || SearchText?.Length == 0) && InstantSearchEnabled))
                {
                    Changed?.Invoke(memberName, GeneratePredicate());
                }
            }
        }

        public Func<object, bool> GeneratePredicate()
        {
            return vbScriptObject =>
            {
                var vbScript = vbScriptObject as VBScriptViewModel;

                if (SelectedScope != null && SelectedScope.ScriptType.HasValue)
                {
                    var scriptType = SelectedScope.ScriptType.Value;

                    if (scriptType == VBScriptType.EventHandler && vbScript.Model.ScriptType != VBScriptType.EventHandler)
                    {
                        return false;
                    }

                    if (scriptType == VBScriptType.Property && vbScript.Model.ScriptType != VBScriptType.Property)
                    {
                        return false;
                    }

                    if (scriptType == VBScriptType.State && vbScript.Model.ScriptType != VBScriptType.State)
                    {
                        return false;
                    }

                    if (scriptType == VBScriptType.StateTransition && vbScript.Model.ScriptType != VBScriptType.StateTransition)
                    {
                        return false;
                    }
                }

                if (HideDisabledScripts && !vbScript.Model.Enabled)
                {
                    return false;
                }

                if (HideGeneratedScripts && !string.IsNullOrWhiteSpace(vbScript.Model.Script) && vbScript.Model.Script.Contains("DO NOT MODIFY!"))
                {
                    return false;
                }

                try
                {
                    if (!string.IsNullOrWhiteSpace(SearchText))
                    {
                        if (Regex.IsMatch(vbScript.Title, SearchText, RegexOptions.IgnoreCase) || Regex.IsMatch(vbScript.ListingDescription, SearchText, RegexOptions.IgnoreCase))
                        {
                            return true;
                        }

                        if (!string.IsNullOrWhiteSpace(vbScript.Model.Script) && !Regex.IsMatch(vbScript.Model.Script, SearchText, RegexOptions.IgnoreCase))
                        {
                            return false;
                        }
                    }
                }
                finally
                {

                }

                return true;
            };
        }
    }
}
