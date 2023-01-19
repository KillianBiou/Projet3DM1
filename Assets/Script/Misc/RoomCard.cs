using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RoomCard : MonoBehaviour
{
    private RoomDeckScriptableObject deck;
    
    private RoomScriptableObject template;

    private int level;

    private SerializedDictionary<string, int> trapInventory = new();

    public KeyCode activationKey { get; set; }

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

        Debug.Log(activationKey.ToString());
        mapTemplate.Find("KeyTxt").GetComponentInChildren<TextMeshProUGUI>().text = Regex.Replace(activationKey.ToString(), @"[a-zA-Z]", "");
    }

    public void RequestConstruction()
    {
        GameContext.instance.roomManager.RequestRoomConstruction(template, level);
    }
}
