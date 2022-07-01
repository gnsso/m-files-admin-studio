using LiteDB;
using MFilesAdminStudio.CoreModule;
using MFilesAdminStudio.License;
using MFilesAdminStudio.Logging;
using MFilesAdminStudio.LoginModule;
using MFilesAdminStudio.MainModule;
using MFilesAdminStudio.MainModule.ViewModels;
using MFilesAdminStudio.MainModule.Views;
using MFilesAdminStudio.Repository;
using MFilesAdminStudio.Services;
using MFilesAdminStudio.Services.FileSystem;
using MFilesAdminStudio.Services.Repository;
using MFilesAdminStudio.Services.Session;
using MFilesAdminStudio.VBScriptExplorerModule;
using MFilesAdminStudio.Views;
using NLog;
using NLog.Config;
using NLog.Targets;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using Unity;
using Unity.Injection;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();

            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            container.RegisterInstance(new AppSession
            {
                FileVersion = fileVersionInfo.FileVersion
            });

            // registers app data file service
            container.RegisterSingleton<IFileSystemService, FileSystemService>(new InjectionConstructor(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                CR.Global_CompanyNameForFileSystem,
                CR.Global_ProductNameForFileSystem));

            CopyFilesToAppData(container.Resolve<IFileSystemService>());
            
            InitNLogConfiguration(container);

            // registers logging service
            container.RegisterType<ILoggingService, LoggingService>();

            // registers core db service
            container.RegisterFactory<IRepositoryService>(RepositoryServiceFactory);

            // registers session service
            container.RegisterFactory<ISessionService>(SessionServiceFactory);

            // registers license service
            container.RegisterSingleton<ILicenseService, LicenseService>();

            BsonMapper.Global.ResolveCollectionName = CollectionNameResolver;

            containerRegistry.RegisterForNavigation<MainView, MainViewModel>(CR.Main_ModuleName);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CoreModuleDef>(CR.Core_ModuleName);
            moduleCatalog.AddModule<MainModuleDef>(CR.Main_ModuleName);
            moduleCatalog.AddModule<LoginModuleDef>(CR.Login_ModuleName);
            moduleCatalog.AddModule<VBScriptExplorerModuleDef>(CR.VBScriptExplorer_ModuleName);
        }

        private static string CollectionNameResolver(Type entityType)
        {
            var colNameAttr = entityType.GetCustomAttribute<CollectionNameAttribute>();

            return colNameAttr == null ? entityType.Name : colNameAttr.Name;
        }

        private static IRepositoryService RepositoryServiceFactory(IUnityContainer unityContainer)
        {
            var fileSystem = unityContainer.Resolve<IFileSystemService>();

            return new RepositoryService(fileSystem, CR.Global_DefaultDBName);
        }

        private ISessionService SessionServiceFactory(IUnityContainer unityContainer)
        {
            var repository = unityContainer.Resolve<IRepositoryService>();

            return new SessionService(repository);
        }

        private void CopyFilesToAppData(IFileSystemService fileSystem)
        {
            // highlighting definition
            var hdefFilePath = fileSystem.GetFilePath(CR.Global_HighlightingDefFileName);

            if (!File.Exists(hdefFilePath))
            {
                File.WriteAllBytes(hdefFilePath, File.ReadAllBytes("vbscript.xshd"));
            }

            // nlog config
            var nlogConfigPath = fileSystem.GetFilePath(CR.Global_NLogConfigFilePath);

            if (!File.Exists(nlogConfigPath))
            {
                File.WriteAllBytes(nlogConfigPath, File.ReadAllBytes("NLog.config"));
            }
        }

        private void InitNLogConfiguration(IUnityContainer container)
        {
            var fileSystem = container.Resolve<IFileSystemService>();
            var appSession = container.Resolve<AppSession>();

            var config = new XmlLoggingConfiguration(fileSystem.GetFilePath(CR.Global_NLogConfigFilePath));

            var target = config.FindTargetByName("f") as FileTarget;

            if (target != null)
            {
                var debugPart = "";
#if DEBUG
                debugPart = "debug/";
#endif
                target.FileName = $"{fileSystem.GetBaseFolderPath().Replace('\\', '/').TrimEnd('/')}/logs/{debugPart}v{appSession.FileVersion}/${{shortdate}}.log";
            }

            LogManager.Configuration = config;
        }
    }
}
