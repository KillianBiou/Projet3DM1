using UnityEngine;

public class RoomData : MonoBehaviour
{
    public RoomScriptableObject template { get; set; }
    public RoomPhase hasStarted { get; set; }
    public int level { get; set; }

    private float currentClock = 0f;

    private void Start()
    {
        Debug.Log("Lvl : " + level);
        for (int i = template.m_maxLevel; i > level; i--)
        {
            Transform trapContainer = transform.Find("Trap").Find("LVL" + i);
            if(trapContainer)
            {
                trapContainer.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (hasStarted == RoomPhase.STARTED)
        {
            currentClock += Time.deltaTime;
            if (currentClock >= template.m_timer)
            {
                ChangeRoomPhase(RoomPhase.ENDED);
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

    public void ChangeRoomPhase(RoomPhase phase)
    {
        switch(phase)
        {
            case RoomPhase.STARTED:
                hasStarted = RoomPhase.STARTED;
                GameContext.StartARoom();
                transform.Find("Entry").GetComponent<BoxCollider>().isTrigger = false;
                transform.Find("Entry").GetComponent<BoxCollider>().center = Vector3.zero + Vector3.up * 5;
                foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
                {
                    mr.material.color = Color.red;
                }
                break;

            case RoomPhase.ENDED:
                hasStarted = RoomPhase.ENDED;
                GameContext.EndARoom();
                Destroy(transform.Find("Exit").GetComponent<BoxCollider>());
                foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
                {
                    mr.material.color = Color.white;
                }
                break;
        }
    }
}
