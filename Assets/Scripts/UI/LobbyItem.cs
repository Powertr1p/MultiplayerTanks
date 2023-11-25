using System;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _lobbyNameText;
    [SerializeField] private TMP_Text _lobbyPlayersText;
    [SerializeField] private Button _joinButton;

    private LobbyLists _lobbyLists;
    private Lobby _lobby;

    public void OnEnable()
    {
        _joinButton.onClick.AddListener(Join);
    }

    public void OnDisable()
    {
        _joinButton.onClick.RemoveListener(Join);
    }

    public void Initialize(LobbyLists lobbiesList, Lobby lobby)
    {
        _lobbyLists = lobbiesList;
        _lobby = lobby;
        
        _lobbyNameText.text = lobby.Name;
        _lobbyPlayersText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
    }

    private void Join()
    {
        _lobbyLists.JoinAsync(_lobby);
    }
}
