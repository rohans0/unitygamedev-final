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
        if (other.CompareTag("Red Player") || other.CompareTag("Blue Player"))
        {
            // Destroy the health pack
            Destroy(gameObject);

            // Play the effect
            if (onCollectEffect != null)
            {
                Instantiate(onCollectEffect, transform.position, transform.rotation);
            }
            //Debug.Log("healed once");
            PlayerManager.Instance.HealPlayer();
            audioSource.PlayOneShot(HealthCollectSound, 1);


        }
    }
    
}
