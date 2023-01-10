using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public enum TrapState
{
    COOLDOWN,
    UP,
    CAN_BE_USED
}

public class SpikeTrap : MonoBehaviour
{
    #region Parameters

    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float upTime;
    [SerializeField]
    private int damage;

    #endregion
    #region Internal Parameters

    private TrapState currentState = TrapState.COOLDOWN;
    private float currentTimer;
    private Animator animator;

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(currentState == TrapState.COOLDOWN)
        {
            currentTimer += Time.deltaTime;
            if(currentTimer > cooldown)
            {
                currentTimer = 0;
                currentState = TrapState.UP;
                animator.SetTrigger("Extend");
                animator.ResetTrigger("Retract");
            }
        }

        if (currentState == TrapState.UP)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > upTime)
            {
                currentTimer = 0;
                currentState = TrapState.COOLDOWN;
                animator.SetTrigger("Retract");
                animator.ResetTrigger("Extend");
            }
        }
    }

    public void HitEffect(Player player)
    {
        player.TakeDamage(damage);
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentState == TrapState.UP && other.CompareTag("Player"))
        {
            HitEffect(other.GetComponentInParent<Player>());
        }
    }
}
