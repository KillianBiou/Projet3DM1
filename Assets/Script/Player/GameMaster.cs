using FishNet.Object;
using System.Collections;
using UnityEngine;

public class GameMaster : NetworkBehaviour
{
    private GameObject cutCameraObject;
    private GameObject GMUI;

    public override void OnStartClient()
    {
        base.OnStartClient();
        GameContext.instance.SetGameMaster(gameObject);
    }

    public void StartGame()
    {
        if (base.IsOwner)
        {
            Debug.Log("Owner of GameMaster");
            GetComponent<ComponentActivator>().StartGame();

            GMUI = transform.Find("GMUI").gameObject;
            GMUI.SetActive(true);

            cutCameraObject = GMUI.transform.Find("NoiseEffect").gameObject;
            cutCameraObject.SetActive(false);

            GetComponent<GameMasterInteraction>().SetGamePhase(GamePhase.REST);
            GameContext.instance.roomManager.RequestEndRoomConstruction(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CutCamera(int seconds)
    {
        StartCoroutine(Clock(seconds));
    }

    [ObserversRpc]
    private void SetCameraCut(bool state)
    {
        if(base.IsOwner)
            cutCameraObject.SetActive(state);
    }

    private IEnumerator Clock(int seconds)
    {
        SetCameraCut(true);
        yield return new WaitForSeconds(seconds);
        SetCameraCut(false);
    }
}
