using UnityEngine;

public class TextBoxVisual : MonoBehaviour
{

    public Animator animator;
    //public static TextBoxVisual instance;

    void Start()
    {
        animator = GetComponent<Animator>();
        //instance = this;
        animator.SetBool("Done", false);
        
    }

    public void GoAway()
    {
        animator.SetBool("Done", true);
        //Debug.Log("TextBoxVisual: GoAway called, animator set to Done.");
    }
    
}
