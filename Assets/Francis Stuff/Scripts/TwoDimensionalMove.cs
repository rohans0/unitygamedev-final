using UnityEngine;

public class TwoDimensionalMove : MonoBehaviour
{

    public float movementSpeed = 5f; // Speed of player movement

    // Update is called once per frame
    void Update()
    {
        Vector3 movedirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f); // Get input direction

        transform.position += movedirection * movementSpeed * Time.deltaTime; // Move the player
    }
}
