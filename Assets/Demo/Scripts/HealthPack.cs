using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject onCollectEffect;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Red Player") || other.CompareTag("Blue Player"))
        {
            // Play the effect
            if (onCollectEffect != null)
            {
                Instantiate(onCollectEffect, transform.position, transform.rotation);
            }
            PlayerManager.Instance.HealPlayer();

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}
