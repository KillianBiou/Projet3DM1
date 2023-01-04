using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "OffensiveTrapScriptableObject", menuName = "ScriptableObject/Trap/Offensive")]

public class OffensiveTrapScriptableObject : ScriptableObject
{
    public string m_name;
    public int m_damage;
    public float m_cooldown;
    public KeyCode m_activationKey;
}
