using System;
using Network.Shared;
using Unity.Netcode;
using UnityEngine;

namespace Network.Server
{
    public class NetworkServer
    {
        private NetworkManager _networkManager;
        
        public NetworkServer(NetworkManager networkManager)
        {
            _networkManager = networkManager;
            networkManager.ConnectionApprovalCallback += ApprovalCheck;
        }

        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
            UserData userData = JsonUtility.FromJson<UserData>(payload); 
            
            Debug.Log(userData.UserName);

            response.Approved = true; 
        }
    }
}

