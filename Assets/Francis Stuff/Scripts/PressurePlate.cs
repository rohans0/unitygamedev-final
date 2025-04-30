using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject door;
    // Dictates how far door will move once pressure plate is pressed
    public Vector3 offset;
    public float openingSpeed = 3.0f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        initialPosition = door.transform.localPosition; 
        targetPosition = initialPosition;
    }

    // Continually moves door based on current target position
    private void Update()
    {
        door.transform.localPosition = Vector3.MoveTowards(door.transform.localPosition, targetPosition, openingSpeed * Time.deltaTime);
    }

    // Why was I dumb and not using 2D versions of below methods :(
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If pressure plate collides with crate apply offset to door
        if (other.CompareTag("Crate")) 
        {
            targetPosition = initialPosition + offset;
            Debug.Log("Crate Collided");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // If crate leaves pressure plate remove offset from door
        if (other.CompareTag("Crate")) 
        {
            targetPosition = initialPosition;
        }
    }
}
