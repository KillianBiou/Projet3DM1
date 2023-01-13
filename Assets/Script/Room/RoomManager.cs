using FishNet.Object;
using System;
using Unity.Mathematics;
using UnityEngine;
public class RoomManager : NetworkBehaviour
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && roomDeck.GetRoom("Classic"))
        {
            InstanciateRoomServer(roomDeck.GetRoom("Classic"), currentRoom, this);
        }
        else if (Input.GetKeyDown(KeyCode.X) && roomDeck.GetRoom("LTurn"))
        {
            InstanciateRoomServer(roomDeck.GetRoom("LTurn"), currentRoom, this);
        }
        else if(Input.GetKeyDown(KeyCode.C) && roomDeck.GetRoom("RTurn")) {
            InstanciateRoomServer(roomDeck.GetRoom("RTurn"), currentRoom, this);
        }
        else if (Input.GetKeyDown(KeyCode.V) && roomDeck.GetRoom("Tower"))
        {
            InstanciateRoomServer(roomDeck.GetRoom("Tower"), currentRoom, this);
        }
        else if (Input.GetKeyDown(KeyCode.B) && roomDeck.GetRoom("TestRoom"))
        {
            InstanciateRoomServer(roomDeck.GetRoom("TestRoom"), currentRoom, this);
        }
        else if (Input.GetKeyDown(KeyCode.R) && roomDeck.GetRoom("LTurn"))
        {
            InstanciateRestRoomServer(restRoom, currentRoom, this);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void InstanciateRoomServer(RoomScriptableObject roomToInstanciate, GameObject lastRoom, RoomManager roomManager)
    {
        Debug.Log("SERVER INSTANCIATE");

        GameObject spawned = null;
        if (lastRoom != null)
        {
            Quaternion rotation = roomToInstanciate.m_template.transform.rotation * lastRoom.transform.Find("Exit").transform.rotation;
            if (roomToInstanciate.m_template.transform.Find("Entry"))
                rotation *= roomToInstanciate.m_template.transform.Find("Entry").rotation;

            spawned = Instantiate(roomToInstanciate.m_template,
                lastRoom.transform.Find("Exit").position,
                rotation);
            spawned.GetComponent<RoomData>().template = roomToInstanciate;
            spawned.GetComponent<RoomData>().level = UnityEngine.Random.Range(1, roomToInstanciate.m_maxLevel + 1);
            spawned.GetComponent<RoomData>().level = 2;
        }
        else
        {
            spawned = Instantiate(roomToInstanciate.m_template, Vector3.zero, roomToInstanciate.m_template.transform.rotation);
            spawned.GetComponent<RoomData>().template = roomToInstanciate;
            spawned.GetComponent<RoomData>().level = UnityEngine.Random.Range(1, roomToInstanciate.m_maxLevel + 1);
        }

        ServerManager.Spawn(spawned);
        SetInstanciateRoom(spawned, roomManager);
     }

    [ObserversRpc]
    public void SetInstanciateRoom(GameObject spawnedRoom, RoomManager roomManager)
    {
        Debug.Log("Client instanciation");
        currentRoom = spawnedRoom;
        spawnedRoom.transform.SetParent(roomContainer.transform);
    }

    [ServerRpc(RequireOwnership = false)]
    public void InstanciateRestRoomServer(RoomScriptableObject restRoomSO, GameObject lastRoom, RoomManager roomManager)
    {
        Debug.Log("SERVER INSTANCIATE");

        GameObject spawned = null;
        Debug.Log(restRoomSO.m_template);
        if (lastRoom != null)
        {
            spawned = Instantiate(restRoomSO.m_template,
                lastRoom.transform.Find("Exit").position + /*tempfix*/lastRoom.transform.Find("Exit").transform.right * 5,
                restRoomSO.m_template.transform.rotation * lastRoom.transform.Find("Exit").transform.rotation);
        }
        else
        {
            spawned = Instantiate(restRoomSO.m_template, Vector3.zero, restRoomSO.m_template.transform.rotation);
        }

        ServerManager.Spawn(spawned);
        SetInstanciateRestRoom(spawned, roomManager);
    }

    [ObserversRpc]
    public void SetInstanciateRestRoom(GameObject spawnedRoom, RoomManager roomManager)
    {
        Debug.Log("Client instanciation");
        currentRoom = spawnedRoom;
        spawnedRoom.transform.SetParent(roomContainer.transform);
    }

    public GameObject GetCurrentRoom()
    {
        return currentRoom;
    }
}
