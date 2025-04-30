using UnityEngine;

public class BoxCollisionTrigger : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 cNormal;
    [SerializeField] float hitForce = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Red Player") || other.CompareTag("Blue Player"))
        {
            cNormal = (transform.position - other.transform.position).normalized;
            rb.AddForce(cNormal * hitForce, ForceMode2D.Impulse);
        }
    }
}
