using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadHandler : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Keep this object alive between scenes
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            GameObject panel = GameObject.Find("MainMenuPanel");
            if (panel != null)
            {
                MainMenu menu = panel.GetComponent<MainMenu>();
                if (menu != null)
                {
                    menu.LoadCredits();
                    StartCoroutine(DestroyAfterDelay());
                }
            }
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
