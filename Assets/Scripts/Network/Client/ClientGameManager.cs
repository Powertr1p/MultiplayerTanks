using System;
using System.Text;
using System.Threading.Tasks;
using Network.Enums;
using Network.Shared;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network.Client
{
    public class ClientGameManager
    {
        private JoinAllocation _allocation;
        
        private const string LobbySceneName = "Lobby";
        
        public async Task<bool> InitAsync()
        {
            await UnityServices.InitializeAsync();
            
            AuthState authState = await AuthWrapper.DoAuth();

            return authState == AuthState.Authenticated;
        }

        public void ShowMenu()
        {
            SceneManager.LoadScene(LobbySceneName);
        }

        public async Task StartClientAsync(string joinCode)
        {
            try
            {
                _allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }

            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            
            RelayServerData relayServerData = new RelayServerData(_allocation, "udp");
            transport.SetRelayServerData(relayServerData);

            UserData userData = new UserData()
            {
                UserName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "Missing Name")
            };

            string payload = JsonUtility.ToJson(userData);
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
            NetworkManager.Singleton.StartClient();
        }
    }
}