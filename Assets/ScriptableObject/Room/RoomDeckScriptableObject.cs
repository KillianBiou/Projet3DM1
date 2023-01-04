using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomDeckScriptableObject", menuName = "ScriptableObject/RoomDeck")]
public class RoomDeckScriptableObject : ScriptableObject
{
    public List<RoomScriptableObject> m_rooms;
}
