using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RoomScriptableObject", menuName = "ScriptableObject/Room")]
public class RoomScriptableObject : ScriptableObject
{
    public string m_name;
    public int m_maxLevel;
    public GameObject m_template;
    public float m_timer;

    public SerializedDictionary<string, int> GetTrapCount(int level)
    {
        SerializedDictionary<string, int> traps = new();

        for (int i = 1; i <= level; i++)
        {
            Transform trapContainer = m_template.transform.Find("Trap").Find("LVL" + i);
            if (trapContainer)
            {
                foreach (Transform trap in trapContainer)
                {
                    switch (trap.tag)
                    {
                        case "ArrowTrap":
                            if (!traps.ContainsKey("Arrow Thrower"))
                                traps["Arrow Thrower"] = 0;
                            traps["Arrow Thrower"] += 1;
                            break;
                        case "BarrelTrap":
                            if (!traps.ContainsKey("Explosive Barrel"))
                                traps["Explosive Barrel"] = 0;
                            traps["Explosive Barrel"] += 1;
                            break;
                        case "ElectricTrap":
                            if (!traps.ContainsKey("Electric Trap"))
                                traps["Electric Trap"] = 0;
                            traps["Electric Trap"] += 1;
                            break;
                        case "GazTrap":
                            if (!traps.ContainsKey("Gaz Trap"))
                                traps["Gaz Trap"] = 0;
                            traps["Gaz Trap"] += 1;
                            break;
                        case "SpikeTrap":
                            if (!traps.ContainsKey("Spike Trap"))
                                traps["Spike Trap"] = 0;
                            traps["Spike Trap"] += 1;
                            break;
                        case "TurretTrap":
                            if (!traps.ContainsKey("Turret"))
                                traps["Turret"] = 0;
                            traps["Turret"] += 1;
                            break;
                        case "WindTrap":
                            if (!traps.ContainsKey("Wind Blower"))
                                traps["Wind Blower"] = 0;
                            traps["Wind Blower"] += 1;
                            break;
                    }
                }
            }
        }

        return traps;
    }
}
