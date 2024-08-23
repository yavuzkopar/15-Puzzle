using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set;}
    public Action OnExit;
    [SerializeField] GameSettingsSO gameSettingsSO;
    private void Awake() {
        Instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
            gameSettingsSO.SaveGameSettings();
            Application.Quit();
        }
    }
     private void OnDisable()
    {
        OnExit?.Invoke();
        gameSettingsSO.SaveGameSettings();
    }
}