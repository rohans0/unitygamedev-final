using UnityEngine;

public class ViewDetector : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Blue Player") || collision.CompareTag("Red Player")) transform.parent.GetComponent<GuardBehavior>().PlayerDetected();
    }
}
