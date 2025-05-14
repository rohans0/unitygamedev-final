using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;
    public GameObject playerObject;
    public static bool isPaused;
    public TextMeshProUGUI coinCountText;

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true; 
    }

    public void DeathMenu()
    {
        deathMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false; 
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // 0 is currently the main menu scene
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        deathMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && playerObject.GetComponent<PlayerManager>().health != 0)
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        else if(PlayerManager.Instance.health == 0)
        {
            DeathMenu();
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        coinCountText.text = PlayerManager.Instance.score.ToString();
    }
}
