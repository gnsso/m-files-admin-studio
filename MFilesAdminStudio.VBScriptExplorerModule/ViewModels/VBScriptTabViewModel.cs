using MFilesAdminStudio.CoreModule.Events;
using MFilesAdminStudio.Proxies;
using MFilesAdminStudio.Proxies.Models;
using MFilesAdminStudio.VBScriptExplorerModule.Events;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MFilesAdminStudio.VBScriptExplorerModule.ViewModels
{
    public class VBScriptTabViewModel : BindableBase
    {
        public int Id { get; set; }

        private VBScript model;
        public VBScript Model
        {
            get { return model; }
            set 
            { 
                model = value;

                if (CurrentScript == null)
                {
                    CurrentScript = value?.Script;
                }
            }
        }

        private string header;

        public string Header 
        { 
            get { return header; } 
            set { SetProperty(ref header, value); } 
        }

        public string TooltipHeader { get; set; }
        public string Description { get; set; }

        private string currentScript;
        public string CurrentScript
        {
            get { return currentScript; }
            set 
            { 
                currentScript = value;
                IsEditedOriginalScript = Model?.Script != null && value != null && Model.Script != value;
            }
        }

        private bool isEditedOriginalScript;
        public bool IsEditedOriginalScript
        {
            get { return isEditedOriginalScript; }
            set 
            {
                SetProperty(ref isEditedOriginalScript, value, () =>
                {
                    if (value && !Header.StartsWith("*"))
                    {
                        Header = "*" + Header;
                    }
                    else if (!value && Header.StartsWith("*"))
                    {
                        Header = Header.Substring(1);
                    }
                }); 
            }
        }

        private string findText;
        public string FindText
        {
            get { return findText; }
            set 
            { 
                SetProperty(ref findText, value, () => 
                {
                    if (string.IsNullOrWhiteSpace(CurrentScript))
                    {
                        FoundCountText = "";
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(value) || value.Length < 2)
                    {
                        FoundCountText = "";
                        return;
                    }

                    var foundCount = Regex.Matches(CurrentScript, Regex.Escape(value)).Count;

                    if (foundCount == 0)
                    {
                        FoundCountText = "";
                    }
                    else 
                    {
                        FoundCountText = foundCount.ToString();
                    }
                }); 
            }
        }

        private string foundCountText = "";
        public string FoundCountText
        {
            get { return foundCountText; }
            set { SetProperty(ref foundCountText, value); }
        }

        private string replaceText;
        public string ReplaceText
        {
            get { return replaceText; }
            set { SetProperty(ref replaceText, value); }
        }

        private bool isInReplaceMode;
        public bool IsInReplaceMode
        {
            get { return isInReplaceMode; }
            set { SetProperty(ref isInReplaceMode, value); }
        }
    }
}
