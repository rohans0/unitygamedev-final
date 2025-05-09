using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private bool unlocked = false;

    public Vector3 offset;
    public float openingSpeed = 3.0f;

    public AudioClip DoorOpenSound;
    public AudioSource audioSource;

    private void Start()
    {
        initialPosition = transform.localPosition;
        targetPosition = initialPosition;
        audioSource = GameObject.Find("DoorSlidingAudio").GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, openingSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if((other.CompareTag("Player") || other.CompareTag("Red Player") || other.CompareTag("Blue Player")) && KeyManager.Instance.keys > 0 && !unlocked)
        {
            KeyManager.Instance.keys--;
            audioSource.PlayOneShot(DoorOpenSound, 1);
            unlocked = true;
            targetPosition = initialPosition + offset;
        }
    }
}
