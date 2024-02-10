using System.Collections;
using System.Threading;
using UnityEngine;
public class FrameRateManager : MonoBehaviour
{
    [Header("Frame Settings")]
    public int targetFrameRate = 60;
    float currentFrameTime;
    
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}