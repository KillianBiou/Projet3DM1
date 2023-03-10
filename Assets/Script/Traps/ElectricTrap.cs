using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

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
    private Modifier slow;
    [SerializeField]
    private Vector2 activatedBrightness;
    [SerializeField]
    private float oscillationFactor;

    #endregion
    #region Internal Parameters

    private Player player;
    private TrapState currentState = TrapState.COOLDOWN;
    private float currentTimer;
    private bool hasSlowTarget = false;
    private GameObject VFXContainer;

    private float brightnessMean;
    private Material emmisiveMaterial;
    private Color emmisiveColor;
    private AudioSource fireSound;


    #endregion

    private void Start()
    {
        fireSound = GetComponent<AudioSource>();
        player = GameObject.FindObjectOfType<Player>();
        VFXContainer = transform.Find("VFX").gameObject;
        emmisiveMaterial = GetComponent<Renderer>().material;
        emmisiveColor = emmisiveMaterial.GetColor("_EmissiveColor");
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
                fireSound.Play();
            }
        }

        if (currentState == TrapState.UP)
        {
            brightnessMean = (activatedBrightness.x + activatedBrightness.y) / 2;
            emmisiveMaterial.SetColor("_EmissiveColor", emmisiveColor * (brightnessMean + (Mathf.Cos(Time.time * oscillationFactor) * brightnessMean / 2)));

            currentTimer += Time.deltaTime;
            if (currentTimer > upTime)
            {
                emmisiveMaterial.SetColor("_EmissiveColor", emmisiveColor);
                currentTimer = 0;
                currentState = TrapState.COOLDOWN;
                GetComponent<MeshRenderer>().material.color = Color.white ;
                VFXContainer.SetActive(false);
                RefreshSlow(false);
                fireSound.Stop();
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