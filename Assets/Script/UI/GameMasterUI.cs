using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMasterUI : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    private void Awake()
    {
        timerText = transform.Find("Timer").Find("Overlay").GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        RefreshTimer();
    }

    private void RefreshTimer()
    {
        timerText.text = TimeSpan.FromSeconds(GameContext.instance.roomTimer).ToString(@"mm\:ss");
    }
}
