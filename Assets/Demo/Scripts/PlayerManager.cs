using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    [SerializeField] int maxHealth = 3;
    public int health;

    public int score = 0;

    void Awake()
    {
        Instance = this;   
        health = maxHealth;
    }

    public void TakeHit()
    {
        health--;
        GameUI.Instance.SetHealth(health);
    }

    public void HealPlayer(){
        health++;
        if(health > maxHealth) health = maxHealth;
        GameUI.Instance.SetHealth(health);
    }

    public bool CanHeal()
    {
        return health < maxHealth;
    }

}
