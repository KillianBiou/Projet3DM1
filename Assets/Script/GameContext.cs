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

    private void Update()
    {
        debugGamePhase = gamePhase;
    }

    public static void StartARoom()
    {
        gamePhase = GamePhase.ROOM;
    }

    public static void EndARoom()
    {
        gamePhase = GamePhase.REST;
    }
}
