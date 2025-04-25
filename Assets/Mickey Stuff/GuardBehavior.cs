using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

public class GuardBehavior : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;
    public float speed = 4f; // Speed of the guard movement

    public GameObject playereref;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public bool canSeePlayer = false;
    private LineRenderer LineRenderer; // Reference to the LineRenderer component
    public Transform origin;
    public Transform target; // Reference to the target (player)

    public Text myText; // Reference to the Text component

    //private Vector3 preChasePos; // Store the position of the guard before chasing the player

    private bool chase = false;


    private void Start()
    {
        playereref = GameObject.FindGameObjectWithTag("Player");
        myText.text = "player not detected"; // Initial text
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.positionCount = 2;
        // preChasePos = transform.position; // Initialize preChasePos to the guard's starting position
        StartCoroutine(FOVRoutine());

    }

    private IEnumerator FOVRoutine(){
        float delay = 0.2f; // delay between each check rn 5 tiems a second
        WaitForSeconds wait = new WaitForSeconds(delay);

        while(true){
            yield return wait;
            FieldOfViewCheck();
        }
    }

    void Update()
    {
        

        if(canSeePlayer){
            
            var lookPos = target.position - transform.position; // Calculate the direction to the target
            lookPos.y = 0; // Set the y component to 0 to keep the rotation on the horizontal plane
            var rotation = Quaternion.LookRotation(lookPos); // Calculate the rotation to face the target
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5); // Smoothly rotate towards the target

            myText.text = "player detected"; // Change text when player is detected

            // preChasePos = transform.position; // Store the position of the guard before chasing the player
            // transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 5); // Move towards the player
        }else{
            LineRenderer.enabled = false; // Disable the line renderer if player is not detected
            myText.text = "player not detected"; // Change text when player is not detected
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
            
            transform.position = Vector3.MoveTowards(transform.position, playereref.transform.position, Time.deltaTime * speed); // Move towards the player  
            Destroy(GameObject.FindWithTag("VisCone")); // Destroy the Vision cone when chase is initiated
        }




    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0){
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < angle / 2){
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask)){
                    canSeePlayer = true;
                    Debug.Log("Can see player");
                    chase = true; // Set chase to true when player is detected
                    
                    
                }else{
                    canSeePlayer = false;
                    Debug.Log("Can't see player");
                    
                }
            }else{
                canSeePlayer = false;
                Debug.Log("Can't see player");
            }
        }
        else if(canSeePlayer){
            canSeePlayer = false;
        }
    }
}
