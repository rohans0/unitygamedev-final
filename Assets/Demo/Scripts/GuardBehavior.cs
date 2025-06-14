using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.AI;

public class GuardBehavior : MonoBehaviour
{
    public float sightRadius;
    [Range(0,360)]
    public float SightAngle;
    public GameObject player;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] LayerMask playerMask;
    LineRenderer lineRenderer; // Reference to the LineRenderer component
    [SerializeField] float viewWidth, viewHeight;
    NavMeshAgent agent;
    [SerializeField] float stunTime;
    [SerializeField] float stunForce;

    public AudioClip GuardDeathSound;
    public AudioSource audioSource;

    [SerializeField] GuardType guardType = GuardType.Default;
    private enum GuardType
	{
		Default,
		Shooter
	}

    public GuardState currentState = GuardState.Idle;
    public enum GuardState
    {
        Stunned,
        Idle,
        Chase,
    }

    public void PlayerDetected()
    {
		// disable view triangle
        transform.GetChild(0).gameObject.SetActive(false);
        currentState = GuardState.Chase;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = KeyManager.Instance.gameObject;
        //lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.positionCount = 2;
        audioSource = GameObject.Find("GuardDeathAudio").GetComponent<AudioSource>();

		switch (guardType)
		{
			case GuardType.Shooter:
				if (GetComponent<ShooterGuard>() == null) Debug.LogError("add ShooterGuard.cs to the shooter guard gameObject stupidhead!!!");
				break;
			default:
		        //lineRenderer.enabled = true;
				//lineRenderer.SetPosition(0, transform.position);
				//lineRenderer.SetPosition(1, GetPlayerPos());
				break;
		}
    }

    void Update()
    {
        transform.GetChild(0).localScale = new Vector3(viewWidth, -viewHeight, 1);

        if(currentState == GuardState.Stunned)
        {
            agent.SetDestination(transform.position);
            return;
        }
        else if(currentState == GuardState.Idle) IdleUpdate();
        else if (currentState == GuardState.Chase) ChaseUpdate();
    }

    void IdleUpdate()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        //lineRenderer.enabled = false;
    }

    void ChaseUpdate()
    {
		Vector3 playerPos = GetPlayerPos();
        agent.SetDestination(GetPlayerPos());

        // this rotates the guard towards the player
        Vector3 direction = playerPos - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        float currentAngle = transform.eulerAngles.z;
        float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, 200 * Time.deltaTime); // 360 is rotation speed, adjust as needed
        transform.rotation = Quaternion.Euler(0, 0, angle);
        // end rotation

        gameObject.transform.GetChild(0).gameObject.SetActive(false); // disables the vision cone


		switch (guardType) // might be unneeded
        {
            case GuardType.Shooter:
                // GetComponent<ShooterGuard>().playerPos = playerPos;
                break;
            default:
                //lineRenderer.enabled = true;
                ///lineRenderer.SetPosition(0, transform.position);
                //lineRenderer.SetPosition(1, GetPlayerPos());
                break;
        }
    }

    public Vector3 GetPlayerPos()
    {
		return (
			player.GetComponent<Movement>().redSwinging ?
			player.transform.GetChild(1).position :
			player.transform.GetChild(0).position
		);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Red Player") || collision.transform.CompareTag("Blue Player"))
        {
            PlayerManager.Instance.TakeHit();
            StartCoroutine(HitCoroutine());
        }else if(collision.transform.CompareTag("Crate") || collision.transform.CompareTag("TableProp"))
        {
            currentState = GuardState.Chase;
        }
    }

    IEnumerator HitCoroutine()
    {
        currentState = GuardState.Stunned;
        GetComponent<Rigidbody2D>().AddForce((transform.position - GetPlayerPos()).normalized * stunForce);

        yield return new WaitForSeconds(stunTime);

        currentState = GuardState.Chase;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Connector"))
        {
            GetComponent<Rigidbody2D>().simulated = false;

            switch (guardType) // might be unneeded
            {
                case GuardType.Shooter:
                    other.transform.parent.GetComponent<PlayerManager>().score+=50;
                    break;
                default:
                    other.transform.parent.GetComponent<PlayerManager>().score+=5;
                    break;
            }

            currentState = GuardState.Stunned;
            //play audio here!!
            audioSource.PlayOneShot(GuardDeathSound, 1);
			//lineRenderer.enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).GetComponent<DeathVisual>().PlayVisual();
        }
        
    }
}
