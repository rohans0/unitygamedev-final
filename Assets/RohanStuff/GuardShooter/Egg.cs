using UnityEngine;

public class Egg : MonoBehaviour
{
    private float timeTilDisabled = 10000;
    const float timeToDestroyAfterDisabled = .04f;

    void Update()
    {
        Vector3 newPos = new Vector3(
            -Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad),
            Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad),
            0
        ) * Time.deltaTime * 5.0f;

        newPos = Camera.main.WorldToScreenPoint(newPos + transform.position);

        if (newPos.x < -2 || newPos.x > Screen.width + 2 ||
            newPos.y < -2 || newPos.y > Screen.height + 2)
        {
            Destroy(gameObject);
        }

        transform.position = Camera.main.ScreenToWorldPoint(newPos);

        if (timeTilDisabled < -timeToDestroyAfterDisabled) Destroy(gameObject);
        GetComponent<BoxCollider2D>().enabled = timeTilDisabled > 0;
        timeTilDisabled -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (timeTilDisabled > 1 && (c.gameObject.tag == "Red Player" || c.gameObject.tag == "Blue Player"))
        {
            c.gameObject.GetComponentInParent<PlayerManager>().score--;
            timeTilDisabled = .2f;
		   //          c.gameObject.GetComponentInParent<PlayerManager>().TakeHit();
		   // timeTilDisabled = .0f;
        }
        else if (c.gameObject.tag != "Guard")
        {
            timeTilDisabled = .2f;
        }
    }
}
