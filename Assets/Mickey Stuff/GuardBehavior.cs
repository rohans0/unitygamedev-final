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

    public GuardState currentState = GuardState.Idle;
    public enum GuardState
    {
        Stunned,
        Idle,
        Chase,
    }

    public void PlayerDetected()
    {
        currentState = GuardState.Chase;
        
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = KeyManager.Instance.gameObject;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        audioSource = GameObject.Find("GuardDeathAudio").GetComponent<AudioSource>();
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
        lineRenderer.enabled = false;
    }

    void ChaseUpdate()
    {
        agent.SetDestination(CurrentPlayerPos());
        transform.GetChild(0).gameObject.SetActive(false);
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, CurrentPlayerPos());
    }

    Vector3 CurrentPlayerPos()
    {
        if(player.GetComponent<Movement>().redSwinging) return player.transform.GetChild(1).position;
        else return player.transform.GetChild(0).position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Red Player") || collision.transform.CompareTag("Blue Player"))
        {
            PlayerManager.Instance.TakeHit();
            StartCoroutine(HitCoroutine());
        }
    }

    IEnumerator HitCoroutine()
    {
        currentState = GuardState.Stunned;
        GetComponent<Rigidbody2D>().AddForce((transform.position - CurrentPlayerPos()).normalized * stunForce);

        yield return new WaitForSeconds(stunTime);

        currentState = GuardState.Chase;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Connector"))
        {
            GetComponent<Rigidbody2D>().simulated = false;
            other.transform.parent.GetComponent<PlayerManager>().score+=5;
            currentState = GuardState.Stunned;
            //play audio here!!
            audioSource.PlayOneShot(GuardDeathSound, 1);
			lineRenderer.enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).GetComponent<DeathVisual>().PlayVisual();
        }
    }
}
