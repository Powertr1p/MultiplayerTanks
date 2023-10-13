using UnityEngine;

public class LifetimeProjectile : MonoBehaviour
{
    [SerializeField] private float _delayBeforeDestroy = 3f;
    
    private void Start()
    {
        Destroy(gameObject, _delayBeforeDestroy);
    }
}
