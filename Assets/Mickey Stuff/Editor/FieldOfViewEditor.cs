using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;




[CustomEditor(typeof(GuardBehavior))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        GuardBehavior fov = (GuardBehavior)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.back, Vector3.right, 360, fov.radius);
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
        return new Vector3(Mathf.Sin(anglesInDegrees * Mathf.Deg2Rad), Mathf.Cos(anglesInDegrees * Mathf.Deg2Rad), 0);
    }
}
