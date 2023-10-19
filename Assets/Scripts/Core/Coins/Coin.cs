using Helpers.Interfaces;
using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour, ICollectable
{
   [SerializeField] private SpriteRenderer _sprite;

   protected int CoinValue;
   protected bool AlreadyCollected;
   
   public abstract int Collect();

   public void SetValue(int value)
   {
      CoinValue = value;
   }

   protected void Show(bool isActive)
   {
      _sprite.enabled = isActive;
   }
}
