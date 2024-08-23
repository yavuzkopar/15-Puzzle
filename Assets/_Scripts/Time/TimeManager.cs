using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set;}
    [SerializeField] GameSettingsSO gameSettingsSO;
    public Action<float> OnTimeUpdated;
    private void Awake() {
        Instance = this;
    }
    float currentTime;
    private void Start() {
        currentTime = gameSettingsSO.gameSettings.currentTime;
        StartCoroutine(UpdateTimer());
        MapGenerator.Instance.OnMapChanged += ResetTimer;
        OnTimeUpdated?.Invoke(currentTime);
    }
    IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            currentTime += 0.1f;
            OnTimeUpdated?.Invoke(currentTime);
        }
        
    }
    public void ResetTimer()
    {
        currentTime = 0;
        gameSettingsSO.gameSettings.currentTime = 0;
        OnTimeUpdated?.Invoke(currentTime);
    }
    public float GetCurrentTime()=>currentTime;
    public void SaveCurrentTime()
    {
        gameSettingsSO.gameSettings.currentTime = currentTime;
    }
}
