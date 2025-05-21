using UnityEngine;

public class BurnManager : MonoBehaviour
{
    public static BurnManager Instance;
    public bool burning;
    float burnProgress;
    [SerializeField] float burnTime = 2.25f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (burnProgress > burnTime)
        {
            burnProgress = 0f;
            GetComponent<AudioSource>().Play();
            PlayerManager.Instance.TakeHit();
        }
    }

    public void StartBurn()
    {
        
    }
}
