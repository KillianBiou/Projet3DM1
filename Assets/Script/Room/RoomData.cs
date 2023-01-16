using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public RoomScriptableObject template { get; set; }
    public RoomPhase hasStarted { get; set; }
    public int level { get; set; }

    public Dictionary<KeyCode, List<TrapInteraction>> interactions = new();

    private float currentClock = 0f;

    private void Start()
    {
        Debug.Log("Lvl : " + level);
        for (int i = 3; i > level; i--)
        {
            Transform trapContainer = transform.Find("Trap").Find("LVL" + i);
            if(trapContainer)
            {
                trapContainer.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            ChangeRoomPhase(RoomPhase.STARTED);
        }
    }

    public void TriggerStart()
    {
        GameContext.instance.StartARoom(10f);
    }

    public void RegisterTrap(KeyCode code, TrapInteraction trap)
    {
        if (!interactions.ContainsKey(code))
            interactions.Add(code, new List<TrapInteraction>());
        interactions[code].Add(trap);
        Debug.Log("Added new trap with keycode" + code);
    }

    public void ActivateTrap(KeyCode code)
    {
        Debug.Log("Activate traps with " + code);
        if (interactions.ContainsKey(code))
        {
            foreach (TrapInteraction trap in interactions[code])
            {
                trap.Activation();
            }
        }
    }

    public void ChangeRoomPhase(RoomPhase phase)
    {
        switch(phase)
        {
            case RoomPhase.STARTED:
                hasStarted = RoomPhase.STARTED;
                transform.Find("Entry").GetComponent<BoxCollider>().isTrigger = false;
                transform.Find("Entry").GetComponent<BoxCollider>().center = Vector3.zero + Vector3.up * 5;
                foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
                {
                    mr.material.color = Color.red;
                }
                break;

            case RoomPhase.ENDED:
                hasStarted = RoomPhase.ENDED;
                Destroy(transform.Find("Exit").GetComponent<BoxCollider>());
                foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
                {
                    mr.material.color = Color.white;
                }
                break;
        }
    }
}
