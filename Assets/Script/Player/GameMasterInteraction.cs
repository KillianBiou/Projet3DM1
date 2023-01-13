using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterInteraction : MonoBehaviour
{
    #region Internal Parameters

    private Dictionary<KeyCode, List<TrapInteraction>> activationDict = new Dictionary<KeyCode, List<TrapInteraction>>();

    #endregion

    public void RegisterTrap(KeyCode activationKey, TrapInteraction trap)
    {
        activationDict[activationKey].Add(trap);
    }

    public void ClearActivationList()
    {
        activationDict.Clear();
    }
}
