using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class JoinServer : MonoBehaviour
{
   [SerializeField] private Button _joinButton;

   private void OnEnable()
   {
      _joinButton.onClick.AddListener(Join);
   }

   private void OnDisable()
   {
      _joinButton.onClick.RemoveListener(Join);
   }

   private void Join()
   {
      NetworkManager.Singleton.StartClient();
   }
}
