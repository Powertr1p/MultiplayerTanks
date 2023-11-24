using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGameManager
{
    public string JoinCode => _joinCode;

    private Allocation _allocation;
    private string _joinCode;
    private string _lobbyId;

    private const int MaxConnections = 20;
    private const string GameSceneName = "Game";
    private const float PingLobbyTime = 15f;
    
    public async Task StartHostAsync()
    {
        try
        {
            _allocation = await Relay.Instance.CreateAllocationAsync(MaxConnections);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }
        
        try
        {
            _joinCode = await Relay.Instance.GetJoinCodeAsync(_allocation.AllocationId);
            Debug.Log(_joinCode);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        
        RelayServerData relayServerData = new RelayServerData(_allocation, "udp");
        transport.SetRelayServerData(relayServerData);

        try
        {
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCode", new DataObject(DataObject.VisibilityOptions.Member, _joinCode)
                }   
            };
            
           Lobby lobby = await Lobbies.Instance.CreateLobbyAsync("My Lobby", MaxConnections, lobbyOptions);
           _lobbyId = lobby.Id;

           HostSingleton.Instance.StartCoroutine(HeartbeatLobby());
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return;
        }
        
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }

    private IEnumerator HeartbeatLobby()
    {
        WaitForSecondsRealtime waitBehaviour = new WaitForSecondsRealtime(PingLobbyTime);
        
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(_lobbyId);
            yield return waitBehaviour;
        }
    }
}
