using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Statistics

    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int hp;

    #endregion

    #region Internal Variables

    private bool isInvulnerable = false;
    [SerializeField]
    private float invulnerableDuration;
    private float invulnerableClock;

    #endregion

    private void Start()
    {
        hp = maxHp;
    }

    private void Update()
    {
        FadeInvulnerable();
    }

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