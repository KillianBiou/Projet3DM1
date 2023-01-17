using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private Color cooldownColor;
    [SerializeField]
    private Color firingColor;
    [SerializeField]
    private float emissivePower;

    #endregion
    #region Internal Parameters

    [SerializeField]
    private GameObject barrelPrefab;

    private TrapState currentState = TrapState.COOLDOWN;
    private float currentTimer;
    private Animator animator;
    private Transform barrelOrigin;
    private List<Renderer> emmisiveRenderers = new List<Renderer>();

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        barrelOrigin = transform.Find("BarrelSpawn");
        emmisiveRenderers = GetComponentsInChildren<Renderer>().ToList();
    }

    private void Update()
    {
        if (currentState == TrapState.COOLDOWN)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > cooldown)
            {
                currentTimer = 0;
                Attack();
                ChangeTrapState(TrapState.UP);
            }
        }

        if (currentState == TrapState.UP)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > upTime)
            {
                currentTimer = 0;
                ChangeTrapState(TrapState.COOLDOWN);
            }
        }
    }

    private void ChangeTrapState(TrapState state)
    {
        if (state == TrapState.UP)
        {
            animator.SetTrigger("OpenTrigger");
            animator.ResetTrigger("CloseTrigger");
            foreach (Renderer renderer in emmisiveRenderers)
            {
                renderer.material.SetColor("_EmissiveColor", firingColor * emissivePower);
            }
        }
        else
        {
            animator.SetTrigger("CloseTrigger");
            animator.ResetTrigger("OpenTrigger");
            foreach (Renderer renderer in emmisiveRenderers)
            {
                renderer.material.SetColor("_EmissiveColor", cooldownColor * emissivePower);
            }
        }
        currentState = state;
    }

    public void Attack()
    {
        Barrel barrel = Instantiate(barrelPrefab, barrelOrigin.position, barrelPrefab.transform.rotation * transform.rotation).GetComponent<Barrel>();
        barrel.Initialize(explosionRadius, damage, tickingOffSeconds);
    }
}
