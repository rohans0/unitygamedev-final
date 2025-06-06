using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public GameObject mainMenuPanel;
	public GameObject levelMenuPanel;
	// public GameObject settingsMenuPanel; Maybe add later (need to utilize data persistence between scenes and mess around with how audio managed)
	public GameObject playPanel;
	public GameObject creditsPanel;

	private void Awake()
	{
		// Doesn't find inactive objects, so we need to assign them in the inspector
		// mainMenuPanel = GameObject.Find("MainMenuPanel");
		// levelMenuPanel = GameObject.Find("LevelMenuPanel");
		// settingsMenuPanel = GameObject.Find("SettingsMenuPanel");
		// playPanel = GameObject.Find("PlayPanel");
	}

	private void Start()
	{
		// Ensure only the main menu is active at the start
		mainMenuPanel.SetActive(true);
		levelMenuPanel.SetActive(false);
		// settingsMenuPanel.SetActive(false);
		playPanel.SetActive(false);
		creditsPanel.SetActive(false);
	}
	public void LoadScene(int sceneBuildIndex)
	{
		// find in File > Build Profiles
		SceneManager.LoadScene(sceneBuildIndex);
	}

	public void LoadLevelMenu()
	{
		mainMenuPanel.SetActive(false);
		// settingsMenuPanel.SetActive(false);
		playPanel.SetActive(false);
		creditsPanel.SetActive(false);
		levelMenuPanel.SetActive(true);
	}

	public void LoadSettingsMenu()
	{
		mainMenuPanel.SetActive(false);
		levelMenuPanel.SetActive(false);
		playPanel.SetActive(false);
		creditsPanel.SetActive(false);
		// settingsMenuPanel.SetActive(true);
	}

	public void BackToMainMenu()
	{
		levelMenuPanel.SetActive(false);
		// settingsMenuPanel.SetActive(false);
		playPanel.SetActive(false);
		creditsPanel.SetActive(false);
		mainMenuPanel.SetActive(true);
	}

	public void LoadPlayPanel()
	{
		mainMenuPanel.SetActive(false);
		levelMenuPanel.SetActive(false);
		creditsPanel.SetActive(false);
		// settingsMenuPanel.SetActive(false);
		playPanel.SetActive(true);
	}

	public void LoadMainMenu()
	{
		levelMenuPanel.SetActive(false);
		playPanel.SetActive(false);
		creditsPanel.SetActive(false);
		mainMenuPanel.SetActive(true);
	}

	public void LoadCredits()
	{ 
		mainMenuPanel.SetActive(false);
		levelMenuPanel.SetActive(false);
		playPanel.SetActive(false);
		creditsPanel.SetActive(true);
	}
}
