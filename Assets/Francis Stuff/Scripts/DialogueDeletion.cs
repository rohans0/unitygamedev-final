using System.Collections;
using UnityEngine;

public class DialogueDeletion : MonoBehaviour
{
    public GameObject dialogueObject;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If player collides with dialogue object, destroy it
        if (other.CompareTag("Player") || other.CompareTag("Red Player")
        || other.CompareTag("Blue Player") || other.CompareTag("Connector"))
        {
            //Debug.Log("1");
            Dialogue dialogue = dialogueObject.GetComponent<Dialogue>();
            dialogue.DeleteHelper();

        }
    }

    
}
