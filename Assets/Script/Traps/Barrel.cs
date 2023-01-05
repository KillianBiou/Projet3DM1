using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    #region Internal Parameters

    private float explosionRadius;
    private int damage;

    private float tickingOffSeconds;
    private float currentTimer;

    private Player player;

    #endregion

    private void Start()
    {
        GetComponent<SphereCollider>().radius = explosionRadius;
        player = null;
    }

    public void Initialize(float explosionRadius, int damage, float tickingOffSeconds)
    {
        this.explosionRadius = explosionRadius;
        this.damage = damage;
        this.tickingOffSeconds = tickingOffSeconds;
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
        if(currentTimer >= tickingOffSeconds) {
            ParticleSystem explosion = GetComponent<ParticleSystem>();
            ParticleSystem.ShapeModule ps = explosion.shape;
            explosion.Play();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            HitEffect();
            Destroy(this);
        }
    }

    public void HitEffect()
    {
        if (player)
        {
            player.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            Debug.Log("Player entered radius");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            Debug.Log("Player exited radius");
        }
    }
}
