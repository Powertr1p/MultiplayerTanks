using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Player
{
    public class NetworkInitializer : MonoBehaviour
    {
        public bool IsInitialized { get; private set; }
        public event Action OnPlayerInitialized;
        
        private void OnEnable()
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete += Initialize;
        }

        private void OnDisable()
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= Initialize;
        }
        
        private void Initialize(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= Initialize;
            
            OnPlayerInitialized?.Invoke();
            
            IsInitialized = true;
        }
    }
}