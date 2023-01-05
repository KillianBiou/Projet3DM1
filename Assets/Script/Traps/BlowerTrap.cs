using StarterAssets;
using System.Collections;
using System.Collections.Generic;
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

    private BoxCollider trapCollider;
    private Transform trapDirection;

    #endregion

    #region Static Parameters
    
    public static bool HashBlown = false;

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        trapCollider = GetComponentInChildren<BoxCollider>();
        trapDirection = transform.Find("BlowerDirection");
    }

    private void Update()
    {
        if (true)
        {
            RaycastHit hit;
            if (Physics.BoxCast(transform.position, transform.lossyScale / 1.4f, trapDirection.forward, out hit, transform.rotation, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    HitEffect(hit.transform.GetComponent<Player>());
                }
            }
        }

        if (turbo)
        {
            animator.speed = 3;

        }
        else
        {
            animator.speed = 1;
        }
    }

    private void HitEffect(Player player)
    {
        Debug.Log("In the way");
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position += (trapDirection.forward * (blowerForce * (turbo ? 3 : 1)) * Time.deltaTime);
        player.GetComponent<CharacterController>().enabled = true;
        HashBlown = true;
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
