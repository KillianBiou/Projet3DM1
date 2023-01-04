using UnityEngine;

[CreateAssetMenu(fileName = "RoomScriptableObject", menuName = "ScriptableObject/Room")]
public class RoomScriptableObject : ScriptableObject
{
    public string m_name;
    public GameObject m_template;
    public float m_timer;
}
