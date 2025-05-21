using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [SerializeField] public List<int2> segments;
    [SerializeField] public int2 exitPos;
    [SerializeField] public DoorSide entranceSide, exitSide;

    public void ToggleGuards(bool on)
    {
        ToggleGuardsR(transform, on);
    }

    public void ToggleGuardsR(Transform t, bool on)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).GetComponent<LavaGuard>() != null) t.GetChild(i).gameObject.SetActive(on);
            ToggleGuardsR(t.GetChild(i), on);
        }
    }

    public enum DoorSide
    {
        Top,
        Bottom,
        Left,
        Right,
        None
    }

    public enum Rotation
    {
        R0,
        R1, 
        R2,
        R3
    }
}
