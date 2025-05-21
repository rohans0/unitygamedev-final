using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CRTV : MonoBehaviour
{
    public static CRTV Instance;
    public Volume m_Volume;

    [Header("Vignette Effect Settings")]
    [SerializeField] private float fadeSpeedMultiplier = 1f;  // How fast it fades
    [SerializeField] private float fadeDelay = 1f;            // Delay before fade begins

    private Color blueish = new Color(0.2784f, 1, 1, 1);

    void Awake()
    {
        VolumeProfile profile = m_Volume.sharedProfile;
        if (profile.TryGet<Vignette>(out Vignette vignette)){vignette.color.value = new Color(0,0,0,1); // set the vignette color to black at startup
        }


        Instance = this;
        m_Volume = GetComponent<Volume>();
        
    }

    public void damageInd()
    {
        StartCoroutine(RedFlash());
    }

    public void healInd()
    {
        StartCoroutine(GreenFlash());
    }

    IEnumerator GreenFlash()
    {
        VolumeProfile profile = m_Volume.sharedProfile;

        if (profile.TryGet<Vignette>(out Vignette vignette))
        {
            vignette.color.value = blueish;
            yield return new WaitForSecondsRealtime(fadeDelay);

            float elapsedTime = 0f;
            while (elapsedTime < 0.5f)
            {
                vignette.color.value = Color.Lerp(blueish, Color.black, elapsedTime / 0.5f);
                elapsedTime += Time.unscaledDeltaTime * fadeSpeedMultiplier;
                yield return null;
            }
        }
    }

    IEnumerator RedFlash()
    {
        VolumeProfile profile = m_Volume.sharedProfile;

        if (profile.TryGet<Vignette>(out Vignette vignette))
        {
            vignette.color.value = Color.red;
            yield return new WaitForSecondsRealtime(fadeDelay);

            float elapsedTime = 0f;
            while (elapsedTime < 0.5f)
            {
                vignette.color.value = Color.Lerp(Color.red, Color.black, elapsedTime / 0.5f);
                elapsedTime += Time.unscaledDeltaTime * fadeSpeedMultiplier;
                yield return null;
            }
        }
    }
}