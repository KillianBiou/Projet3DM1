using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameContext.instance.ChangeRoomStateServer(GamePhase.REST);
            CloseRestRoom();
        }
    }

    private void CloseRestRoom()
    {
        transform.Find("Entry").GetChild(0).gameObject.SetActive(true);
    }
}
