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
            if (hp <= 0)
            {
                Debug.Log("DEAD");
            }
            isInvulnerable = true;
            invulnerableClock = invulnerableDuration;
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
}