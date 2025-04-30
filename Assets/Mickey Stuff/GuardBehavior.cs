using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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

    private GameObject ActivePlayer; // Reference to the active player object

    //public Text myText; // Reference to the Text component

    //private Vector3 preChasePos; // Store the position of the guard before chasing the player

    private bool chase = false;


    private void Start()
    {
        playereref = GameObject.FindGameObjectWithTag("Player");
        //myText.text = "player not detected"; // Initial text
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
        if(playereref != null){

            if(playereref.gameObject.GetComponent<Movement>().redSwinging == false){
                    ActivePlayer = playereref.transform.GetChild(1).gameObject; // Get the active player object (red)
                    target = playereref.transform.GetChild(1).transform; // Set the target to the red player
                }else{
                   ActivePlayer = playereref.transform.GetChild(0).gameObject; // Get the active player object (blue)
                    target = playereref.transform.GetChild(0).transform; // Set the target to the blue player
            }

            if(canSeePlayer){
                
                Vector3 direction = (ActivePlayer.transform.position - transform.position).normalized;
                float angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToPlayer));



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

                
                transform.position = Vector3.MoveTowards(transform.position, ActivePlayer.transform.position, speed * Time.deltaTime * 2);
               
                
                //transform.position = Vector3.MoveTowards(transform.position, playereref.transform.position, speed * Time.deltaTime);
                Destroy(GameObject.FindWithTag("VisCone")); // Destroy the Vision cone when chase is initiated
            }

        }


    }

    private void FieldOfViewCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0 && playereref != null)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.right, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask);
                if (hit.collider == null)
                {
                    canSeePlayer = true;
                    //Debug.Log("Can see player");
                    chase = true; // Set chase to true when player is detected
                }
                else
                {
                    canSeePlayer = false;
                    //Debug.Log("Can't see player");
                }
            }
            else
            {
                canSeePlayer = false;
                //Debug.Log("Can't see player");
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Red Player") || collision.transform.CompareTag("Blue Player"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("DemoScene"); // Load scene 0 on collision with the active player
            //Debug.Log("Collided with active player. Loading scene 0.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Connector"))
        {
            Destroy(gameObject);
        }
    }
}
