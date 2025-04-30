using UnityEngine;

public class BoxCollision : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 cNormal;
    [SerializeField] float hitForce = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.transform.CompareTag("Player") || c.transform.CompareTag("Red Player") || c.transform.CompareTag("Blue Player"))
        {
            cNormal = c.contacts[0].normal;
            rb.AddForce(cNormal * hitForce, ForceMode2D.Impulse);
        }
    }
}
