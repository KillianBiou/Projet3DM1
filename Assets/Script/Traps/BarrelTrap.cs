using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrap : MonoBehaviour
{
    #region Parameters

    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float upTime;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float explosionRadius;
    [SerializeField]
    private float tickingOffSeconds;

    #endregion
    #region Internal Parameters

    [SerializeField]
    private GameObject barrelPrefab;

    private TrapState currentState = TrapState.COOLDOWN;
    private float currentTimer;
    private Animator animator;
    private Transform barrelOrigin;

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        barrelOrigin = transform.Find("BarrelSpawn");
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
                animator.SetTrigger("OpenTrigger");
                animator.ResetTrigger("CloseTrigger");
                Attack();
            }
        }

        if (currentState == TrapState.UP)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > upTime)
            {
                currentTimer = 0;
                currentState = TrapState.COOLDOWN;
                animator.SetTrigger("CloseTrigger");
                animator.ResetTrigger("OpenTrigger");
            }
        }
    }

    public void Attack()
    {
        //player.TakeDamage(damage);
        Debug.Log("Spawn Barrel");
        Barrel barrel = Instantiate(barrelPrefab, barrelOrigin.position, barrelPrefab.transform.rotation).GetComponent<Barrel>();
        barrel.Initialize(explosionRadius, damage, tickingOffSeconds);
    }
}
