using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public enum SpikeState
{
    COOLDOWN,
    EXTENDED
}

public class SpikeTrap : MonoBehaviour
{
    [SerializeField]
    private OffensiveTrapScriptableObject template;

    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float upTime;
    [SerializeField]
    private int damage;

    private SpikeState currentState = SpikeState.COOLDOWN;
    private float currentTimer;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(currentState == SpikeState.COOLDOWN)
        {
            currentTimer += Time.deltaTime;
            if(currentTimer > cooldown)
            {
                currentTimer = 0;
                currentState = SpikeState.EXTENDED;
                animator.SetTrigger("Extend");
                animator.ResetTrigger("Retract");
            }
        }

        if (currentState == SpikeState.EXTENDED)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > upTime)
            {
                currentTimer = 0;
                currentState = SpikeState.COOLDOWN;
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
        if (currentState == SpikeState.EXTENDED && other.CompareTag("Player"))
        {
            HitEffect(other.GetComponentInParent<Player>());
        }
    }
}
