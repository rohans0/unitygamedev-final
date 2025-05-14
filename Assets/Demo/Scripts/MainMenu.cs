using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadScene(int sceneBuildIndex)
	{
		// find in File > Build Profiles
		SceneManager.LoadScene(sceneBuildIndex);
	}
}
