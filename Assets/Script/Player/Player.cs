using StarterAssets;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

public class Player : NetworkBehaviour
{
    #region Statistics

    [SerializeField]
    [SyncVar]
    private int maxHp = 5;

    [SerializeField]
    [SyncVar]
    private int points;

    [SerializeField]
    [SyncVar]
    private int hp;

    [SerializeField]
    private bool haveShield;

    #endregion

    #region Internal Variables

    [SyncVar]
    private bool isInvulnerable = false;
    [SerializeField]
    private float invulnerableDuration;

    [SerializeField]
    private bool hasCutTrap;
    [SerializeField]
    private bool hasBlind;
    [SyncVar]
    private GameObject trapTarget;

    private PlayerUI playerUI;
    private StarterAssetsInputs playerInput;
    private Camera camera;

    private ParticleSystem shieldSystem;

    #endregion



    public override void OnStartClient()
    {
        base.OnStartClient();
        GameContext.instance.SetPlayer(gameObject);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        hp = maxHp;
    }

    private void Update()
    {
        FadeInvulnerable();
        if (base.IsOwner)
        {
            if(hasCutTrap)
                CheckTrapTarget();
            UseSkills();
        }
    }


    public void StartGame()
    {
        if (base.IsOwner)
        {
            Debug.Log("Owner of player");
            GetComponent<CharacterController>().enabled = true;
            playerUI = transform.Find("CanvasPlayer").GetComponent<PlayerUI>();
            playerUI.gameObject.SetActive(true);
            GetComponent<ComponentActivator>().StartGame();
            playerInput = GetComponent<StarterAssetsInputs>();
            camera = transform.Find("PlayerCameraRoot").GetComponentInChildren<Camera>();
        }
        shieldSystem = GetComponent<ParticleSystem>();
        SetShield(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage(int amount)
    {
        if (!isInvulnerable)
        {
            if (haveShield)
            {
                isInvulnerable = true;
                StartCoroutine(FadeInvulnerable());
                SetShield(false);
            }
            else
            {
                isInvulnerable = true;
                hp -= amount;
                if (hp <= 0)
                {
                    Debug.Log("DEAD");
                }
                StartCoroutine(FadeInvulnerable());
            }
        }
    }

    private void UseSkills()
    {
        if (playerInput.CutTrap && hasCutTrap)
        {
            Debug.Log("Use cut trap");
            if (trapTarget)
            {
                DestroyNetworkTrapTarget();
                trapTarget = null;
                hasCutTrap = false;
            }
        }
        if (playerInput.Blind && hasBlind)
        {
            Debug.Log("Use Blind");
            hasBlind = false;
            GameContext.instance.gameMasterObject.GetComponent<GameMaster>().CutCamera(5);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyNetworkTrapTarget()
    {
        ServerManager.Despawn(trapTarget);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetShield(bool shield)
    {
        haveShield = shield;
        RefreshShield(shield);
    }

    [ObserversRpc]
    public void RefreshShield(bool value)
    {
        if (value)
        {
            shieldSystem.Play();
        }
        else
        {
            shieldSystem.Stop();
        }
        haveShield = value;
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

    [ServerRpc(RequireOwnership = false)]
    private void SetInvulnerable(bool invulnerable)
    {
        isInvulnerable = invulnerable;
    }

    public IEnumerator FadeInvulnerable()
    {
        yield return new WaitForSeconds(invulnerableDuration);
        SetInvulnerable(false);
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
            case ModifierType.SHIELD:
                SetShield(true);
                break;
            case ModifierType.CUT_TRAP:
                hasCutTrap = true;
                break;
            case ModifierType.BLIND:
                hasBlind = true;
                break;
        }
    }

    private void CheckTrapTarget()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out raycastHit, 50, 1 << 13))
        {
            SetNewTargetLook(raycastHit.transform.gameObject);
        }
        else
        {
            trapTarget = null;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetNewTargetLook(GameObject trap)
    {
        if (trap != trapTarget)
        {
            Debug.Log("Change target");
            trapTarget = trap;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddPoint(int amount)
    {
        this.points += amount;
    }

    public int GetHP()
    {
        return hp;
    }

    public int GetPoints()
    {
        return points;
    }

    public bool GetHasCutTrap()
    {
        return hasCutTrap;
    }

    public bool GetHasBlind()
    {
        return hasBlind;
    }

    public bool GetHaveShield()
    {
        return haveShield;
    }
}