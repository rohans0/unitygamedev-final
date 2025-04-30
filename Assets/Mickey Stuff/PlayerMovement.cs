using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f; // Speed of player movement

    void Update()
    {
        Vector3 movedirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f); // Get input direction

        transform.position += movedirection * movementSpeed * Time.deltaTime; // Move the player
    }
    // void OnCollisionEnter(Collision collision)
    // {
    //     if(collision.gameObject.CompareTag("Guard")){
    //         Destroy(gameObject); // Destroy the player when it collides with an object
    //     }
        
    // }


}
