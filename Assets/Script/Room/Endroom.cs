using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Endroom : NetworkBehaviour
{
    [SerializeField]
    private Image fadeOutImage;

    private Image fadeOutInstance;

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

            StartEndServer();
            Destroy(GetComponent<BoxCollider>());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartEndServer()
    {
        StartEnd();
    }

    [ObserversRpc]
    private void StartEnd()
    {
        system.Play();
        animator.SetTrigger("Close");
        fadeOutInstance = Instantiate(fadeOutImage).GetComponent<Image>();
    }

    private IEnumerator FadeOut()
    {
        for(int i = 0; i < 255; i++)
        {
            fadeOutInstance.color = new Color(255, 255, 255, i);
            yield return new WaitForNextFrameUnit();
        }
    }

}
