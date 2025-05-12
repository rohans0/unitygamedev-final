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
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player") || other.transform.CompareTag("Red Player") || other.transform.CompareTag("Blue Player"))
        {
            cNormal = (transform.transform.position - other.transform.position).normalized;//c.contacts[0].normal;
            rb.AddForce(cNormal * hitForce, ForceMode2D.Impulse);
        }
    }
}
