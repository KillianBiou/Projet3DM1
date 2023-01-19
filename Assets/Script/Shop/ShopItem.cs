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
        Debug.Log("Bought : " + itemName);
        transform.parent.GetComponentInParent<Shop>().Buy();
        GameContext.instance.GetPlayer().GetComponent<Player>().ProcessShopItem(effect);
    }
}
