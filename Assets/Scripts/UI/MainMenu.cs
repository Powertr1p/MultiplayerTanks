using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _hostButton;

    private void OnEnable()
    {
        _hostButton.onClick.AddListener(StartHost);
    }

    private void OnDisable()
    {
        _hostButton.onClick.RemoveListener(StartHost);
    }

    private async void StartHost()
    {
        await HostSingleton.Instance.Host.StartHostAsync();
    }
}
