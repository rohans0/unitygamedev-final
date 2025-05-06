using UnityEngine;

public class Coin : MonoBehaviour
{

    public GameObject onCollectEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Red Player") || other.CompareTag("Blue Player"))
        {
            // Play the effect
            if (onCollectEffect != null)
            {
                Instantiate(onCollectEffect, transform.position, transform.rotation);
            }

            other.transform.parent.GetComponent<PlayerManager>().score++;

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}
