using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterInteraction : MonoBehaviour
{
    private void Update()
    {
        for(int i = (int)KeyCode.Alpha1; i <= (int)KeyCode.Alpha9; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                GameContext.instance.ActivateTrap((KeyCode)i);
            }
        }
    }
}
