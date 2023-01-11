using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private Color cooldownColor;
    [SerializeField]
    private Color firingColor;
    [SerializeField]
    private float emissivePower;

    #endregion
    #region Internal Parameters

    private List<Renderer> emmisiveRenderers = new List<Renderer>();
    private TrapState currentState = TrapState.COOLDOWN;
    private float currentTimer;
    private Animator animator;

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();

        foreach (Renderer renderer in emmisiveRenderers)
        {
            renderer.material.SetColor("_EmissiveColor", cooldownColor * emissivePower);
        }
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
                ChangeTrapState(TrapState.UP);
            }
        }

        if (currentState == TrapState.UP)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > upTime)
            {
                currentTimer = 0;
                currentState = TrapState.COOLDOWN;
                ChangeTrapState(TrapState.COOLDOWN);
            }
        }
    }

    private void ChangeTrapState(TrapState state)
    {
        emmisiveRenderers = GetComponentsInChildren<Renderer>().ToList();
        if (state == TrapState.COOLDOWN)
        {
            animator.SetTrigger("Retract");
            animator.ResetTrigger("Extend");
            foreach (Renderer renderer in emmisiveRenderers)
            {
                renderer.material.SetColor("_EmissiveColor", cooldownColor * emissivePower);
            }
        }
        else
        {
            animator.SetTrigger("Extend");
            animator.ResetTrigger("Retract");
            foreach (Renderer renderer in emmisiveRenderers)
            {
                renderer.material.SetColor("_EmissiveColor", firingColor * emissivePower);
            }
        }
        currentState = state;
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
