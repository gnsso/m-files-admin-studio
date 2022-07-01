using MFilesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Proxies
{
    public class ServerProxy
    {
        private readonly MFilesServerApplication serverApp = new MFilesServerApplication();

        public bool IsConnected { get; private set; }
        public string NetworkAddress { get; private set; }

        public void ConnectToServer(
            int authType,
            string username = "",
            string password = "",
            string domain = "",
            string spn = "",
            string protocolSequence = "ncacn_ip_tcp",
            string networkAddress = "localhost",
            string endpoint = "2266",
            bool encryptedConnection = false,
            string localComputerName = "")
        {
            var tzi = new TimeZoneInformation();
            tzi.LoadWithCurrentTimeZone();

            try
            {
                serverApp.ConnectAdministrativeEx(tzi, (MFAuthType)authType, username, password, domain, spn, protocolSequence, networkAddress, endpoint, encryptedConnection, localComputerName);

                NetworkAddress = networkAddress;
            }
            catch (Exception ex) when (MFException.CaptureMessage(ex, out var message))
            {
                throw new Exception(message, ex);
            }

            IsConnected = true;
        }

        public (string name, string guid, bool online)[] GetVaults()
        {
            var vaults = serverApp.GetVaults().Cast<VaultOnServer>();
            var onlineVaults = serverApp.GetOnlineVaults().Cast<VaultOnServer>();

            return vaults.Select(s => (s.Name, s.GUID, onlineVaults.Any(a => a.GUID == s.GUID))).ToArray();
        }

        public VaultProxy LoginToVault(string vaultGuid)
        {
            try
            {
                var vault = serverApp.LogInToVault(vaultGuid);

                return new VaultProxy(vault);
            }
            catch (Exception ex) when (MFException.CaptureMessage(ex, out var message))
            {
                throw new Exception(message, ex);
            }
        }

        public string GetVersion()
        {
            return serverApp.GetServerVersion().Display;
        }
    }
}
