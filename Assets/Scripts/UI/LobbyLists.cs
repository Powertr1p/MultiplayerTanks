using System;
using System.Collections.Generic;
using UI;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyLists : MonoBehaviour
{
    [SerializeField] private Transform _lobbyItemParent;
    [SerializeField] private LobbyItem _lobbyItem;
    [SerializeField] private LobbyView _view;

    private bool _isJoining;
    private bool _isRefreshing;

    private void OnEnable()
    {
        RefreshList();

        _view.OnRefreshButtonClicked += RefreshList;
    }

    private void OnDisable()
    {
        _view.OnRefreshButtonClicked -= RefreshList;
    }

    private async void RefreshList()
    {
        if (_isRefreshing) return;

        _isRefreshing = true;

        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            options.Filters = new List<QueryFilter>()
            {
                new(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"),
                new(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0"),
            };

           QueryResponse lobbies =  await Lobbies.Instance.QueryLobbiesAsync(options);

           foreach (Transform child in _lobbyItemParent)
           {
               Destroy(child.gameObject);
           }

           foreach (Lobby lobby in lobbies.Results)
           {
              LobbyItem lobbyItem =  Instantiate(_lobbyItem, _lobbyItemParent);
              lobbyItem.Initialize(this, lobby);
           }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return;
        }
        
        _isRefreshing = false;
    }

    public async void JoinAsync(Lobby lobby)
    {
        if (_isJoining) return;
        _isJoining = true;
        
        try
        { 
            Lobby joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = joiningLobby.Data["JoinCode"].Value;

            await ClientSingleton.Instance.ClientManager.StartClientAsync(joinCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return;
        }

        _isJoining = false;
    }
}
