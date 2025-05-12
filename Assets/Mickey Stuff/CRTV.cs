using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class CRTV : MonoBehaviour
{

    public static CRTV Instance;
    public Volume m_Volume;
    Color blueish = new Color(0.2784f,1,1,1);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        m_Volume = GetComponent<Volume>();
    }

    public void damageInd(){
        StartCoroutine(RedFlash());
    }

    public void healInd(){
        StartCoroutine(GreenFlash());
    }

    IEnumerator GreenFlash(){ // THIS IS A BLUE COLOR CHANGE IGNORE THE NAME IM TOO LAZY TO CHANGE IT :3

        //Debug.Log("Hit Taken");
        VolumeProfile profile = m_Volume.sharedProfile;
        float elapsedTime;
        
        if (profile.TryGet<Vignette>(out Vignette vignette))
        {
            //vignette = profile.Add<Vignette>(false);
            vignette.color.value = new Color(0.2784f,1,1,1); // Green color BLUE COLOR RIGHT NOW
            //Debug.Log("Green shift");
            yield return new WaitForSecondsRealtime(1);

            elapsedTime = 0f;
            while (elapsedTime < 0.5f){
                vignette.color.value = Color.Lerp(blueish, Color.black, elapsedTime / 0.5f);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            //vignette.color.value = new Color(0, 0, 0, 1); // Reset to black
                
        }

    }

    IEnumerator RedFlash(){

        //Debug.Log("Hit Taken");
        VolumeProfile profile = m_Volume.sharedProfile;
        float elapsedTime;
        
        if (profile.TryGet<Vignette>(out Vignette vignette))
        {
            //vignette = profile.Add<Vignette>(false);
            vignette.color.value = new Color(1, 0, 0, 1); // Red color
            //Debug.Log("Red shift");
            yield return new WaitForSecondsRealtime(1);

            elapsedTime = 0f;
            while (elapsedTime < 0.5f){
                vignette.color.value = Color.Lerp(Color.red, Color.black, elapsedTime / 0.5f);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            //vignette.color.value = new Color(0, 0, 0, 1); // Reset to black

            
        }

    }

    // VolumeProfile profile = m_Volume.sharedProfile;
    // if (!profile.TryGet<Fog>(out var fog))
    // {
    //     fog = profile.Add<Fog>(false);
    // }

    // fog.enabled.overrideState = overrideFog;
    // fog.enabled.value = enableFog;

}
