using FishNet.Object;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Endroom : NetworkBehaviour
{

    private ParticleSystem system;
    private Animator animator;

    private void Start()
    {
        system = GetComponent<ParticleSystem>();
        animator = transform.Find("Entry").GetComponentInChildren<Animator>();
        animator.SetTrigger("Lift");
        system.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            system.Play();
            animator.SetTrigger("Close");
            GameContext.instance.StartEndServer();
            Destroy(GetComponent<BoxCollider>());
        }
    }

}
