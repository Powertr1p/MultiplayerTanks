using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _joinCode;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private Button _lobbyButton;
    [SerializeField] private LobbyView _lobby;

    private void OnEnable()
    {
        _hostButton.onClick.AddListener(StartHost);
        _clientButton.onClick.AddListener(StartClient);
        _lobbyButton.onClick.AddListener(ShowLobby);
    }

    private void OnDisable()
    {
        _hostButton.onClick.RemoveListener(StartHost);
        _clientButton.onClick.RemoveListener(StartClient);
        _lobbyButton.onClick.RemoveListener(ShowLobby);
    }
    
    private async void StartHost()
    {
        await HostSingleton.Instance.Host.StartHostAsync();
    }

    private async void StartClient()
    {
        await ClientSingleton.Instance.ClientManager.StartClientAsync(_joinCode.text);
    }

    private void ShowLobby()
    {
        _lobby.Show();
    }
}
