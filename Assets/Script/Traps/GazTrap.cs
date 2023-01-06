using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazTrap : MonoBehaviour
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
    private List<ParticleSystem> gazParticlesSystems = new List<ParticleSystem>();

    #endregion

    private void Start()
    {
        gazParticlesSystems.AddRange(gameObject.GetComponentsInChildren<ParticleSystem>());
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
                ToggleParticles();
            }
        }

        if (currentState == TrapState.UP)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > upTime)
            {
                currentTimer = 0;
                currentState = TrapState.COOLDOWN;
                ToggleParticles();
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

    private void ToggleParticles()
    {
        if(currentState == TrapState.UP)
        {
            foreach(ParticleSystem particle in gazParticlesSystems)
            {
                particle.Play();
            }
        }
        else
        {
            foreach(ParticleSystem particle in gazParticlesSystems)
            {
                particle.Stop();
            }
        }
    }
}
