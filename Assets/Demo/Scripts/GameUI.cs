using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [SerializeField] TMP_Text healthText;

    [Header("Health Text Animation Settings")]
    [SerializeField] private float effectSpeedMultiplier = 1f; // Speed multiplier for the effect

    Vector3 originalScale;
    Vector3 enlargedScale;
    Color blueish = new Color(0.2784f, 1, 1, 1);

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        originalScale = healthText.transform.localScale;
        enlargedScale = originalScale * 1.5f;
    }

    public void SetHealth(int currentHealth, bool heal)
    {
        healthText.text = currentHealth.ToString();
        Color HColor;

        if (heal)
        {
            HColor = blueish;
            CRTV.Instance.healInd();
        }
        else
        {
            HColor = Color.red;
        }

        StartCoroutine(HealthEffect(HColor));
    }

    IEnumerator HealthEffect(Color color)
    {
        // Enlarge the text
        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            healthText.transform.localScale = Vector3.Lerp(originalScale, enlargedScale, elapsedTime / 0.5f);
            healthText.color = Color.Lerp(healthText.color, color, elapsedTime / 0.5f);
            elapsedTime += Time.unscaledDeltaTime * effectSpeedMultiplier;
            yield return null;
        }

        // Shrink back to normal scale
        elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            healthText.transform.localScale = Vector3.Lerp(enlargedScale, originalScale, elapsedTime / 0.5f);
            healthText.color = Color.Lerp(color, Color.green, elapsedTime / 0.5f);
            elapsedTime += Time.unscaledDeltaTime * effectSpeedMultiplier;
            yield return null;
        }

        healthText.transform.localScale = originalScale;
        yield return new WaitForSecondsRealtime(1);
    }
}