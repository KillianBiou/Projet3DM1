using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            GetComponentInParent<RoomData>().TriggerStart();
            Destroy(this);
        }
    }
}
