using FishNet.Object;
using System.Collections;
using UnityEngine;

public enum GamePhase
{
    MENU,
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

    private GamePhase gamePhase = GamePhase.MENU;
    private RoomManager roomManager;

    public GameObject playerObject;
    public GameObject gameMasterObject;

    public void Start()
    {
        instance = this;
        roomManager = GetComponent<RoomManager>();
    }

    public void StartGame()
    {
        gamePhase = GamePhase.REST;

        roomManager.enabled = true;

        playerObject.GetComponent<Player>().StartGame();
        gameMasterObject.GetComponent<GameMaster>().StartGame();
    }

    public void SetPlayer(GameObject player)
    {
        instance.playerObject = player;
        if (gameMasterObject)
            StartGame();
    }

    public void SetGameMaster(GameObject gamemaster)
    {
        instance.gameMasterObject = gamemaster;
        if (playerObject)
            StartGame();
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartARoom(float challengeTimer)
    {
        instance.SetRoomState(GamePhase.ROOM);
        StartCoroutine(RoomClock(challengeTimer));
    }

    private IEnumerator RoomClock(float challengeTimer)
    {
        yield return new WaitForSeconds(challengeTimer);
        EndARoom();
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndARoom()
    {
        instance.SetRoomState(GamePhase.REST);
    }

    [ObserversRpc]
    public void SetRoomState(GamePhase phase)
    {
        if(phase == GamePhase.ROOM) {
            roomManager.GetCurrentRoom().GetComponent<RoomData>().ChangeRoomPhase(RoomPhase.STARTED);
            if (instance.playerObject.GetComponent<PlayerInteraction>())
                instance.playerObject.GetComponent<PlayerInteraction>().SetCanInteract(false);
        }
        else
        {
            roomManager.GetCurrentRoom().GetComponent<RoomData>().ChangeRoomPhase(RoomPhase.ENDED);
            if (instance.playerObject.GetComponent<PlayerInteraction>())
                instance.playerObject.GetComponent<PlayerInteraction>().SetCanInteract(true);
        }
        gamePhase = phase;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ActivateTrap(KeyCode code)
    {
        ActivateTrapClient(code);
    }

    [ObserversRpc]
    public void ActivateTrapClient(KeyCode code)
    {
        roomManager.GetCurrentRoom().GetComponent<RoomData>().ActivateTrap(code);
    }

    public GameObject GetPlayer()
    {
        return instance.playerObject;
    }
}
