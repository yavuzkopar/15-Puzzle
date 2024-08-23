using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// In first setup scene
public class SettingsSetup : MonoBehaviour
{
    [SerializeField] GameSettingsSO gameSettingsSO;
    private void Awake() {
        gameSettingsSO.LoadGameSettings();
        SceneManager.LoadSceneAsync(1);
    }
}
