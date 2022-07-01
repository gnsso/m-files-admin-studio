using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Search;
using MFilesAdminStudio.CoreModule.Events;
using MFilesAdminStudio.CoreModule.Helpers;
using MFilesAdminStudio.VBScriptExplorerModule.Events;
using MFilesAdminStudio.VBScriptExplorerModule.ViewModels;
using Prism.Commands;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.VBScriptExplorerModule.Views
{
    public partial class VBScriptBoxView : UserControl
    {
        private readonly ColorizeAvalonEdit colorizeSelectedText;
        private readonly ColorizeAvalonEdit colorizeFindText;

        public VBScriptBoxView()
        {
            InitializeComponent();

            var higlightingDef = ContainerLocator.Container.Resolve<IHighlightingDefinition>("hd");

            vbScriptBox.SyntaxHighlighting = higlightingDef;

            vbScriptBox.TextArea.Margin = new Thickness(2, 0, 0, 0);

            colorizeSelectedText = new ColorizeAvalonEdit();
            colorizeFindText = new ColorizeAvalonEdit();

            vbScriptBox.TextArea.TextView.LineTransformers.Add(colorizeSelectedText);
            vbScriptBox.TextArea.TextView.LineTransformers.Add(colorizeFindText);

            vbScriptBox.TextArea.SelectionChanged += delegate
            {
                if (vbScriptBox.TextArea.Selection.Length > 1)
                {
                    colorizeSelectedText.SelectedText = vbScriptBox.SelectedText;
                }
                else
                {
                    colorizeSelectedText.SelectedText = "";
                }

                vbScriptBox.TextArea.TextView.Redraw();
            };

            DataContextChanged += VBScriptBoxView_DataContextChanged;

            vbScriptBox.Document.TextChanged += delegate
            {
                (DataContext as VBScriptTabViewModel).CurrentScript = vbScriptBox.Document.Text;
            };

            EventOperator.GetEvent<VBScriptFindTextChangedEvent>().Subscribe(ExecuteVBScriptFindTextChanged);
        }

        private void ExecuteVBScriptFindTextChanged(VBScriptTabViewModel vm)
        {
            if (ReferenceEquals(DataContext, vm))
            {
                colorizeFindText.SelectedText = vm.FindText;
                vbScriptBox.TextArea.TextView.Redraw();
            }
        }

        private void VBScriptBoxView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as VBScriptTabViewModel;
            var hasScriptAlready = vm.IsEditedOriginalScript && !string.IsNullOrWhiteSpace(vm?.CurrentScript);

            vbScriptBox.Text = hasScriptAlready ? vm.CurrentScript : vm.Model?.Script;

            if (!hasScriptAlready)
            {
                vbScriptBox.Document.UndoStack.ClearAll();
            }
        }

        private async void SaveVBScript(object sender, RoutedEventArgs e)
        {
            if (await SimpleMessageDialog.ConfirmWarning("Script will be updated on the server side, any changes will be overwritten, are you sure?", dialog: CR.Main_RootDialogIdentifier))
            {
                if (DataContext is VBScriptTabViewModel dc)
                {
                    EventOperator.GetEvent<VBScriptSaveRequestedEvent>().Publish((dc.Model, dc.CurrentScript));
                    (DataContext as VBScriptTabViewModel).Model.Script = dc.CurrentScript;
                    vbScriptBox.Document.UndoStack.ClearAll();
                }
                else
                {
                    await SimpleMessageDialog.Error("Data context error");
                }
            }
        }

        private void Undo(object sender, RoutedEventArgs e)
        {
            vbScriptBox.Document.UndoStack.Undo();
        }

        private void Redo(object sender, RoutedEventArgs e)
        {
            vbScriptBox.Document.UndoStack.Redo();
        }

        private async void UndoAll(object sender, RoutedEventArgs e)
        {
            if (await SimpleMessageDialog.ConfirmWarning("All changes will be undone, are you sure?", dialog: CR.Main_RootDialogIdentifier))
            {
                vbScriptBox.Text = (DataContext as VBScriptTabViewModel)?.Model?.Script;
                vbScriptBox.Document.UndoStack.ClearAll();
            }
        }

        private void CopyAll(object sender, RoutedEventArgs e)
        {
            vbScriptBox.SelectAll();
            Clipboard.SetText(vbScriptBox.Text);
        }

        private async void Reload(object sender, RoutedEventArgs e)
        {
            if (await SimpleMessageDialog.ConfirmWarning("Script will be reloaded from server, all changes made here will be lost, are you sure?", dialog: CR.Main_RootDialogIdentifier))
            {
                EventOperator.GetEvent<VBScriptReloadRequestedEvent>().Publish(((DataContext as VBScriptTabViewModel).Model, vbScriptBox));
            }
        }

        private async void Replace(object sender, RoutedEventArgs e)
        {
            if (DataContext is VBScriptTabViewModel vm && !string.IsNullOrWhiteSpace(vm.FindText) && !string.IsNullOrWhiteSpace(vm.ReplaceText))
            {
                var foundCount = vm.FoundCountText;

                vbScriptBox.Text = vbScriptBox.Text.Replace(vm.FindText, vm.ReplaceText);

                await SimpleMessageDialog.Ok($"{foundCount} occurance(s) replaced", dialog: CR.Main_RootDialogIdentifier);
            }
        }

        private void ToggleReplaceMode(object sender, RoutedEventArgs e)
        {
            if (DataContext is VBScriptTabViewModel vm)
            {
                vm.IsInReplaceMode = !vm.IsInReplaceMode;
            }
        }
    }
}
