using Unity.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] GameObject red;
    [SerializeField] GameObject blue;
    [SerializeField] GameObject connector;

    [Header("Movement Settings")]
    [SerializeField] float swingDistance;
    [SerializeField] Vector2 swingDistanceBounds;
    [SerializeField] float distanceChangeSpeed;
    [SerializeField] float swingSpeed;
    //how much space to leave between the swinging player and the wall when attempting to swap swingers while current swinger is touching a wall
    [SerializeField] float wallSpacing;
    //maxMove affects the movement smoothing/interpolation. lower values will be smoother but slower
    [SerializeField] float maxMove;

    [Header("Other")]
    [SerializeField] LayerMask wallLayer;
    [HideInInspector] public bool redSwinging;

    [Header("Visuals")]
    [SerializeField] Sprite redSwing;
    [SerializeField] Sprite redStill;
    [SerializeField] Sprite blueSwing;
    [SerializeField] Sprite blueStill;

    public AudioClip PlayerSwitchSound;
    public AudioSource audioSource;

    void LateUpdate()
    {
        //Control connector position
        connector.transform.localScale = new Vector2(connector.transform.localScale.x, Vector2.Distance(red.transform.position, blue.transform.position) - 1f);
        connector.transform.position = (blue.transform.position + red.transform.position) / 2f;
        connector.transform.rotation = Quaternion.LookRotation(Vector3.forward, red.transform.position - blue.transform.position);

        red.transform.rotation = Quaternion.LookRotation(Vector3.forward, red.transform.position - blue.transform.position);
        blue.transform.rotation = Quaternion.LookRotation(Vector3.forward, red.transform.position - blue.transform.position);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Handle swing size changes
        if(Input.GetKey(KeyCode.A))
        {
            swingDistance -= distanceChangeSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
            swingDistance += distanceChangeSpeed * Time.deltaTime;
        }
        if(swingDistance < swingDistanceBounds.x) swingDistance = swingDistanceBounds.x;
        if(swingDistance > swingDistanceBounds.y) swingDistance = swingDistanceBounds.y;



        //Handle swapping swingers
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Move new stationary player slightly away from wall to prevent bugs
            if(redSwinging)
            {
                //Get current angle between players
                Vector2 pivot = blue.transform.position;
                Vector2 currentPos = red.transform.position;
                float angle = Vector2.SignedAngle(Vector2.right, (currentPos - pivot).normalized);

                //Place the swinging player where they should be if the angle was slightly increased
                angle -= Time.deltaTime * swingSpeed;
                Vector2 desiredPos = pivot + (swingDistance * new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)));

                RaycastHit2D hit = Physics2D.Raycast(pivot, (desiredPos - pivot).normalized, swingDistance, wallLayer);
                if(hit)
                {
                    red.transform.position = hit.point + (hit.normal * wallSpacing);
                }
                else red.transform.position = desiredPos;
            }
            else
            {
                //Get current angle between players
                Vector2 pivot = red.transform.position;
                Vector2 currentPos = blue.transform.position;
                float angle = Vector2.SignedAngle(Vector2.right, (currentPos - pivot).normalized);

                //Place the swinging player where they should be if the angle was slightly increased
                angle -= Time.deltaTime * swingSpeed;
                Vector2 desiredPos = pivot + (swingDistance * new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)));

                RaycastHit2D hit = Physics2D.Raycast(pivot, (desiredPos - pivot).normalized, swingDistance, wallLayer);
                if(hit)
                {
                    blue.transform.position = hit.point + (hit.normal * wallSpacing);
                }
                else blue.transform.position = desiredPos;
            }

            redSwinging = !redSwinging;
            UpdateVisuals();
            audioSource.PlayOneShot(PlayerSwitchSound, 1);
        }

        

        //Update functions depending on which is swinging
        if(redSwinging) RedSwingCasted();
        else BlueSwingCasted();
    }

    void RedSwingCasted()
    {
        //Get current angle between players
        Vector2 pivot = blue.transform.position;
        Vector2 currentPos = red.transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, (currentPos - pivot).normalized);

        //Place the swinging player where they should be if the angle was slightly increased
        angle -= Time.deltaTime * swingSpeed;
        Vector2 desiredPos = pivot + (swingDistance * new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)));

        RaycastHit2D hit = Physics2D.Raycast(pivot, (desiredPos - pivot).normalized, swingDistance, wallLayer);
        if(hit)
        {
            //Do final interpolation
            if(Vector2.Distance(hit.point, red.transform.position) > maxMove * Time.deltaTime) red.transform.position = (Vector2)red.transform.position + (hit.point - (Vector2)red.transform.position).normalized * maxMove * Time.deltaTime;
            else red.transform.position = hit.point;
        }
        else
        {
            //Do final interpolation
            if(Vector2.Distance(desiredPos, red.transform.position) > maxMove * Time.deltaTime) red.transform.position = (Vector2)red.transform.position + (desiredPos - (Vector2)red.transform.position).normalized * maxMove * Time.deltaTime;
            else red.transform.position = desiredPos;
        }
    }

    void BlueSwingCasted()
    {
        //Get current angle between players
        Vector2 pivot = red.transform.position;
        Vector2 currentPos = blue.transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, (currentPos - pivot).normalized);

        //Place the swinging player where they should be if the angle was slightly increased
        angle += Time.deltaTime * swingSpeed;
        Vector2 desiredPos = pivot + (swingDistance * new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)));

        RaycastHit2D hit = Physics2D.Raycast(pivot, (desiredPos - pivot).normalized, swingDistance, wallLayer);
        if(hit)
        {
            //Do final interpolation
            if(Vector2.Distance(hit.point, blue.transform.position) > maxMove * Time.deltaTime) blue.transform.position = (Vector2)blue.transform.position + (hit.point - (Vector2)blue.transform.position).normalized * maxMove * Time.deltaTime;
            else blue.transform.position = hit.point;
        }
        else
        {
            //Do final interpolation
            if(Vector2.Distance(desiredPos, blue.transform.position) > maxMove * Time.deltaTime) blue.transform.position = (Vector2)blue.transform.position + (desiredPos - (Vector2)blue.transform.position).normalized * maxMove * Time.deltaTime;
            else blue.transform.position = desiredPos;
        }
    }

    void UpdateVisuals()
    {
        if(redSwinging)
        {
            red.GetComponent<SpriteRenderer>().sprite = redSwing;
            blue.GetComponent<SpriteRenderer>().sprite = blueStill;
        }
        else
        {
            red.GetComponent<SpriteRenderer>().sprite = redStill;
            blue.GetComponent<SpriteRenderer>().sprite = blueSwing;
        }
    }
}
