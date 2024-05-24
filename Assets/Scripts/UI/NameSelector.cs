using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameSelector : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private ConnectButton _connectButton;
    [SerializeField] private int _minNameLength = 1;
    [SerializeField] private int _maxNameLength = 12;

    public const string PlayerNameKey = "PlayerName";

    private void OnEnable()
    {
        _nameField.onValueChanged.AddListener(HandleNameChanged);
        _connectButton.ConnectPressed += SavePlayerName;
    }

    private void OnDisable()
    {
        _nameField.onValueChanged.RemoveAllListeners();
        _connectButton.ConnectPressed -= SavePlayerName;
    }

    private void Start()
    {
        if (IsServer())
        {
            //todo: убрать отсюда
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        _nameField.text = PlayerPrefs.GetString(PlayerNameKey, String.Empty);
        HandleNameChanged(_nameField.text);
    }
    
    private void SavePlayerName()
    {
        PlayerPrefs.SetString(PlayerNameKey, _nameField.text);
    }

    private void HandleNameChanged(string input)
    {
        bool isLegit = input.Length >= _minNameLength && input.Length <= _maxNameLength;
        
        _connectButton.Show(isLegit);
    }

    private bool IsServer()
    {
        return SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null;
    }
}
