using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    #region Parameters

    
    #endregion

    #region Internal Parameters

    private Camera playerCamera;
    private ShopItem currentItemLook;
    private bool canInteract = true;

    #endregion

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        currentItemLook = null;
    }

    void Update()
    {
        if (canInteract)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out raycastHit, 10, 1 << 6))
            {
                SetNewTargetLook(raycastHit.transform.GetComponent<ShopItem>());
            }
            else
            {
                if (currentItemLook)
                    currentItemLook.SetShopUI(false);
                currentItemLook = null;
            }
        }
    }

    private void SetNewTargetLook(ShopItem shopItem)
    {
        if(shopItem != currentItemLook)
        {
            if(currentItemLook)
                currentItemLook.SetShopUI(false);
            currentItemLook = shopItem;
            shopItem.FetchShopInformation();
        }
    }

    public void SetCanInteract(bool canInteract)
    {
        this.canInteract = canInteract;
    }

    public void OnInteraction(InputValue value)
    {
        if(canInteract && currentItemLook)
            currentItemLook.BuyItem();
    }
}
