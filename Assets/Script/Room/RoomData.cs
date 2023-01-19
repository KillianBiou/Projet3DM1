using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public RoomScriptableObject template { get; set; }
    public RoomPhase hasStarted { get; set; }
    public int level { get; set; }
    public Sprite icon;

    public Dictionary<KeyCode, List<TrapInteraction>> interactions = new();

    private float currentClock = 0f;

    private GameObject entry;
    private GameObject exit;

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
        entry = transform.Find("Entry").gameObject;

        exit = transform.Find("Exit").gameObject;
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
        GameContext.instance.StartARoom(template.m_timer);
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
                foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
                {
                    mr.material.color = Color.red;
                }
                //entry.GetComponentInChildren<Animator>().SetTrigger("Close");
                break;

            case RoomPhase.ENDED:
                hasStarted = RoomPhase.ENDED;
                foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
                {
                    mr.material.color = Color.white;
                }
                exit.GetComponentInChildren<Animator>().SetTrigger("Lift");
                break;
        }
    }
}
