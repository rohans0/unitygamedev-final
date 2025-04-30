using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

public class GuardCone : MonoBehaviour 
{
    
    public float speed = 4f; // Speed of the guard movement

    public GameObject PlayerRef;
    public bool canSeePlayer = false;
    private LineRenderer LineRenderer; // Reference to the LineRenderer component
    public Transform origin; // Reference to the origin position of the guard for the line renderer
    public Transform target; // Reference to the target (player)
    public float rotateSpeed = 20f; // Speed of rotation towards the player

    //public Text myText; // Reference to the Text component

    //private Vector3 preChasePos; // Store the position of the guard before chasing the player

    private bool chase = false;


    private void Start()
    {
        PlayerRef = GameObject.FindGameObjectWithTag("Player");
        //myText.text = "player not detected"; // Initial text
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.positionCount = 2;
        // preChasePos = transform.position; // Initialize preChasePos to the guard's starting position

    }


    void Update()
    {
        

        if(canSeePlayer){
            
            float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x -transform.position.x ) * Mathf.Rad2Deg;
            angle -=90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

            //myText.text = "player detected"; // Change text when player is detected

            // preChasePos = transform.position; // Store the position of the guard before chasing the player
            // transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 5); // Move towards the player
        }else{
            LineRenderer.enabled = false; // Disable the line renderer if player is not detected
            //myText.text = "player not detected"; // Change text when player is not detected
            // transform.position = Vector3.MoveTowards(transform.position, preChasePos, Time.deltaTime * 5); // Move back to the original position
        }
        if(origin != null && target != null)
        {
            LineRenderer.SetPosition(0, origin.position); // Set the start point of the line to the origin position
            LineRenderer.SetPosition(1, target.position); // Set the end point of the line to the target position
        }else{
            LineRenderer.enabled = false; // Disable the line renderer if origin or target is not set
        }

        if(chase){
            LineRenderer.enabled = true; // Enable the line renderer when the player is detected
            
            transform.position = Vector3.MoveTowards(transform.position, PlayerRef.transform.position, Time.deltaTime * speed); // Move towards the player  
            Destroy(GameObject.FindWithTag("VisCone")); // Destroy the Vision cone when chase is initiated
        }




    }


    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"OnTriggerStay called with {other.name}");
        if (other.CompareTag("Player"))
        {
            canSeePlayer = true; // Keep canSeePlayer true while the player is within the trigger
            chase = true; // Continue chasing the player
            LineRenderer.enabled = true; // Keep the line renderer enabled while the player is detected
        }
    }
}
