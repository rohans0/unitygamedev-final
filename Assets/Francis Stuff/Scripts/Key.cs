using UnityEngine;

public class Key : MonoBehaviour
{

    public GameObject onCollectEffect;
    public AudioClip KeyCollectSound;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GameObject.Find("KeyPickupAudio").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Red Player") || other.CompareTag("Blue Player") || other.CompareTag("Connector")){
            other.transform.parent.GetComponent<KeyManager>().keys++;

            if (onCollectEffect != null)
            {
                Instantiate(onCollectEffect, transform.position, transform.rotation);
            }

            

            Destroy(gameObject);
            audioSource.PlayOneShot(KeyCollectSound, 1);
        }
    }
}
