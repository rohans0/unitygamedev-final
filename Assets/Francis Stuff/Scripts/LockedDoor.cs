using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public KeyManager km;
    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private bool unlocked = false;

    public Vector3 offset;
    public float openingSpeed = 3.0f;
    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, openingSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerObject") && km.hasKey && !unlocked)
        {
            unlocked = true;
            targetPosition = initialPosition + offset;
        }
    }
}
