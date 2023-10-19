using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
   [Header("References")] 
   [SerializeField] private Health _health;
   [SerializeField] private Image _healthImage;

   public override void OnNetworkSpawn()
   {
      if (!IsClient) return;
      
      _health.CurrentHealth.OnValueChanged += HandleHealthChanged;
      HandleHealthChanged(0, _health.CurrentHealth.Value);
   }

   public override void OnNetworkDespawn()
   {
      if (!IsClient) return;
      
      _health.CurrentHealth.OnValueChanged -= HandleHealthChanged;
   }

   private void HandleHealthChanged(int oldHealth, int newHealth)
   {
      _healthImage.fillAmount = (float)newHealth / _health.MaxHealth;
   }
}
