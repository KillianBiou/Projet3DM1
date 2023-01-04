using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private RoomScriptableObject restRoom;

    [SerializeField]
    private RoomDeckScriptableObject roomDeck;

    private GameObject roomContainer;
    private GameObject currentRoom;

    private void Start()
    {
        roomContainer = GameObject.Find("RoomContainer");

        InstanciateRoom(roomDeck.GetRoom("Classic"));
        InstanciateRestRoom();
        InstanciateRoom(roomDeck.GetRoom("Blockout"));
        InstanciateRestRoom();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && roomDeck.GetRoom("Classic"))
        {
            InstanciateRoom(roomDeck.GetRoom("Classic"));
            InstanciateRestRoom();
        }
        else if(Input.GetKeyDown(KeyCode.X) && roomDeck.GetRoom("LTurn")) {
            InstanciateRoom(roomDeck.GetRoom("LTurn"));
            InstanciateRestRoom();
        }
        else if(Input.GetKeyDown(KeyCode.C) && roomDeck.GetRoom("RTurn")) {
            InstanciateRoom(roomDeck.GetRoom("RTurn"));
            InstanciateRestRoom();
        }
        else if (Input.GetKeyDown(KeyCode.V) && roomDeck.GetRoom("Tower"))
        {
            InstanciateRoom(roomDeck.GetRoom("Tower"));
            InstanciateRestRoom();
        }
        else if (Input.GetKeyDown(KeyCode.B) && roomDeck.GetRoom("Blockout"))
        {
            InstanciateRoom(roomDeck.GetRoom("Blockout"));
            InstanciateRestRoom();
        }
    }

    private void InstanciateRoom(RoomScriptableObject roomToInstanciate)
    {
        if (currentRoom != null)
        {
            currentRoom = Instantiate(roomToInstanciate.m_template,
                currentRoom.transform.Find("Exit").position,
                roomToInstanciate.m_template.transform.rotation * currentRoom.transform.Find("Exit").transform.rotation,
                roomContainer.transform);
            currentRoom.GetComponent<RoomData>().template = roomToInstanciate;
        }
        else
        {
            currentRoom = Instantiate(roomToInstanciate.m_template, Vector3.zero, roomToInstanciate.m_template.transform.rotation, roomContainer.transform);
            currentRoom.GetComponent<RoomData>().template = roomToInstanciate;
        }
    }

    private void InstanciateRestRoom()
    {
        if (currentRoom != null)
        {
            currentRoom = Instantiate(restRoom.m_template,
                currentRoom.transform.Find("Exit").position,
                restRoom.m_template.transform.rotation * currentRoom.transform.Find("Exit").transform.rotation,
                roomContainer.transform);
        }
        else
        {
            currentRoom = Instantiate(restRoom.m_template, Vector3.zero, restRoom.m_template.transform.rotation, roomContainer.transform);
        }
    }
}
