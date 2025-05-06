using UnityEngine;

public class Egg : MonoBehaviour
{
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // plane script = other.GetComponent<plane>();
        // 
        // if (script != null)
        // {
        //     script.BulletHit();
        //     arrowPrefab.GetComponent<arrow>().numEggOnScreen--;
        //     arrowPrefab.GetComponent<arrow>().updateText();
        //     Destroy(gameObject);
        // }
    }
}
