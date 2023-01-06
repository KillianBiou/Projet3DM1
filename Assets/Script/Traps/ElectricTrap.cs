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
    private Debuff slow;

    #endregion
    #region Internal Parameters

    private Player player;
    private TrapState currentState = TrapState.COOLDOWN;
    private float currentTimer;
    private bool hasSlowTarget = false;
    private GameObject VFXContainer;

    #endregion

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        VFXContainer = transform.Find("VFX").gameObject;
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
                VFXContainer.SetActive(true);
            }
        }

        if (currentState == TrapState.UP)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > upTime)
            {
                currentTimer = 0;
                currentState = TrapState.COOLDOWN;
                GetComponent<MeshRenderer>().material.color = Color.white ;
                VFXContainer.SetActive(false);
                RefreshSlow(false);
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
            player.GetComponent<FirstPersonController>().slowDebuff.Add(slow);
        }
        else if(!state && hasSlowTarget)
        {
            hasSlowTarget = false;
            player.GetComponent<FirstPersonController>().slowDebuff.Remove(slow);
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