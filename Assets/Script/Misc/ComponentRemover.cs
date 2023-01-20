using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class ComponentRemover : NetworkBehaviour
{
    [SerializeField]
    public List<Component> componentsToRemove = new List<Component>();

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            foreach (Component component in componentsToRemove)
            {
                Destroy(component);
            }
        }
        Destroy(this);
    }
}
