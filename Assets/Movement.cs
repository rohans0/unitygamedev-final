using Unity.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] GameObject red;
    [SerializeField] GameObject blue;
    [SerializeField] GameObject connector;

    [SerializeField] float swingDistance;
    //NOTE: circleRadius must be less than or equal to swingRadius
    [SerializeField] float swingSpeed;
    [SerializeField] float wallSpacing;

    [SerializeField] LayerMask environment;

    bool redSwinging;

    void Update()
    {
        //Control connector position
        connector.transform.localScale = new Vector2(connector.transform.localScale.x, Vector2.Distance(blue.transform.position, red.transform.position) * 2f);
        connector.transform.position = (blue.transform.position + red.transform.position) / 2f;
        connector.transform.rotation = Quaternion.LookRotation(Vector3.forward, blue.transform.position - red.transform.position);

        //Handle swapping swingers
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SwapVisuals();

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

                RaycastHit2D hit = Physics2D.Raycast(pivot, (desiredPos - pivot).normalized, swingDistance, environment);
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

                RaycastHit2D hit = Physics2D.Raycast(pivot, (desiredPos - pivot).normalized, swingDistance, environment);
                if(hit)
                {
                    blue.transform.position = hit.point + (hit.normal * wallSpacing);
                }
                else blue.transform.position = desiredPos;
            }

            redSwinging = !redSwinging;
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

        RaycastHit2D hit = Physics2D.Raycast(pivot, (desiredPos - pivot).normalized, swingDistance, environment);
        if(hit)
        {
            red.transform.position = hit.point;
        }
        else red.transform.position = desiredPos;
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

        RaycastHit2D hit = Physics2D.Raycast(pivot, (desiredPos - pivot).normalized, swingDistance, environment);
        if(hit)
        {
            blue.transform.position = hit.point;
        }
        else blue.transform.position = desiredPos;
    }

    void RedSwing()
    {
        //Get current angle between players
        Vector2 pivot = blue.transform.position;
        Vector2 currentPos = red.transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, (currentPos - pivot).normalized);

        //Place the swinging player where they should be if the angle was slightly increased
        angle -= Time.deltaTime * swingSpeed;
        red.transform.position = pivot + (swingDistance * new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)));
    }

    void BlueSwing()
    {
        //Get current angle between players
        Vector2 pivot = red.transform.position;
        Vector2 currentPos = blue.transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, (currentPos - pivot).normalized);

        //Place the swinging player where they should be if the angle was slightly increased
        angle += Time.deltaTime * swingSpeed;
        blue.transform.position = pivot + (swingDistance * new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)));
    }

    void SwapVisuals()
    {
        if(red.transform.GetChild(1).gameObject.activeInHierarchy)
        {
            //Change red from stopped to swinging
            red.transform.GetChild(1).gameObject.SetActive(false);
            red.transform.GetChild(0).gameObject.SetActive(true);

            blue.transform.GetChild(1).gameObject.SetActive(true);
            blue.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            //Change red from swinging to stopped
            red.transform.GetChild(1).gameObject.SetActive(true);
            red.transform.GetChild(0).gameObject.SetActive(false);

            blue.transform.GetChild(1).gameObject.SetActive(false);
            blue.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
