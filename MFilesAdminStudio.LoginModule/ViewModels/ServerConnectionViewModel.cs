using MFilesAdminStudio.CoreModule;
using MFilesAdminStudio.CoreModule.Controls;
using MFilesAdminStudio.CoreModule.Helpers;
using MFilesAdminStudio.CoreModule.Models;
using MFilesAdminStudio.CoreModule.ViewModels;
using MFilesAdminStudio.Proxies;
using MFilesAdminStudio.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unity;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.LoginModule.ViewModels
{
    public class ServerConnectionViewModel : ModuleViewModelBase
    {
        private readonly IRegionManager regionManager;

        private int authType;
        public int AuthType
        {
            get { return authType; }
            set { SetProperty(ref authType, value); }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private string domain;
        public string Domain
        {
            get { return domain; }
            set { SetProperty(ref domain, value); }
        }

        private string spn;
        public string Spn
        {
            get { return spn; }
            set { SetProperty(ref spn, value); }
        }

        private string protocolSequence = "ncacn_ip_tcp";
        public string ProtocolSequence
        {
            get { return protocolSequence; }
            set { SetProperty(ref protocolSequence, value); }
        }

        private string networkAddress = "localhost";
        public string NetworkAddress
        {
            get { return networkAddress; }
            set { SetProperty(ref networkAddress, value); }
        }

        private string endpoint = "2266";
        public string Endpoint
        {
            get { return endpoint; }
            set { SetProperty(ref endpoint, value); }
        }

        private string localComputerName;
        public string LocalComputerName
        {
            get { return localComputerName; }
            set { SetProperty(ref localComputerName, value); }
        }

        private bool encryptedConnection;
        public bool EncryptedConnection
        {
            get { return encryptedConnection; }
            set { SetProperty(ref encryptedConnection, value); }
        }

        public DelegateCommand<PasswordBox> ConnectToServerCommand { get; set; }
        public DelegateCommand OpenServerConnectionHelpPageCommand { get; set; }

        public ServerConnectionViewModel(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, CR.Login_Server_ModuleName)
        {
            this.regionManager = regionManager;

            ConnectToServerCommand = new DelegateCommand<PasswordBox>(ExecuteConnectToServer);
            OpenServerConnectionHelpPageCommand = new DelegateCommand(ExecuteOpenServerConnectionHelpPage);
        }

        private void ExecuteConnectToServer(PasswordBox passwordBox)
        {
            regionManager.RequestTransitionNavigate(CR.Login_RegionHolderName, moduleName, CR.Login_Vault_ModuleName, new LoadingStack { Text = "Connecting to server..." }, d =>
            {
                var serverProxy = new ServerProxy();

                serverProxy.ConnectToServer(
                    authType + 1, // correction
                    username,
                    passwordBox.Password,
                    domain,
                    spn,
                    protocolSequence,
                    networkAddress,
                    endpoint,
                    encryptedConnection,
                    localComputerName);

                unityContainer.RegisterInstance(serverProxy);

                var vaults = serverProxy.GetVaults().Select(s => new VaultViewModel
                {
                    Name = s.name,
                    Guid = s.guid,
                    IsOnline = s.online,
                    IsSelected = false,
                }).ToList();

                return ("vaults", vaults);
            });
        }

        private void ExecuteOpenServerConnectionHelpPage()
        {
            Process.Start("https://www.m-files.com/api/documentation/index.html#MFilesAPI~MFilesServerApplication~Connect.html");
        }
    }
}
