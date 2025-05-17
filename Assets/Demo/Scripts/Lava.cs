using System.Linq;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float burnProgress;
    [SerializeField] float burnTime = 2f;
    [SerializeField] LayerMask burnLayer;

    void Update()
    {
        if(burnProgress > burnTime)
        {
            burnProgress = 0f;
            GetComponent<AudioSource>().Play();
            PlayerManager.Instance.TakeHit();
        }
    }

    void FixedUpdate()
    {
        bool bluePlay = false, redPlay = false;
        
        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, radius, burnLayer);

        if(others.Count() > 0)
        {
            foreach(Collider2D other in others)
            {
                if(other.CompareTag("Red Player") && !PlayerManager.Instance.GetComponent<Movement>().redSwinging)
                {
                    burnProgress += Time.fixedDeltaTime;
                    other.transform.GetChild(1).gameObject.SetActive(true);
                    redPlay = true;
                    if(!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
                }
                else if(other.CompareTag("Blue Player") && PlayerManager.Instance.GetComponent<Movement>().redSwinging)
                {
                    burnProgress += Time.fixedDeltaTime;
                    other.transform.GetChild(1).gameObject.SetActive(true);
                    bluePlay = true;
                    if(!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
                }
            }
        }
        else
        {
            burnProgress = 0f;
        }

        if(!redPlay) PlayerManager.Instance.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        if(!bluePlay) PlayerManager.Instance.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        if(!redPlay && !bluePlay) GetComponent<AudioSource>().Stop();
    }
}
