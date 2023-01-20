using UnityEngine;
using FishNet.Object;

public class Instanciator : NetworkBehaviour
{
    [SerializeField]
    private GameObject m_player;
    [SerializeField]
    private GameObject m_gamemaster;

    void Start()
    {
        if (base.IsHost)
        {
            Instantiate(m_gamemaster, transform.position, m_gamemaster.transform.rotation, transform);
        }
        else if (base.IsClient)
        {
            Instantiate(m_player, transform.position, m_player.transform.rotation, transform);
        }
        Destroy(this);
    }
}
