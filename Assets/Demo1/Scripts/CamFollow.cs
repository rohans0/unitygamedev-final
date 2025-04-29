using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] CameraMode camMode; 
    [SerializeField] GameObject playerObject;

    //Smoothing value, lower value is smoother but slower (smoothing only applied in stationary mode)
    [SerializeField] float maxMove;

    public enum CameraMode
    {
        Midpoint,
        Stationary,
    }

    void Update()
    {
        if(camMode == CameraMode.Stationary) UpdateStationary();
        else UpdateMidpoint();
    }

    void UpdateMidpoint()
    {
        Vector2 midPos = (playerObject.transform.GetChild(0).position + playerObject.transform.GetChild(1).position) / 2f;
        transform.position = (Vector3)midPos + (Vector3.forward * transform.position.z);
    }

    void UpdateStationary()
    {
        Vector2 pos;
        if(playerObject.GetComponent<Movement>().redSwinging) pos = playerObject.transform.GetChild(1).position;
        else pos = playerObject.transform.GetChild(0).position;

        if(Vector2.Distance(transform.position, pos) > maxMove) pos = (Vector2)transform.position + ((pos - (Vector2)transform.position).normalized * maxMove);
        transform.position = (Vector3)pos + (Vector3.forward * transform.position.z);
    }
}
