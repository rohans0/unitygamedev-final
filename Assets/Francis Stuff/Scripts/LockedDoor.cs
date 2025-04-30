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
        initialPosition = transform.localPosition;
        targetPosition = initialPosition;
    }

    private void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, openingSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if((other.CompareTag("Player") || other.CompareTag("Red Player") || other.CompareTag("Blue Player")) && km.hasKey && !unlocked)
        {
            unlocked = true;
            targetPosition = initialPosition + offset;
        }
    }
}
