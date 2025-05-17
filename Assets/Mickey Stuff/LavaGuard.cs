using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.AI;

public class LavaGuard : MonoBehaviour
{
    public GameObject player;
    [SerializeField] float viewWidth, viewHeight;
    NavMeshAgent agent;

    public AudioClip GuardDeathSound;
    public AudioSource audioSource;

    [SerializeField] float stopDistance = 3f;
    [SerializeField] LayerMask sightLayers;
    float throwTimer;
    [SerializeField] float throwTime = 2f;
    [SerializeField] GameObject bottlePrefab;

    public GuardState currentState = GuardState.Idle;
    public enum GuardState
    {
        Throwing,
        Idle,
        Chase,
        Death,
    }

    public void PlayerDetected()
    {
        if (!HasLineOfSight()) return;
		// disable view triangle
        transform.GetChild(0).gameObject.SetActive(false);
        currentState = GuardState.Chase;
    }

    bool HasLineOfSight()
    {
        RaycastHit2D other = Physics2D.Raycast(transform.position, (GetPlayerPos() - transform.position).normalized, float.PositiveInfinity, sightLayers);

        if (other.transform.CompareTag("Red Player") || other.transform.CompareTag("Blue Player")) return true;
        else return false;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = KeyManager.Instance.gameObject;
        audioSource = GameObject.Find("GuardDeathAudio").GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.GetChild(0).localScale = new Vector3(viewWidth, -viewHeight, 1);

        if (currentState == GuardState.Death) return;
        else if (currentState == GuardState.Throwing)
        {
            agent.SetDestination(transform.position);
            ThrowUpdate();
        }
        else if (currentState == GuardState.Idle) IdleUpdate();
        else if (currentState == GuardState.Chase) ChaseUpdate();
    }

    void ThrowUpdate()
    {
        Vector3 playerPos = GetPlayerPos();
        if (Vector2.Distance(transform.position, (Vector2)playerPos) > stopDistance || !HasLineOfSight())
        {
            currentState = GuardState.Chase;
            throwTimer = 0f;
            return;
        }

        throwTimer -= Time.deltaTime;
        if (throwTimer <= 0f)
        {
            throwTimer = throwTime;
            GameObject bottle = Instantiate(bottlePrefab);
            bottle.transform.position = (Vector2)transform.position;
            bottle.GetComponent<ThrowBottle>().target = playerPos;
        }
    }

    void IdleUpdate()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void ChaseUpdate()
    {
		Vector3 playerPos = GetPlayerPos();
        if (Vector2.Distance(transform.position, (Vector2)playerPos) <= stopDistance && HasLineOfSight())
        {
            //start throwing
            currentState = GuardState.Throwing;
            return;
        }
        agent.SetDestination(GetPlayerPos());
    }

    private Vector3 GetPlayerPos()
    {
		return (
			player.GetComponent<Movement>().redSwinging ?
			player.transform.GetChild(1).position :
			player.transform.GetChild(0).position
		);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Connector"))
        {
            GetComponent<Rigidbody2D>().simulated = false;
            other.transform.parent.GetComponent<PlayerManager>().score += 5;
            currentState = GuardState.Death;
            //play audio here!!
            audioSource.PlayOneShot(GuardDeathSound, 1);
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).GetComponent<DeathVisual>().PlayVisual();
        }
        
    }
}
