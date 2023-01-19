using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RoomCard : MonoBehaviour
{
    [SerializeField]
    private RoomDeckScriptableObject deck;
    
    [SerializeField]
    private RoomScriptableObject template;

    [SerializeField]
    private int level;

    [SerializeField]
    private SerializedDictionary<string, int> trapInventory = new();

    private void Start()
    {
        RandomRoom(deck);
    }

    public void RandomRoom(RoomDeckScriptableObject roomDeck)
    {
        template = roomDeck.m_rooms[UnityEngine.Random.Range(0, roomDeck.m_rooms.Count)];
        level = UnityEngine.Random.Range(1, template.m_maxLevel + 1);

        trapInventory = template.GetTrapCount(level);

        Transform mapTemplate = transform.Find("MapTemplate").Find("MapTxT");

        mapTemplate.Find("LvLTxT").GetComponentInChildren<TextMeshProUGUI>().text = "LVL " + level.ToString();
        mapTemplate.Find("TimerArea").GetComponentInChildren<TextMeshProUGUI>().text = TimeSpan.FromSeconds(80).ToString(@"mm\:ss");

        string trap = "";
        foreach(KeyValuePair<string, int> entry in trapInventory)
        {
            trap += entry.Value.ToString() + " X " + entry.Key + "\n";
        }
        mapTemplate.Find("TrapArea").GetComponentInChildren<TextMeshProUGUI>().text = trap;

        mapTemplate.Find("PointsArea").GetComponentInChildren<TextMeshProUGUI>().text = (level * 2).ToString() + " PTS";

        mapTemplate.Find("RoomArea").GetComponentInChildren<Image>().sprite = template.m_template.GetComponent<RoomData>().icon;
    }
}
