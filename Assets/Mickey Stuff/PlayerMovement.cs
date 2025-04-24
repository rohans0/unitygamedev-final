using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f; // Speed of player movement


    void Update()
    {
        Vector3 movedirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")); // Get input direction

        transform.position += movedirection * movementSpeed * Time.deltaTime; // Move the player
    }


}
