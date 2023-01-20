using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private Color cooldownColor;
    [SerializeField]
    private Color firingColor;
    [SerializeField]
    private float emissivePower;

    #endregion
    #region Internal Parameters

    private TrapState currentState = TrapState.COOLDOWN;
    private float currentTimer;
    private List<ParticleSystem> gazParticlesSystems = new List<ParticleSystem>();
    private List<Renderer> emmisiveRenderers = new List<Renderer>();
    private AudioSource fireSound;

    #endregion

    private void Start()
    {
        fireSound = GetComponent<AudioSource>();
        gazParticlesSystems.AddRange(gameObject.GetComponentsInChildren<ParticleSystem>());
        emmisiveRenderers = GetComponentsInChildren<Renderer>().ToList();

        foreach (Renderer renderer in emmisiveRenderers)
        {
            renderer.material.SetColor("_EmissiveColor", cooldownColor * emissivePower);
        }
    }

    private void Update()
    {
        if (currentState == TrapState.COOLDOWN)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > cooldown)
            {
                currentTimer = 0;
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

    public void HitEffect(Player player)
    {
        player.TakeDamage(damage);
    }

    private void ChangeTrapState(TrapState state)
    {
        if (state == TrapState.UP)
        {
            fireSound.Play();
            ToggleParticles(state);
            foreach (Renderer renderer in emmisiveRenderers)
            {
                renderer.material.SetColor("_EmissiveColor", firingColor * emissivePower);
            }
        }
        else
        {
            fireSound.Stop();
            ToggleParticles(state);
            foreach (Renderer renderer in emmisiveRenderers)
            {
                renderer.material.SetColor("_EmissiveColor", cooldownColor * emissivePower);
            }
        }
        currentState = state;
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentState == TrapState.UP && other.CompareTag("Player"))
        {
            HitEffect(other.GetComponentInParent<Player>());
        }
    }

    private void ToggleParticles(TrapState state)
    {
        if(state == TrapState.UP)
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
