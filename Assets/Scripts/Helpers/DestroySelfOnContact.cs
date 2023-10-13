using UnityEngine;

public class DestroySelfOnContact : MonoBehaviour
{
   private void OnCollisionEnter2D(Collision2D other)
   {
      Destroy(gameObject);
   }
}
