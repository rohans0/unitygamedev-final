using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            other.transform.parent.GetComponent<KeyManager>().hasKey = true;
            Destroy(gameObject);
        }
    }
}
