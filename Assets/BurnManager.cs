using UnityEngine;
using UnityEngine.VFX;

public class BurnManager : MonoBehaviour
{
    public static BurnManager Instance;
    public bool burning;
    float burnProgress;
    [SerializeField] float burnTime = 2.25f;
    [SerializeField] GameObject burnEffectBlue;
    [SerializeField] GameObject burnEffectRed;
    [SerializeField] AudioSource burnAudio;

    void Awake()
    {
        Instance = this;
    }

    void FixedUpdate()
    {
        if (burning)
        {
            if (!burnAudio.isPlaying) burnAudio.Play();

            burnProgress += Time.deltaTime;
            if (Movement.Instance.redSwinging)
            {
                if (!burnEffectBlue.activeInHierarchy) burnEffectBlue.SetActive(true);
                if (burnEffectRed.activeInHierarchy) burnEffectRed.SetActive(false);
            }
            else
            {
                if (burnEffectBlue.activeInHierarchy) burnEffectBlue.SetActive(false);
                if (!burnEffectRed.activeInHierarchy) burnEffectRed.SetActive(true);
            }
        }
        else
        {
            burnProgress = 0f;
            burnAudio.Stop();
            burnEffectBlue.SetActive(false);
            burnEffectRed.SetActive(false);
        }

        if (burnProgress > burnTime)
        {
            burnProgress = 0f;
            burnAudio.Play();
            PlayerManager.Instance.TakeHit();
        }

        burning = false;
    }
}
