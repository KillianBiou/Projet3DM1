using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterInteraction : MonoBehaviour
{
    private RoomCard card1;
    private RoomCard card2;
    private RoomCard card3;

    private Transform cardHolder;

    private GamePhase currentGamePhase;

    private void Start()
    {
        cardHolder = transform.Find("CardUI");

        card1 = cardHolder.Find("Card1").GetComponent<RoomCard>();
        card1.activationKey = KeyCode.Alpha1;

        card2 = cardHolder.Find("Card2").GetComponent<RoomCard>();
        card2.activationKey = KeyCode.Alpha2;

        card3 = cardHolder.Find("Card3").GetComponent<RoomCard>();
        card3.activationKey = KeyCode.Alpha3;
    }


    private void Update()
    {
        if(currentGamePhase == GamePhase.ROOM)
        {
            for (int i = (int)KeyCode.Alpha1; i <= (int)KeyCode.Alpha9; i++)
            {
                if (Input.GetKeyDown((KeyCode)i))
                {
                    GameContext.instance.ActivateTrap((KeyCode)i);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                card1.RequestConstruction();
                cardHolder.gameObject.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                card2.RequestConstruction();
                cardHolder.gameObject.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                card3.RequestConstruction();
                cardHolder.gameObject.SetActive(false);
            }
        }
    }

    public void SetGamePhase(GamePhase newPhase)
    {
        if(newPhase == GamePhase.REST)
        {

            card1.RandomRoom(GameContext.instance.roomManager.GetDeck());
            card2.RandomRoom(GameContext.instance.roomManager.GetDeck());
            card3.RandomRoom(GameContext.instance.roomManager.GetDeck());
            cardHolder.gameObject.SetActive(true);
        }
        currentGamePhase = newPhase;
    }
}
