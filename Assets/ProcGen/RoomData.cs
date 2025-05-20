using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [SerializeField] public List<int2> segments;
    [SerializeField] public int2 exitPos;
    [SerializeField] public DoorSide entranceSide, exitSide;

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
