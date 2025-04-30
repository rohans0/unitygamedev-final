using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.CompareTag("Player") || other.transform.CompareTag("Red Player") || other.transform.CompareTag("Blue Player")){
            SceneManager.LoadScene(0);
        }
    }
}
