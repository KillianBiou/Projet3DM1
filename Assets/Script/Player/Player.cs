using StarterAssets;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class Player : NetworkBehaviour
{
    #region Statistics

    [SerializeField]
    [SyncVar]
    private int maxHp;

    [SerializeField]
    [SyncVar]
    private int hp;

    #endregion

    #region Internal Variables

    private bool isInvulnerable = false;
    [SerializeField]
    private float invulnerableDuration;
    private float invulnerableClock;

    #endregion

    public override void OnStartClient()
    {
        base.OnStartClient();
        GameContext.instance.SetPlayer(gameObject);
    }

    private void Start()
    {
        hp = maxHp;
    }

    private void Update()
    {
        FadeInvulnerable();
    }

    public void StartGame()
    {
        if (base.IsOwner)
        {
            Debug.Log("Owner of player");
            GetComponent<CharacterController>().enabled = true;
            GetComponent<ComponentActivator>().StartGame();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage(int amount)
    {
        if (!isInvulnerable)
        {
            hp -= amount;
            Debug.Log("Took " + amount + " damage.");
            if (hp <= 0)
            {
                Debug.Log("DEAD");
            }
            isInvulnerable = true;
            invulnerableClock = invulnerableDuration;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void Heal(int amount)
    {
        hp += amount;
        Debug.Log("Healed " + amount + " HP.");
        if (hp >= maxHp)
        {
            hp = maxHp;
        }
    }

    public void FadeInvulnerable()
    {
        if (isInvulnerable)
        {
            invulnerableClock -= Time.deltaTime;
            if (invulnerableClock <= 0)
            {
                isInvulnerable = false;
            }
        }
    }

    public void ProcessShopItem(Modifier modifier)
    {
        switch(modifier.type)
        {
            case ModifierType.MOVEMENT_SPEED:
                GetComponent<FirstPersonController>().moveSpeedBuff.Add(modifier);
                break;
            case ModifierType.HP:
                Heal(modifier.value);
                break;
        }
    }
}