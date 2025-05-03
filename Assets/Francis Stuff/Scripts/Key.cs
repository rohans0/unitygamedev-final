using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Red Player") || other.CompareTag("Blue Player")){
            if(other.CompareTag("Connector")) other.transform.parent.parent.GetComponent<KeyManager>().keys++;
            else other.transform.parent.GetComponent<KeyManager>().keys++;
            Destroy(gameObject);
        }
    }
}
