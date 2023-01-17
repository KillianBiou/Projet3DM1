using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField]
    private float colorTime;
    [SerializeField]
    private int flickingCount;
    [SerializeField]
    private float oscillationFactor;

    [Header("Color")]
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color flickingColor;
    [SerializeField]
    private float maxEmissivePower;
    [SerializeField]
    private float minimumEmissivePower;

    private List<Renderer> renderers = new();
    private float brightnessMean;

    // Start is called before the first frame update
    void Start()
    {
        renderers.AddRange(transform.Find("Casing").GetComponentsInChildren<Renderer>());
        ChangeColor(normalColor, minimumEmissivePower);

        StartCoroutine(FakeBuy());
    }

    private void ChangeColor(Color color, float emissivePower)
    {
        foreach(Renderer renderer in renderers)
        {
            renderer.material.SetColor("_EmissiveColor", color * emissivePower);
        }
    }

    private IEnumerator FakeBuy()
    {
        Debug.Log("FakeBuy");
        yield return new WaitForSeconds(3);
        Buy();
        yield return new WaitForSeconds(3);
        StartCoroutine(FakeBuy());
    }

    private IEnumerator Flicker(int step)
    {
        if(step <= flickingCount)
        {
            Color currentColor = step % 2 == 0 ? normalColor : flickingColor;
            for (float i = 0; i <= colorTime; i += Time.deltaTime)
            {
                ChangeColor(currentColor, brightnessMean + (Mathf.Cos(Time.time * oscillationFactor) * brightnessMean / 2));
                yield return new WaitForNextFrameUnit();
            }
            StartCoroutine(Flicker(step + 1));
        }
        else
        {
            ChangeColor(normalColor, minimumEmissivePower);
        }
    }

    public void Buy()
    {
        brightnessMean = (maxEmissivePower + minimumEmissivePower) / 2;
        Debug.Log("Buy");
        StartCoroutine(Flicker(1));
    }
}
