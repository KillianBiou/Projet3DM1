using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Color lifeColor;
    [SerializeField]
    private Color emptyColor;
    private List<Image> lifeImages = new();
    private int hpMemory = -1;

    private TextMeshProUGUI pointsUI;
    private int pointsMemory = -1;

    [SerializeField]
    private Color haveColor;
    [SerializeField]
    private Color dontHaveColor;
    private Image blindIcon;
    private Image cutTrapIcon;

    [SerializeField]
    private Color noShieldColor;
    [SerializeField]
    private Color shieldColor;
    private Image externImage;


    private Player player;

    private void Awake()
    {
        foreach(Transform child in transform.Find("Life").Find("Intern"))
        {
            lifeImages.Add(child.GetComponent<Image>());
        }

        pointsUI = transform.Find("Points").Find("Amount").GetComponent<TextMeshProUGUI>();

        blindIcon = transform.Find("Skills").Find("Square").Find("Cut").Find("CutCam").GetComponent<Image>();
        cutTrapIcon = transform.Find("Skills").Find("Square").Find("Trap").Find("CutTrap").GetComponent<Image>();

        externImage = transform.Find("Life").Find("Extern").GetComponent<Image>();

        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        int newHP = player.GetHP();
        if (hpMemory != newHP)
        {
            RefreshLife(newHP);
        }

        int newPoints = player.GetPoints();
        if (pointsMemory != newPoints)
        {
            RefreshPoints(newPoints);
        }

        RefreshSkills();
        RefreshShield();
    }

    private void RefreshShield()
    {
        if (player.GetHaveShield())
            externImage.color = shieldColor;
        else
            externImage.color = noShieldColor;
    }

    private void RefreshLife(int newLife)
    {
        for(int i = 0; i < lifeImages.Count; i++)
        {
            if(i < newLife)
            {
                lifeImages[i].color = lifeColor;
            }
            else
            {
                lifeImages[i].color = emptyColor;
            }
        }
        hpMemory = newLife;
    }

    private void RefreshPoints(int newPoints) 
    { 
        pointsUI.text = newPoints.ToString();
        pointsMemory = newPoints;
    }

    private void RefreshSkills()
    {
        if (player.GetHasCutTrap())
            cutTrapIcon.color = haveColor;
        else
            cutTrapIcon.color = dontHaveColor;

        if (player.GetHasBlind())
            blindIcon.color = haveColor;
        else
            blindIcon.color = dontHaveColor;
    }
}
