using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject onCollectEffect;
    public AudioClip HealthCollectSound;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GameObject.Find("HealAudio").GetComponent<AudioSource>();
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
            PlayerManager.Instance.HealPlayer();
            audioSource.PlayOneShot(HealthCollectSound, 1);

			print("hello");

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}
