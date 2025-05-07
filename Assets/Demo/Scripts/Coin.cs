using UnityEngine;

public class Coin : MonoBehaviour
{

    public AudioClip CoinCollectSound;
    public AudioSource audioSource;

    public GameObject onCollectEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        audioSource = GameObject.Find("CoinPickupAudio").GetComponent<AudioSource>();
    }
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
            audioSource.PlayOneShot(CoinCollectSound, 1);

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}
