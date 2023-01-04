using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomDeckScriptableObject", menuName = "ScriptableObject/RoomDeck")]
public class RoomDeckScriptableObject : ScriptableObject
{
    public List<RoomScriptableObject> m_rooms;

    public RoomScriptableObject GetRoom(string roomName)
    {
        foreach (RoomScriptableObject room in m_rooms)
        {
            if(room.name == roomName) return room;
        }
        return null;
    }
}
