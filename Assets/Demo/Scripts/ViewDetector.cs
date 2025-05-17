using UnityEngine;

public class ViewDetector : MonoBehaviour
{

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Blue Player") || collision.CompareTag("Red Player"))
        {
            if (transform.parent.GetComponent<LavaGuard>()) transform.parent.GetComponent<LavaGuard>().PlayerDetected();
            else transform.parent.GetComponent<GuardBehavior>().PlayerDetected();
        }
    }
}
