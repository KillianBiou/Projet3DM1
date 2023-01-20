using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentActivator : MonoBehaviour
{
    [SerializeField]
    public List<Behaviour> componentsToActivate = new List<Behaviour>();

    public void StartGame()
    {
        foreach (Behaviour component in componentsToActivate)
        {
            if(component)
                component.enabled = true;
        }
        Destroy(this);
    }
}
