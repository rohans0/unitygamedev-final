using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;
    [SerializeField] TMP_Text healthText;

    void Awake()
    {
        Instance = this;
    }

    public void SetHealth(int currentHealth)
    {
        healthText.text = currentHealth.ToString();
    }
}
