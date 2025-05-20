using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> roomPrefabs;
    [SerializeField] GameObject startRoomPrefab;
    [SerializeField] GameObject endRoomPrefab;
    [SerializeField] int rooms = 5;
    [SerializeField] bool generate;

    //if an int2 is in the set, there is a room segment at those coordinates
    HashSet<int2> map = new HashSet<int2>();

    void Update()
    {
        if (generate)
        {
            Clear();
            generate = false;
            Generate();
        }
    }

    void Clear()
    {
        map.Clear();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void Generate()
    {
        GameObject startRoom = Instantiate(startRoomPrefab);
        startRoom.transform.parent = transform;
        map.Add(int2.zero);
        bool success = GenerateMapR(int2.zero, startRoom.GetComponent<RoomData>().exitSide, 1);
        if (!success) Debug.Log("GENERATION FAILURE, TRY AGAIN");
    }

    bool GenerateMapR(int2 exitPos, RoomData.DoorSide exitSideComp, int depth)
    {
        GameObject roomObj;
        int room = UnityEngine.Random.Range(0, roomPrefabs.Count);

        //loop through every room prefab, starting at a random point
        for (int i = room; i != -1; i++)
        {
            if (i >= roomPrefabs.Count) i = 0;

            //Get position that the start room must be at
            int2 pos;
            if (exitSideComp == RoomData.DoorSide.Top) pos = exitPos + new int2(0, 1);
            else if (exitSideComp == RoomData.DoorSide.Bottom) pos = exitPos + new int2(0, -1);
            else if (exitSideComp == RoomData.DoorSide.Left) pos = exitPos + new int2(-1, 0);
            else pos = exitPos + new int2(1, 0);

            //Get necessary rotation for the room to fit
            RoomData.Rotation rot = GetRotation(exitSideComp, roomPrefabs[i].GetComponent<RoomData>().entranceSide);

            //Prepare transformations
            int2 mult;
            bool swap;
            if (rot == RoomData.Rotation.R0 || rot == RoomData.Rotation.R2) swap = false;
            else swap = true;
            if (rot == RoomData.Rotation.R0) mult = new int2(1, 1);
            else if (rot == RoomData.Rotation.R1) mult = new int2(1, -1);
            else if (rot == RoomData.Rotation.R2) mult = new int2(-1, -1);
            else mult = new int2(-1, 1);

            //Check if the room can be positioned here
            bool doesFit = true;
            foreach (int2 segment in roomPrefabs[i].GetComponent<RoomData>().segments)
            {
                int2 segmentActual = segment;
                if (swap) segmentActual = new int2(segmentActual.y, segmentActual.x);
                segmentActual *= mult;
                segmentActual += pos;

                if (map.Contains(segmentActual))
                {
                    doesFit = false;
                    break;
                }
            }

            if (doesFit)
            {
                //Add all segments to map
                foreach (int2 segment in roomPrefabs[i].GetComponent<RoomData>().segments)
                {
                    int2 segmentActual = segment;
                    if (swap) segmentActual = new int2(segmentActual.y, segmentActual.x);
                    segmentActual *= mult;
                    segmentActual += pos;

                    map.Add(segmentActual);
                }
                //Create room object
                roomObj = Instantiate(roomPrefabs[i]);
                roomObj.transform.parent = transform;
                if (rot == RoomData.Rotation.R1) roomObj.transform.Rotate(0f, 0f, -90f);
                else if (rot == RoomData.Rotation.R2) roomObj.transform.Rotate(0f, 0f, -180f);
                else if (rot == RoomData.Rotation.R3) roomObj.transform.Rotate(0f, 0f, -270f);
                roomObj.transform.position = new Vector2(pos.x, pos.y) * 12f;

                int2 newExitPos = roomPrefabs[i].GetComponent<RoomData>().exitPos;
                if (swap) newExitPos = new int2(newExitPos.y, newExitPos.x);
                newExitPos *= mult;
                newExitPos += pos;

                RoomData.DoorSide newExitSide = GetExitSide(roomPrefabs[i].GetComponent<RoomData>().exitSide, rot);

                if (depth == rooms)
                {
                    //Last room succesfully generated
                    if (PlaceFinalRoom(newExitPos, newExitSide)) return true;
                    else return false;
                }

                if (GenerateMapR(newExitPos, newExitSide, depth + 1)) return true;
                else
                {
                    Destroy(roomObj);
                    //undo all the added segments
                    foreach (int2 segment in roomPrefabs[i].GetComponent<RoomData>().segments)
                    {
                        int2 segmentActual = pos + segment;
                        if (swap) segmentActual = new int2(segmentActual.y, segmentActual.x);
                        segmentActual *= mult;

                        map.Remove(segmentActual);
                    }
                }
            }

            if (i == room - 1 || (room == 0 && i == roomPrefabs.Count - 1))
            {
                //no room can be found given current configuration, undo last one
                return false;
            }
        }

        return false;
    }

    bool PlaceFinalRoom(int2 exitPos, RoomData.DoorSide exitSideComp)
    {
        //Get position that the start room must be at
        int2 pos;
        if (exitSideComp == RoomData.DoorSide.Top) pos = exitPos + new int2(0, 1);
        else if (exitSideComp == RoomData.DoorSide.Bottom) pos = exitPos + new int2(0, -1);
        else if (exitSideComp == RoomData.DoorSide.Left) pos = exitPos + new int2(-1, 0);
        else pos = exitPos + new int2(1, 0);

        //Get necessary rotation for the room to fit
        RoomData.Rotation rot = GetRotation(exitSideComp, endRoomPrefab.GetComponent<RoomData>().entranceSide);

        //Prepare transformations
        int2 mult;
        bool swap;
        if (rot == RoomData.Rotation.R0 || rot == RoomData.Rotation.R2) swap = false;
        else swap = true;
        if (rot == RoomData.Rotation.R0) mult = new int2(1, 1);
        else if (rot == RoomData.Rotation.R1) mult = new int2(1, -1);
        else if (rot == RoomData.Rotation.R2) mult = new int2(-1, -1);
        else mult = new int2(-1, 1);

        //Check if the room can be positioned here
        foreach (int2 segment in endRoomPrefab.GetComponent<RoomData>().segments)
        {
            int2 segmentActual = segment;
            if (swap) segmentActual = new int2(segmentActual.y, segmentActual.x);
            segmentActual *= mult;
            segmentActual += pos;

            if (map.Contains(segmentActual))
            {
                return false;
            }
        }

        //Create room object
        GameObject roomObj = Instantiate(endRoomPrefab);
        roomObj.transform.parent = transform;
        if (rot == RoomData.Rotation.R1) roomObj.transform.Rotate(0f, 0f, -90f);
        else if (rot == RoomData.Rotation.R2) roomObj.transform.Rotate(0f, 0f, -180f);
        else if (rot == RoomData.Rotation.R3) roomObj.transform.Rotate(0f, 0f, -270f);
        roomObj.transform.position = new Vector2(pos.x, pos.y) * 12f;

        return true;
    }

    RoomData.DoorSide GetExitSide(RoomData.DoorSide exitSide, RoomData.Rotation rot)
    {
        RoomData.DoorSide side;

        if (exitSide == RoomData.DoorSide.Top)
        {
            if (rot == RoomData.Rotation.R0) side = RoomData.DoorSide.Top;
            else if (rot == RoomData.Rotation.R1) side = RoomData.DoorSide.Right;
            else if (rot == RoomData.Rotation.R2) side = RoomData.DoorSide.Bottom;
            else side = RoomData.DoorSide.Left;
        }
        else if (exitSide == RoomData.DoorSide.Right)
        {
            if (rot == RoomData.Rotation.R0) side = RoomData.DoorSide.Right;
            else if (rot == RoomData.Rotation.R1) side = RoomData.DoorSide.Bottom;
            else if (rot == RoomData.Rotation.R2) side = RoomData.DoorSide.Left;
            else side = RoomData.DoorSide.Top;
        }
        else if (exitSide == RoomData.DoorSide.Bottom )
        {
            if (rot == RoomData.Rotation.R0) side = RoomData.DoorSide.Bottom;
            else if (rot == RoomData.Rotation.R1) side = RoomData.DoorSide.Left;
            else if (rot == RoomData.Rotation.R2) side = RoomData.DoorSide.Top;
            else side = RoomData.DoorSide.Right;
        }
        else
        {
            if (rot == RoomData.Rotation.R0) side = RoomData.DoorSide.Left;
            else if (rot == RoomData.Rotation.R1) side = RoomData.DoorSide.Top;
            else if (rot == RoomData.Rotation.R2) side = RoomData.DoorSide.Right;
            else side = RoomData.DoorSide.Bottom;
        }

        return side;
    }

    RoomData.Rotation GetRotation(RoomData.DoorSide exitSideComp, RoomData.DoorSide entranceSide)
    {
        RoomData.Rotation rot;

        if (exitSideComp == RoomData.DoorSide.Top)
        {
            if (entranceSide == RoomData.DoorSide.Bottom) rot = RoomData.Rotation.R0;
            else if (entranceSide == RoomData.DoorSide.Right) rot = RoomData.Rotation.R1;
            else if (entranceSide == RoomData.DoorSide.Top) rot = RoomData.Rotation.R2;
            else rot = RoomData.Rotation.R3;
        }
        else if (exitSideComp == RoomData.DoorSide.Bottom)
        {
            if (entranceSide == RoomData.DoorSide.Top) rot = RoomData.Rotation.R0;
            else if (entranceSide == RoomData.DoorSide.Left) rot = RoomData.Rotation.R1;
            else if (entranceSide == RoomData.DoorSide.Bottom) rot = RoomData.Rotation.R2;
            else rot = RoomData.Rotation.R3;
        }
        else if (exitSideComp == RoomData.DoorSide.Left)
        {
            if (entranceSide == RoomData.DoorSide.Right) rot = RoomData.Rotation.R0;
            else if (entranceSide == RoomData.DoorSide.Top) rot = RoomData.Rotation.R1;
            else if (entranceSide == RoomData.DoorSide.Left) rot = RoomData.Rotation.R2;
            else rot = RoomData.Rotation.R3;
        }
        else
        {
            if (entranceSide == RoomData.DoorSide.Left) rot = RoomData.Rotation.R0;
            else if (entranceSide == RoomData.DoorSide.Bottom) rot = RoomData.Rotation.R1;
            else if (entranceSide == RoomData.DoorSide.Right) rot = RoomData.Rotation.R2;
            else rot = RoomData.Rotation.R3;
        }

        return rot;
    }
}
