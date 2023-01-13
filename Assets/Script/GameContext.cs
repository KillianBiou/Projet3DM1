using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    REST,
    ROOM
}

public enum RoomPhase
{
    NONE,
    STARTED,
    ENDED
}

public class GameContext : NetworkBehaviour
{
    public static GameContext instance;

    [SyncVar]
    private GamePhase gamePhase;

    [SyncVar]
    public GameObject playerObject;

    private void Start()
    {
        instance = this;
    }

    public static void SetPlayer(GameObject player)
    {
        instance.playerObject = player;
    }

    public static void StartARoom()
    {
        instance.gamePhase = GamePhase.ROOM;
        instance.playerObject.GetComponent<PlayerInteraction>().SetCanInteract(false);
    }

    public static void EndARoom()
    {
        instance.gamePhase = GamePhase.REST;
        instance.playerObject.GetComponent<PlayerInteraction>().SetCanInteract(true);
    }
}
