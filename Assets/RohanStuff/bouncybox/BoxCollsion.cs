using UnityEngine;

public class BoxCollision : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 cNormal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            cNormal = c.contacts[0].normal;
            rb.AddForce(cNormal * 100, ForceMode2D.Impulse);
        }
    }
}
