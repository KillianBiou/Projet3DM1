using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class TurretTrap : MonoBehaviour, TrapInteraction
{
    #region Parameters

    [Header("Shot")]
    [SerializeField]
    private float shotDuration;
    [SerializeField]
    private int nbShot;
    [SerializeField]
    private int shotDamage;

    [Header("Timer")]
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private float cooldown;

    [Header("Miscellaneous")]
    [SerializeField]
    private float rotationFactor;

    [SerializeField]
    private float maxDistance;

    [SerializeField]
    private KeyCode activationKey;

    [Header("Color")]
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color cooldownColor;
    [SerializeField]
    private Color firingColor;
    [SerializeField]
    private float emissivePower;

    [SerializeField]
    private GameObject bulletPrefab;

    #endregion
    #region Internal Parameters

    private Animator animator;

    private float clock;

    private TrapState currentState = TrapState.CAN_BE_USED;

    private List<Renderer> emmisiveRenderers = new List<Renderer>();

    private Transform cannonTip;

    private LaserScope scope;

    private AudioSource fireSound;

    #endregion

    private void Start()
    {
        Register();
        fireSound = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
        emmisiveRenderers = GetComponentsInChildren<Renderer>().ToList();

        scope = GetComponentInChildren<LaserScope>();
        scope.SetLaserMaxLenght(maxDistance);

        cannonTip = scope.transform.parent;
        foreach (Renderer renderer in emmisiveRenderers)
        {
            if(renderer.name != "Line")
            {
                renderer.material.SetColor("_EmissiveColor", normalColor * emissivePower);
            }
        }

        transform.Find("UI").Find("Image").GetComponentInChildren<TextMeshProUGUI>().text = Regex.Replace(activationKey.ToString(), @"[a-zA-Z]", "");
    }

    private void Update()
    {
        if (currentState == TrapState.COOLDOWN)
        {
            clock += Time.deltaTime;
            if (clock >= cooldown)
            {
                ChangeTrapState(TrapState.CAN_BE_USED);
                clock = 0f;
            }
        }
        transform.GetChild(0).Rotate(Vector3.forward * rotationFactor);
    }

    private void ChangeTrapState(TrapState state)
    {
        if (state == TrapState.UP)
        {
            GetComponentInChildren<LaserScope>().SetCanPreview(false);
            animator.speed = 1f/shotDuration;
            foreach (Renderer renderer in emmisiveRenderers)
            {
                if (renderer.name != "Line")
                {
                    renderer.material.SetColor("_EmissiveColor", firingColor * emissivePower);
                }
            }
        }
        else if (state == TrapState.COOLDOWN)
        {
            scope.SetCanPreview(true);
            foreach (Renderer renderer in emmisiveRenderers)
            {
                if (renderer.name != "Line")
                {
                    renderer.material.SetColor("_EmissiveColor", cooldownColor * emissivePower);
                }
            }
        }
        else if(state == TrapState.RELOADING)
        {
            animator.speed = (1f/reloadTime);
            foreach (Renderer renderer in emmisiveRenderers)
            {
                if (renderer.name != "Line")
                {
                    renderer.material.SetColor("_EmissiveColor", cooldownColor * emissivePower);
                }
            }
        }
        else
        {
            foreach (Renderer renderer in emmisiveRenderers)
            {
                if (renderer.name != "Line")
                {
                    renderer.material.SetColor("_EmissiveColor", normalColor * emissivePower);
                }
            }
        }
        currentState = state;
    }

    private IEnumerator Reload()
    {
        ChangeTrapState(TrapState.RELOADING);

        animator.SetBool("Reload", true);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("Reload", false);

        ChangeTrapState(TrapState.COOLDOWN);
    }

    private IEnumerator Shoot(int currentShot)
    {
        if(currentShot < nbShot)
        {
            animator.SetTrigger("Shoot");
            ShootBullet();
            yield return new WaitForSeconds(shotDuration);
            animator.ResetTrigger("Shoot");
            StartCoroutine(Shoot(currentShot + 1));
        }
        else
        {
            StartCoroutine(Reload());
        }
    }

    private void ShootBullet()
    {
        RaycastHit hit;
        fireSound.Play();
        Instantiate(bulletPrefab, cannonTip.transform.position, cannonTip.rotation);
        if(Physics.Raycast(cannonTip.position, cannonTip.forward, out hit, maxDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                HitEffect(hit.transform.GetComponent<Player>());
            }
        }
    }

    private void HitEffect(Player player)
    {
        player.TakeDamage(shotDamage);
    }

    public void Activation()
    {
        if(currentState == TrapState.CAN_BE_USED)
        {
            ChangeTrapState(TrapState.UP);
            StartCoroutine(Shoot(0));
        }
    }

    public void Register()
    {
        transform.parent.GetComponentInParent<RoomData>().RegisterTrap(activationKey, this);
    }
}
