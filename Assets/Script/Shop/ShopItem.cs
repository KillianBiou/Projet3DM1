using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    #region Parameters
    [SerializeField]
    private string itemName;

    [SerializeField]
    private string itemDescription = "Placeholder";

    [SerializeField]
    private Modifier effect;

    #endregion
    #region Internal Parameters

    private GameObject UICanvas;

    #endregion

    private void Start()
    {
        UICanvas = transform.GetChild(0).gameObject;

        UICanvas.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = itemName;
        UICanvas.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = itemDescription;
    }

    public void FetchShopInformation()
    {
        Debug.Log("You are looking at : " + itemName);
        SetShopUI(true);
    }

    public void SetShopUI(bool state)
    {
        UICanvas.SetActive(state);
    }

    public void BuyItem()
    {
        Debug.Log("Bought : " + itemName);
    }
}
