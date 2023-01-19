using FishNet.Object;
using FishNet.Object.Synchronizing;
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
    public RoomManager roomManager;

    public GameObject playerObject;
    public GameObject gameMasterObject;

    [SyncVar]
    public float roomTimer;

    public void Start()
    {
        instance = this;
        roomManager = GetComponent<RoomManager>();
    }

    public void StartGame()
    {
        Destroy(GameObject.FindGameObjectWithTag("Pregame").gameObject);
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
        roomTimer = challengeTimer;
        instance.SetRoomState(GamePhase.ROOM);
        instance.gameMasterObject.GetComponent<GameMasterInteraction>().SetGamePhase(GamePhase.ROOM);
        StartCoroutine(RoomClock(challengeTimer));
    }

    private IEnumerator RoomClock(float challengeTimer)
    {
        for(int i = 0; i <= (int)challengeTimer; i++)
        {
            roomTimer = challengeTimer - i;
            yield return new WaitForSeconds(1);
        }
        EndARoom();
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndARoom()
    {
        instance.SetRoomState(GamePhase.REST);
        instance.gameMasterObject.GetComponent<GameMasterInteraction>().SetGamePhase(GamePhase.REST);
        roomManager.InstanciateRestRoomSchedule();
        playerObject.GetComponent<Player>().AddPoint(roomManager.GetCurrentRoom().GetComponent<RoomData>().level * 2);
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

    public GamePhase GetGamePhase()
    {
        return gamePhase;
    }
}
