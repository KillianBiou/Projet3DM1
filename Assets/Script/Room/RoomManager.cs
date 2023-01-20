using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using Unity.Mathematics;
using UnityEngine;
public class RoomManager : NetworkBehaviour
{

    [SerializeField]
    private RoomScriptableObject restRoom;

    [SerializeField]
    private RoomDeckScriptableObject roomDeck;

    [SerializeField]
    private GameObject beginRoom;
    [SerializeField]
    private GameObject endRoom;

    private GameObject roomContainer;
    private GameObject currentRoom;
    private GameObject lastGameRoom;
    private GameObject lastRestRoom;

    private void Start()
    {
        roomContainer = GameObject.Find("RoomContainer");
    }

    public void RequestRoomConstruction(RoomScriptableObject roomToInstanciate, int lvl)
    {
        InstanciateRoomServer(roomDeck.GetRoom(roomToInstanciate.m_name), currentRoom, lvl, this);
    }

    public void RequestEndRoomConstruction(bool reversed)
    {
        if (reversed)
        {
            InstanciateEndRoom(endRoom, currentRoom, this);

        }
        else
        {
            InstanciateEndRoom(beginRoom, currentRoom, this);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void InstanciateEndRoom(GameObject roomToInstanciate, GameObject lastRoom, RoomManager roomManager)
    {
        Debug.Log("SERVER INSTANCIATE");

        GameObject spawned = null;
        if (lastRoom != null)
        {
            Quaternion rotation = roomToInstanciate.transform.rotation * lastRoom.transform.Find("Exit").transform.rotation;
            if (roomToInstanciate.transform.Find("Entry"))
                rotation *= roomToInstanciate.transform.Find("Entry").rotation;

            spawned = Instantiate(roomToInstanciate,
                lastRoom.transform.Find("Exit").position,
                rotation);
        }
        else
        {
            spawned = Instantiate(roomToInstanciate, Vector3.zero, roomToInstanciate.transform.rotation);
        }

        ServerManager.Despawn(lastGameRoom);
        ServerManager.Spawn(spawned);

        SetInstanciateEndRoom(spawned);
    }

    [ObserversRpc]
    public void SetInstanciateEndRoom(GameObject spawnedRoom)
    {
        Debug.Log("Client instanciation");
        if (currentRoom)
            currentRoom.transform.Find("Exit").GetComponentInChildren<Animator>().SetTrigger("Lift");

        lastRestRoom = currentRoom;
        currentRoom = spawnedRoom;
        spawnedRoom.transform.SetParent(roomContainer.transform);
    }


    [ServerRpc(RequireOwnership = false)]
    public void InstanciateRoomServer(RoomScriptableObject roomToInstanciate, GameObject lastRoom, int level, RoomManager roomManager)
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
        spawned.GetComponent<RoomData>().level = level;

        ServerManager.Despawn(lastGameRoom);
        ServerManager.Spawn(spawned);

        SetInstanciateRoom(spawned, roomToInstanciate.m_name, level, roomManager);
     }

    [ObserversRpc]
    public void SetInstanciateRoom(GameObject spawnedRoom, string templateName, int roomLevel, RoomManager roomManager)
    {
        Debug.Log("Client instanciation");
        spawnedRoom.GetComponent<RoomData>().level = roomLevel;
        spawnedRoom.GetComponent<RoomData>().template = roomDeck.GetRoom(templateName);
        if (currentRoom)
        {
            currentRoom.transform.Find("Exit").GetComponentInChildren<AudioSource>().Play();
            currentRoom.transform.Find("Exit").GetComponentInChildren<Animator>().SetTrigger("Lift");
        }

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

    public RoomDeckScriptableObject GetDeck()
    {
        return roomDeck;
    }
}
