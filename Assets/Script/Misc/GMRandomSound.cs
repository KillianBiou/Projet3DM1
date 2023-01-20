using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GMRandomSound : NetworkBehaviour
{
    public List<AudioClip> audioClipList;

    private AudioSource audioSource;

    public float minimumDelay;
    public float maximumDelay;

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(RandomSound());
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlayRandomSound()
    {
        int randomClip = Random.Range(0, audioClipList.Count - 1);
        PlayRandomSoundClient(randomClip);
    }

    [ObserversRpc]
    private void PlayRandomSoundClient(int index)
    {
        audioSource.clip = audioClipList[index];
        audioSource.Play();
    }

    private IEnumerator RandomSound()
    {
        yield return new WaitForSeconds(Random.Range(minimumDelay, maximumDelay));
        PlayRandomSound();
        StartCoroutine(RandomSound());
    }
}
