using UnityEngine;

[CreateAssetMenu(fileName = "RoomScriptableObject", menuName = "ScriptableObject/Room")]
public class RoomScriptableObject : ScriptableObject
{
    public string m_name;
    public int m_maxLevel;
    public GameObject m_template;
    public float m_timer;
}
