using System.Linq;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float burnProgress;
    [SerializeField] float burnTime = 2f;
    [SerializeField] LayerMask burnLayer;

    [SerializeField] AnimationCurve alphaCurve;
    [SerializeField] float lifetime = 3f;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifetime)
        {
            PlayerManager.Instance.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            PlayerManager.Instance.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            Destroy(gameObject);
        }
        SetAlpha(alphaCurve.Evaluate(timer / lifetime));
    }

    void FixedUpdate()
    {
        //bool bluePlay = false, redPlay = false;

        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, radius, burnLayer);

        if (others.Count() > 0)
        {
            foreach (Collider2D other in others)
            {
                if (other.CompareTag("Red Player") && !PlayerManager.Instance.GetComponent<Movement>().redSwinging)
                {
                    BurnManager.Instance.burning = true;
                    /*
                    burnProgress += Time.fixedDeltaTime;
                    other.transform.GetChild(1).gameObject.SetActive(true);
                    redPlay = true;
                    if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
                    */
                }
                else if (other.CompareTag("Blue Player") && PlayerManager.Instance.GetComponent<Movement>().redSwinging)
                {
                    BurnManager.Instance.burning = true;
                    /*
                    burnProgress += Time.fixedDeltaTime;
                    other.transform.GetChild(1).gameObject.SetActive(true);
                    bluePlay = true;
                    if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
                    */
                }
            }
        }
        else
        {
            //burnProgress = 0f;
        }
    }

    void SetAlpha(float newAlpha)
    {
        Renderer renderer = GetComponent<Renderer>();
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(mpb);

        // Example for float alpha property
        mpb.SetFloat("_alpha", newAlpha);

        // Or for color property with alpha
        // mpb.SetColor("_BaseColor", new Color(1f, 1f, 1f, 0.5f));

        renderer.SetPropertyBlock(mpb);





        Renderer renderer2 = transform.GetChild(0).GetComponent<Renderer>();
        MaterialPropertyBlock mpb2 = new MaterialPropertyBlock();
        renderer2.GetPropertyBlock(mpb2);

        // Example for float alpha property
        mpb2.SetFloat("_alpha", newAlpha);

        // Or for color property with alpha
        // mpb.SetColor("_BaseColor", new Color(1f, 1f, 1f, 0.5f));

        renderer2.SetPropertyBlock(mpb2);
    }
}
