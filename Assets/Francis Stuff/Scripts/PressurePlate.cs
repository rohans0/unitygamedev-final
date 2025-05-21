using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject door;
    public float doorOpenSpeed = 1.0f;
    public float doorCloseSpeed = 0.05f;
    private float doorCurSpeed;

    private Vector3 doorInitPos;
    // Dictates how far door will move once pressure plate is pressed
    public Vector3 doorCloseOffset;
    private Vector3 doorCurTargetPos;
    public AudioClip PlateUpSound;
    public AudioClip PlateDownSound;
    public AudioSource audioSource;

    private void Start()
    {
        doorInitPos = door.transform.localPosition; 
        doorCurTargetPos = doorInitPos;
        doorCurSpeed = doorCloseSpeed;
        audioSource = GameObject.Find("DoorSlidingAudio").GetComponent<AudioSource>();
    }


    // Continually moves door based on current target position
    private void Update()
    {
        door.transform.localPosition =
            Vector3.MoveTowards(door.transform.localPosition, doorCurTargetPos, doorCurSpeed * Time.deltaTime);
    }

    // Why was I dumb and not using 2D versions of below methods :(
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If pressure plate collides with crate apply offset to door
        if (other.CompareTag("Crate")) 
        {
            audioSource.PlayOneShot(PlateDownSound, 1);
            doorCurSpeed = doorOpenSpeed;
            doorCurTargetPos = doorInitPos + doorCloseOffset;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // If crate leaves pressure plate remove offset from door
        if (other.CompareTag("Crate"))
        {
            audioSource.PlayOneShot(PlateUpSound, 1);
            doorCurSpeed = doorCloseSpeed;
            doorCurTargetPos = doorInitPos;
        }
    }
}
