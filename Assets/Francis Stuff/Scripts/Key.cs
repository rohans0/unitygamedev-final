using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Red Player") || other.CompareTag("Blue Player")){
            if(other.CompareTag("Connector")) other.transform.parent.parent.GetComponent<KeyManager>().hasKey = true;
            else other.transform.parent.GetComponent<KeyManager>().hasKey = true;
            Destroy(gameObject);
        }
    }
}
