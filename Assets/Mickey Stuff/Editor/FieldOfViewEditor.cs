using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;




[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);
        Vector3 viewAngle1 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.red;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle1 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle2 * fov.radius);

        if(fov.canSeePlayer){
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playereref.transform.position);
        }

    }

    private Vector3 DirectionFromAngle(float eulereY, float anglesInDegrees)
    {
        anglesInDegrees += eulereY;
        return new Vector3(Mathf.Sin(anglesInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(anglesInDegrees * Mathf.Deg2Rad));
    }
}
