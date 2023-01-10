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

public class GameContext : MonoBehaviour
{
    private static GamePhase gamePhase;

    [SerializeField]
    private GamePhase debugGamePhase;

    private static GameObject playerObject;

    private void Start()
    {
        playerObject = GameObject.FindObjectOfType<Player>().gameObject;
    }

    private void Update()
    {
        debugGamePhase = gamePhase;
    }

    public static void StartARoom()
    {
        gamePhase = GamePhase.ROOM;
        playerObject.GetComponent<PlayerInteraction>().SetCanInteract(false);
    }

    public static void EndARoom()
    {
        gamePhase = GamePhase.REST;
        playerObject.GetComponent<PlayerInteraction>().SetCanInteract(true);
    }
}
