using UnityEngine;

public class ThrowBottle : MonoBehaviour
{
    [SerializeField] GameObject lavaPrefab;
    [SerializeField] float speed;
    public Vector3 target;

    void Update()
    {
        transform.position += (target - transform.position).normalized * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target) < 0.05f) Explode();
    }

    void Explode()
    {
        GameObject lava = Instantiate(lavaPrefab);
        lava.transform.position = transform.position;
        Destroy(gameObject);
    }
}
