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
    private GameObject lastGameRoom;
    private GameObject lastRestRoom;

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
        else if (Input.GetKeyDown(KeyCode.X) && roomDeck.GetRoom("BigRoom"))
        {
            InstanciateRoomServer(roomDeck.GetRoom("BigRoom"), currentRoom, this);
        }
        else if(Input.GetKeyDown(KeyCode.C) && roomDeck.GetRoom("LongTunnelMap")) {
            InstanciateRoomServer(roomDeck.GetRoom("LongTunnelMap"), currentRoom, this);
        }
        else if (Input.GetKeyDown(KeyCode.B) && roomDeck.GetRoom("TowerLongRoom"))
        {
            InstanciateRoomServer(roomDeck.GetRoom("TowerLongRoom"), currentRoom, this);
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
        }
        else
        {
            spawned = Instantiate(roomToInstanciate.m_template, Vector3.zero, roomToInstanciate.m_template.transform.rotation);
        }
        int roomLevel = UnityEngine.Random.Range(1, roomToInstanciate.m_maxLevel + 1);
        spawned.GetComponent<RoomData>().level = roomLevel;

        ServerManager.Despawn(lastGameRoom);
        ServerManager.Spawn(spawned);

        SetInstanciateRoom(spawned, roomToInstanciate.m_name, roomLevel, roomManager);
     }

    [ObserversRpc]
    public void SetInstanciateRoom(GameObject spawnedRoom, string templateName, int roomLevel, RoomManager roomManager)
    {
        Debug.Log("Client instanciation");
        spawnedRoom.GetComponent<RoomData>().level = roomLevel;
        spawnedRoom.GetComponent<RoomData>().template = roomDeck.GetRoom(templateName);
        if(currentRoom)
            currentRoom.transform.Find("Exit").GetComponentInChildren<Animator>().SetTrigger("Lift");

        lastRestRoom = currentRoom;
        currentRoom = spawnedRoom;
        spawnedRoom.transform.SetParent(roomContainer.transform);
    }

    public void InstanciateRestRoomSchedule()
    {
        InstanciateRestRoomServer(restRoom, currentRoom, this);
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
                lastRoom.transform.Find("Exit").position,
                restRoomSO.m_template.transform.rotation * lastRoom.transform.Find("Exit").transform.rotation);
        }
        else
        {
            spawned = Instantiate(restRoomSO.m_template, Vector3.zero, restRoomSO.m_template.transform.rotation);
        }

        ServerManager.Despawn(lastRestRoom);
        ServerManager.Spawn(spawned);

        SetInstanciateRestRoom(spawned, roomManager);
    }

    [ObserversRpc]
    public void SetInstanciateRestRoom(GameObject spawnedRoom, RoomManager roomManager)
    {
        Debug.Log("Client instanciation");
        lastGameRoom = currentRoom;
        currentRoom = spawnedRoom;
        spawnedRoom.transform.SetParent(roomContainer.transform);
    }

    public GameObject GetCurrentRoom()
    {
        return currentRoom;
    }
}
