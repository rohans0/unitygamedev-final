using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject playereref;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public bool canSeePlayer = false;

    public Text myText; // Reference to the Text component

    private void Start()
    {
        playereref = GameObject.FindGameObjectWithTag("Player");
        myText.text = "player not detected"; // Initial text
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
                    myText.text = "player detected"; // Change text when player is detected
                }else{
                    canSeePlayer = false;
                    Debug.Log("Can't see player");
                    myText.text = "player not detected"; // Change text when player is not detected
                }
            }else{
                canSeePlayer = false;
                Debug.Log("Can't see player");
                myText.text = "player not detected"; // Change text when player is not detected
            }
        }
        else if(canSeePlayer){
            canSeePlayer = false;
        }
    }
}
