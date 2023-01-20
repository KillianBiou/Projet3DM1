using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    #region Parameters
    [SerializeField]
    private string itemName;

    [SerializeField]
    private string itemDescription = "Placeholder";

    [SerializeField]
    private int cost = 1;

    [SerializeField]
    private Modifier effect;

    private bool canBuy;

    #endregion
    #region Internal Parameters

    private GameObject UICanvas;
    private Player player;

    #endregion

    private void Start()
    {
        player = GameContext.instance.playerObject.GetComponent<Player>();
        UICanvas = transform.GetChild(0).gameObject;

        RefreshOffer();
    }

    private void RefreshOffer()
    {
        bool canBuy = true;
        switch(effect.type)
        {
            case ModifierType.SHIELD:
                if (player.GetHaveShield())
                    canBuy = false;
                break;
            case ModifierType.CUT_TRAP:
                if(player.GetHasCutTrap())
                    canBuy = false;
                break;
            case ModifierType.BLIND:
                if(player.GetHasBlind())
                    canBuy = false;
                break;
        }
        UICanvas.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = itemName;
        if (!canBuy)
        {
            UICanvas.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = "Out of stock";
            UICanvas.transform.Find("Price").gameObject.SetActive(false);
            UICanvas.transform.Find("Unit").gameObject.SetActive(false);
        }
        else
        {
            UICanvas.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = itemDescription;
            UICanvas.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = cost.ToString();
        }
        this.canBuy = canBuy;
    }

    public void FetchShopInformation()
    {
        Debug.Log("You are looking at : " + itemName);
        SetShopUI(true);
    }

    public IEnumerator ScaleUpAnimation()
    {
        for (int i = 0; i <= 100; i += 5)
        {
            UICanvas.GetComponent<RectTransform>().localScale = (Vector3.one * (i / 100f)) * 5;
            yield return new WaitForNextFrameUnit();
        }
    }

    public IEnumerator ScaleDownAnimation()
    {
        for (int i = 100; i >= 100; i -= 5)
        {
            UICanvas.GetComponent<RectTransform>().localScale = (Vector3.one * (i / 100f)) * 5;
            yield return new WaitForNextFrameUnit();
        }
    }

    public void SetShopUI(bool state)
    {
        if (state)
            StartCoroutine(ScaleUpAnimation());
        else
            StartCoroutine(ScaleDownAnimation());

        UICanvas.SetActive(state);
    }

    public void BuyItem()
    {
        if (canBuy)
        {
            Debug.Log("Bought : " + itemName);
            transform.parent.GetComponentInParent<Shop>().Buy();
            GameContext.instance.GetPlayer().GetComponent<Player>().ProcessShopItem(effect, cost);

            RefreshOffer();
        }
    }
}
