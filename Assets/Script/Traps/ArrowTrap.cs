using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    #region Parameters

    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float upTime;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float arrowSpeed;

    [SerializeField]
    private Color cooldownColor;
    [SerializeField]
    private Color firingColor;
    [SerializeField]
    private float emissivePower;

    #endregion
    #region Internal Parameters

    private TrapState currentState = TrapState.COOLDOWN;
    private float currentTimer = 0f;
    private List<Transform> arrowSpawners = new List<Transform>();
    private List<GameObject> currentArrows = new List<GameObject>();
    private List<Renderer> emmisiveRenderers = new List<Renderer>();

    #endregion

    private void Start()
    {
        foreach(Transform t in transform.Find("ArrowSpawners"))
        {
            arrowSpawners.Add(t);
        }

        emmisiveRenderers = GetComponentsInChildren<Renderer>().ToList();
        
        foreach(Transform transform in arrowSpawners)
        {
            currentArrows.Add(transform.GetChild(0).gameObject);
            transform.GetChild(0).GetComponent<Arrow>().damage = damage;
        }

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
                ThrowArrows();
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
        emmisiveRenderers = GetComponentsInChildren<Renderer>().ToList();
        if (state == TrapState.COOLDOWN)
        {
            foreach(Renderer renderer in emmisiveRenderers)
            {
                renderer.material.SetColor("_EmissiveColor", cooldownColor * emissivePower);
            }
        }
        else
        {
            foreach (Renderer renderer in emmisiveRenderers)
            {
                renderer.material.SetColor("_EmissiveColor", firingColor * emissivePower);
            }
        }
        currentState = state;
    }
    
    private IEnumerator RespawnArrows()
    {
        yield return new WaitForSeconds(upTime);

        foreach(GameObject arrow in currentArrows)
        {
            if(arrow)
                StartCoroutine(arrow.GetComponent<Arrow>().ScheduleDestruction(2));
        }

        currentArrows.Clear();

        foreach (Transform spawner in arrowSpawners)
        {
            GameObject arrow = Instantiate(arrowPrefab, spawner.transform.position, arrowPrefab.transform.rotation * transform.rotation, spawner);
            arrow.GetComponent<Arrow>().damage = damage;
            currentArrows.Add(arrow);
        }
    }

    private void ThrowArrows()
    {
        foreach(GameObject arrow in currentArrows)
        {
            arrow.GetComponent<Rigidbody>().isKinematic = false;
            arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * arrowSpeed);
        }
        StartCoroutine(RespawnArrows());
    }
}
