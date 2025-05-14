using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueObject;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If player collides with dialogue object, destroy it
        if (other.CompareTag("Player") || other.CompareTag("Red Player") 
        || other.CompareTag("Blue Player") || other.CompareTag("Connector"))
        {
            dialogueObject.SetActive(true);

            Dialogue dialogue = dialogueObject.GetComponent<Dialogue>();
            dialogue.StartDialogue();
        }
    }
}
