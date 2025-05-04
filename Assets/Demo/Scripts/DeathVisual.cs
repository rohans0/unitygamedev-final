using UnityEngine;

public class DeathVisual : MonoBehaviour
{
    [SerializeField] float deathTime;
    float deathTimer;
    [SerializeField] float dissolveTime;
    float dissolveTimer;

    bool dying;
    bool dissolving;

    [SerializeField] bool playBool;

    public void PlayVisual()
    {
        deathTimer = deathTime;
        dying = true;
    }

    void Update()
    {
        if(playBool)
        {
            PlayVisual();
            playBool = false;
        }

        if(dying)
        {
            deathTimer -= Time.deltaTime;
            float tVal = 1f - (deathTimer / deathTime);

            MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
            GetComponent<Renderer>().GetPropertyBlock(_propBlock);
            _propBlock.SetFloat("_DeathProgress", tVal);
            GetComponent<Renderer>().SetPropertyBlock(_propBlock);

            if(deathTimer <= 0f)
            {
                dying = false;
                dissolving = true;
                dissolveTimer = dissolveTime;
                transform.parent.GetComponent<Renderer>().enabled = false;
            }
        }
        else if(dissolving)
        {
            dissolveTimer -= Time.deltaTime;
            float tVal = 1f - (dissolveTimer / dissolveTime);

            MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
            GetComponent<Renderer>().GetPropertyBlock(_propBlock);
            _propBlock.SetFloat("_DissolveProgress", tVal);
            GetComponent<Renderer>().SetPropertyBlock(_propBlock);

            if(dissolveTimer <= 0f)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
