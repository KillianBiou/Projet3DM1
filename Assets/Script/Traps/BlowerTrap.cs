using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlowerTrap : MonoBehaviour
{
    #region Parameters

    [SerializeField]
    private float blowerForce;

    #endregion
    #region Internal Parameters

    private Animator animator;

    [SerializeField]    
    private bool turbo;
    [SerializeField]
    private float turboMulti;
    [SerializeField] 
    private float activationDuration;
    [SerializeField]
    private KeyCode activationKey;

    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color cooldownColor;
    [SerializeField]
    private Color firingColor;
    [SerializeField]
    private float emissivePower;

    [SerializeField]
    private float cooldown;

    private float clock;

    private BoxCollider trapCollider;
    private Transform trapDirection;

    private TrapState currentState = TrapState.COOLDOWN;

    private List<Renderer> emmisiveRenderers = new List<Renderer>();

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        trapCollider = GetComponentInChildren<BoxCollider>();
        emmisiveRenderers = GetComponentsInChildren<Renderer>().ToList();

        foreach (Renderer renderer in emmisiveRenderers)
        {
            renderer.material.SetColor("_EmissiveColor", cooldownColor * emissivePower);
        }

        trapDirection = transform.Find("BlowerDirection");
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, transform.lossyScale / 1.4f, trapDirection.forward, out hit, transform.rotation, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("Player"))
            {
                HitEffect(hit.transform.GetComponent<Player>());
            }
        }

        if(currentState == TrapState.COOLDOWN)
        {
            clock += Time.deltaTime;
            if (clock >= cooldown)
            {
                ChangeTrapState(TrapState.CAN_BE_USED);
                clock = 0f;
            }
        }
        else if(currentState == TrapState.CAN_BE_USED && Input.GetKeyDown(activationKey))
        {
            ChangeTrapState(TrapState.UP);
        }
        else if(currentState == TrapState.UP)
        {
            clock += Time.deltaTime;
            if (clock >= activationDuration)
            {
                ChangeTrapState(TrapState.COOLDOWN);
                clock = 0f;
            }
        }
    }

    private void ChangeTrapState(TrapState state)
    {
        if (state == TrapState.UP)
        {
            animator.speed = turboMulti;
            turbo = true;
            foreach (Renderer renderer in emmisiveRenderers)
            {
                renderer.material.SetColor("_EmissiveColor", firingColor * emissivePower);
            }
        }
        else if(state == TrapState.COOLDOWN)
        {
            turbo = false;
            animator.speed = 1;
            foreach (Renderer renderer in emmisiveRenderers)
            {
                renderer.material.SetColor("_EmissiveColor", cooldownColor * emissivePower);
            }
        }
        else
        {
            turbo = false;
            animator.speed = 1;
            foreach (Renderer renderer in emmisiveRenderers)
            {
                renderer.material.SetColor("_EmissiveColor", normalColor * emissivePower);
            }
        }
        currentState = state;
    }

    private void HitEffect(Player player)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position += (trapDirection.forward * (blowerForce * (turbo ? turboMulti : 1)) * Time.deltaTime);
        player.GetComponent<CharacterController>().enabled = true;
    }

    void OnDrawGizmos()
    {
        RaycastHit hit;

        bool isHit = Physics.BoxCast(transform.position, transform.lossyScale / 2, trapDirection.forward, out hit, transform.rotation, Mathf.Infinity);
        Gizmos.DrawWireCube(transform.position + trapDirection.forward * 1, transform.lossyScale * 1.5f);
        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, trapDirection.forward * hit.distance);
            Gizmos.DrawWireCube(transform.position + trapDirection.forward * hit.distance, transform.lossyScale * 1.5f);
        }
        else
        {   
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, trapDirection.forward * 25);
        }
    }
}
