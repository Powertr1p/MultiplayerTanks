using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _joinCode;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;

    private void OnEnable()
    {
        _hostButton.onClick.AddListener(StartHost);
        _clientButton.onClick.AddListener(StartClient);
    }

    private void OnDisable()
    {
        _hostButton.onClick.RemoveListener(StartHost);
        _clientButton.onClick.RemoveListener(StartClient);
    }
    
    private async void StartHost()
    {
        await HostSingleton.Instance.Host.StartHostAsync();
    }

    private async void StartClient()
    {
        await ClientSingleton.Instance.ClientManager.StartClientAsync(_joinCode.text);
    }
}
