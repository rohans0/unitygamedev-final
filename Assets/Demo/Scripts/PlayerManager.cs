using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    [SerializeField] int maxHealth = 3;
    public int health;

    public int score = 0;
    [SerializeField] AudioSource hurtSound;

    void Awake()
    {
        Instance = this;   
        health = maxHealth;
    }

    public void TakeHit()
    {
        hurtSound.Play();
        health--;
        GameUI.Instance.SetHealth(health, false);
        CRTV.Instance.damageInd();
    }

    public void HealPlayer(){
        health++;
        if(health > maxHealth) health = maxHealth;
        GameUI.Instance.SetHealth(health, true);
    }


}
