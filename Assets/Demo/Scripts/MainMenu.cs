using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartTutorial()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene(2);
    }
}
