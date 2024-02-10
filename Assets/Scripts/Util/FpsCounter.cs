using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    private float fps;
    private float updateTimer = 0.2f;

    [SerializeField] private TextMeshProUGUI fpsText;

    private void Update()
    {
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0f)
        {
            fps = 1f / Time.unscaledDeltaTime;
            fpsText.text = "FPS: " + Mathf.Round(fps);
            updateTimer = 0.2f;
        }
    }
}
