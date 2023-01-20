using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

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

    public int roomToComplete = 1;
    [SyncVar]
    public int roomCompleted;

    private GamePhase gamePhase = GamePhase.MENU;
    public RoomManager roomManager;

    public GameObject playerObject;
    public GameObject gameMasterObject;

    [SyncVar]
    public float roomTimer;

    [SerializeField]
    private Canvas fadeOutCanvas;

    private Canvas fadeOutInstance;

    [SerializeField]
    private AudioSource ringSource;
    [SerializeField]
    private AudioSource jingle;
    [SerializeField]
    private AudioSource supreme;
    [SerializeField]
    private AudioSource restRoomMusic;

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

        jingle.Play();
        supreme.Play();
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
    public void StartEndServer()
    {
        StartEnd();
    }

    [ObserversRpc]
    private void StartEnd()
    {
        fadeOutInstance = Instantiate(fadeOutCanvas);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Image fadeOutImage = fadeOutInstance.transform.Find("FadeOut").GetComponent<Image>();
        for (int i = 0; i < 500; i += 2)
        {
            fadeOutImage.color = new Color(0, 0, 0, Mathf.Clamp(i / 350f, 0, 1));
            yield return new WaitForNextFrameUnit();
        }
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        if (playerObject.GetComponent<Player>().GetHP() > 0)
            fadeOutInstance.transform.Find("PlayerWinButGMWinAnyway").GetComponent<Image>().enabled = true;
        else
            fadeOutInstance.transform.Find("GMWin").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(5);
        Application.Quit(0);
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartARoom(float challengeTimer)
    {
        roomTimer = challengeTimer;
        instance.SetRoomState(GamePhase.ROOM);
        instance.gameMasterObject.GetComponent<GameMasterInteraction>().SetGamePhase(GamePhase.ROOM);
        StartCoroutine(RoomClock(challengeTimer));
        restRoomMusic.Stop();
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
        playerObject.GetComponent<Player>().AddPoint(roomManager.GetCurrentRoom().GetComponent<RoomData>().level * 2);
        
        roomCompleted++;
        if(roomCompleted >= roomToComplete)
        {
            roomManager.RequestEndRoomConstruction(true);
        }
        else
        {
            roomManager.InstanciateRestRoomSchedule();
            instance.SetRoomState(GamePhase.REST);
            instance.gameMasterObject.GetComponent<GameMasterInteraction>().SetGamePhase(GamePhase.REST);
            restRoomMusic.Play();
        }
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
            ringSource.Play();
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
