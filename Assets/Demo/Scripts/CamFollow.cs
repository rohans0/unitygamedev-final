using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] CameraMode camMode; 
    [SerializeField] GameObject playerObject;

    //Smoothing value, lower value is smoother but slower (smoothing only applied in stationary mode)
    [SerializeField] float maxMoveStationary;
    [SerializeField] float maxMoveRoom;
    [SerializeField] float maxMoveGrid;

    List<CamRoom> rooms = new List<CamRoom>();
    int currentRoom = -1;
    public AudioClip CameraMoveSound;
    public AudioSource audioSource;

    public enum CameraMode
    {
        Midpoint,
        Stationary,
        Room,
        Grid,
        //This mode does nothing
        None,
    }

    void Update()
    {
        if (camMode == CameraMode.Stationary) UpdateStationary();
        else if (camMode == CameraMode.Midpoint) UpdateMidpoint();
        else if (camMode == CameraMode.Room) UpdateRoom();
        else if (camMode == CameraMode.Grid) UpdateGrid();
    }

    void UpdateGrid()
    {
        Vector2 playerPos = playerObject.GetComponent<Movement>().redSwinging ? playerObject.transform.GetChild(1).position : playerObject.transform.GetChild(0).position;
        float x = Mathf.Round(playerPos.x / 12f) * 12f;
        float y = Mathf.Round(playerPos.y / 12f) * 12f;
        Vector2 pos = new Vector2(x, y);
        if(Vector2.Distance(transform.position, pos) > maxMoveGrid * Time.deltaTime) pos = (Vector2)transform.position + ((pos - (Vector2)transform.position).normalized * maxMoveGrid * Time.deltaTime);
        transform.position = (Vector3)pos + (Vector3.forward * transform.position.z);
    }

    void UpdateRoom()
    {
        //Populate rooms list if it hasnt been yet
        if(rooms.Count == 0)
        {
            foreach(CamRoom cr in FindObjectsByType<CamRoom>(FindObjectsSortMode.None))
            {
                rooms.Add(cr);
            }

            if(rooms.Count == 0) camMode = CameraMode.None;
        }

        //Change target room if outside bounds of current target room
        Vector2 xBounds = Vector2.zero;
        Vector2 yBounds = Vector2.zero;
        Vector2 playerPos = playerObject.GetComponent<Movement>().redSwinging? playerObject.transform.GetChild(1).position : playerObject.transform.GetChild(0).position;
        if(currentRoom != -1)
        {
            xBounds = new Vector2(rooms[currentRoom].transform.position.x - rooms[currentRoom].roomBounds.x, rooms[currentRoom].transform.position.x + rooms[currentRoom].roomBounds.x);
            yBounds = new Vector2(rooms[currentRoom].transform.position.y - rooms[currentRoom].roomBounds.y, rooms[currentRoom].transform.position.y + rooms[currentRoom].roomBounds.y);
        }
        if(
            currentRoom == -1 ||
            playerPos.x > xBounds.y ||
            playerPos.x < xBounds.x ||
            playerPos.y > yBounds.y ||
            playerPos.y < yBounds.x
        ) 
        {
            AssignRoom();
            audioSource.PlayOneShot(CameraMoveSound, 1);
        }

        //Actually move camera to target room
        if(currentRoom == -1) return;
        Vector2 pos = rooms[currentRoom].transform.position;
        if(Vector2.Distance(transform.position, pos) > maxMoveRoom * Time.deltaTime) {
            pos = (Vector2)transform.position + ((pos - (Vector2)transform.position).normalized * maxMoveRoom * Time.deltaTime);
            transform.position = (Vector3)pos + (Vector3.forward * transform.position.z);
            
        }
    }

    void AssignRoom()
    {
        for(int i = 0; i < rooms.Count; i++)
        {
            Vector2 xBounds = new Vector2(rooms[i].transform.position.x - rooms[i].roomBounds.x, rooms[i].transform.position.x + rooms[i].roomBounds.x);
            Vector2 yBounds = new Vector2(rooms[i].transform.position.y - rooms[i].roomBounds.y, rooms[i].transform.position.y + rooms[i].roomBounds.y);
            Vector2 playerPos = playerObject.GetComponent<Movement>().redSwinging? playerObject.transform.GetChild(1).position : playerObject.transform.GetChild(0).position;

            if(
            playerPos.x <= xBounds.y &&
            playerPos.x >= xBounds.x &&
            playerPos.y <= yBounds.y &&
            playerPos.y >= yBounds.x
            )
            {
                currentRoom = i;
            }
        }
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

        if(Vector2.Distance(transform.position, pos) > maxMoveStationary * Time.deltaTime) pos = (Vector2)transform.position + ((pos - (Vector2)transform.position).normalized * maxMoveStationary * Time.deltaTime);
        transform.position = (Vector3)pos + (Vector3.forward * transform.position.z);
    }
}
