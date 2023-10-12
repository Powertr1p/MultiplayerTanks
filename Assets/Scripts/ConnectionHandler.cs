using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionHandler : MonoBehaviour
{
   [SerializeField] private Button _joinButton;
   [SerializeField] private Button _hostButton;

   private void OnEnable()
   {
      _joinButton.onClick.AddListener(Join);
      _hostButton.onClick.AddListener(Host);
   }

   private void OnDisable()
   {
      _joinButton.onClick.RemoveAllListeners();
      _hostButton.onClick.RemoveAllListeners();
   }

   private void Join()
   {
      NetworkManager.Singleton.StartClient();
   }

   private void Host()
   {
      NetworkManager.Singleton.StartHost();
   }
}
