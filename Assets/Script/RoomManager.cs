using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private RoomScriptableObject restRoom;

    [SerializeField]
    private List<RoomScriptableObject> room;

    private GameObject currentRoom;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            InstanciateRoom(room[0]);
            InstanciateRestRoom();
        }
        else if(Input.GetKeyDown(KeyCode.X)) {
            InstanciateRoom(room[1]);
            InstanciateRestRoom();
        }
        else if(Input.GetKeyDown(KeyCode.C)) {
            InstanciateRoom(room[2]);
            InstanciateRestRoom();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            InstanciateRoom(room[3]);
            InstanciateRestRoom();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            InstanciateRoom(room[4]);
            InstanciateRestRoom();
        }
    }

    private void InstanciateRoom(RoomScriptableObject roomToInstanciate)
    {
        if (currentRoom != null)
        {
            currentRoom = Instantiate(roomToInstanciate.template,
                currentRoom.transform.Find("Exit").position,
                roomToInstanciate.template.transform.rotation * currentRoom.transform.Find("Exit").transform.rotation);
        }
        else
            currentRoom = Instantiate(roomToInstanciate.template, Vector3.zero, roomToInstanciate.template.transform.rotation);
    }

    private void InstanciateRestRoom()
    {
        if (currentRoom != null)
        {
            currentRoom = Instantiate(restRoom.template,
                currentRoom.transform.Find("Exit").position,
                restRoom.template.transform.rotation * currentRoom.transform.Find("Exit").transform.rotation);
        }
        else
            currentRoom = Instantiate(restRoom.template, Vector3.zero, restRoom.template.transform.rotation);
    }
}
