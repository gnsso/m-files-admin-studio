using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using MFilesAdminStudio.Services;
using MFilesAdminStudio.VBScriptExplorerModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using Unity;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.VBScriptExplorerModule
{
    public class VBScriptExplorerModuleDef : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var container = containerProvider.GetContainer();

            var fileSystem = container.Resolve<IFileSystemService>();

            using (var streamReader = new System.IO.StreamReader(fileSystem.GetFilePath(CR.Global_HighlightingDefFileName)))
            {
                using (var xmlReader = new System.Xml.XmlTextReader(streamReader))
                {
                    var highlightingDefinition = HighlightingLoader.Load(xmlReader, HighlightingManager.Instance);

                    container.RegisterInstance("hd", highlightingDefinition);
                }
            }
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
