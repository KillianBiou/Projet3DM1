using FishNet.Object;
using UnityEngine;

public class GameMaster : NetworkBehaviour
{
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
        }
    }

}
