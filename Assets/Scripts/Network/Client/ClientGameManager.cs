using System;
using System.Threading.Tasks;
using Network.Enums;
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
        
            NetworkManager.Singleton.StartClient();
        }
    }
}