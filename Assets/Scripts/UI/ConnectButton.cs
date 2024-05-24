using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ConnectButton : MonoBehaviour
    {
        public event Action ConnectPressed;
        
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Connect);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void Show(bool isActive)
        {
            _button.interactable = isActive;
        }

        private void Connect()
        {
            ConnectPressed?.Invoke();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}