using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTrap : MonoBehaviour
{
    #region Parameters

    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float upTime;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float slowPercentage;

    #endregion
    #region Internal Parameters

    private Player player;
    private TrapState currentState = TrapState.COOLDOWN;
    private float currentTimer;
    private bool hasSlowTarget = false;
    //private Animator animator;

    #endregion

    private void Start()
    {
        //animator = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (currentState == TrapState.COOLDOWN)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > cooldown)
            {
                currentTimer = 0;
                currentState = TrapState.UP;
                GetComponent<MeshRenderer>().material.color = Color.yellow;
                //animator.SetTrigger("Extend");
                //animator.ResetTrigger("Retract");
            }
        }

        if (currentState == TrapState.UP)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > upTime)
            {
                currentTimer = 0;
                currentState = TrapState.COOLDOWN;
                GetComponent<MeshRenderer>().material.color = Color.white;
                RefreshSlow(false);
                //animator.SetTrigger("Retract");
                //animator.ResetTrigger("Extend");
            }
        }
    }

    public void HitEffect(Player player)
    {
        player.TakeDamage(damage);
        RefreshSlow(true);
    }

    private void RefreshSlow(bool state)
    {
        if (state && !hasSlowTarget)
        {
            hasSlowTarget = true;
            player.GetComponent<FirstPersonController>().cumulativeSlow += slowPercentage;
        }
        else if(!state && hasSlowTarget)
        {
            hasSlowTarget = false;
            player.GetComponent<FirstPersonController>().cumulativeSlow -= slowPercentage;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentState == TrapState.UP && other.CompareTag("Player"))
        {
            HitEffect(other.GetComponentInParent<Player>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentState == TrapState.UP && other.CompareTag("Player"))
        {
            RefreshSlow(false);
        }
    }
}