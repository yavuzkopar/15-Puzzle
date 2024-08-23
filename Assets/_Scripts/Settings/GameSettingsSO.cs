using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsSO", menuName = "GameSettingsSO", order = 0)]
public class GameSettingsSO : ScriptableObject
{
    public string playerPrefKey;
    public AnimationCurve tileMoveCurve;
    public Sprite[] tileshapes;
    public GameSettings gameSettings;
    [SerializeField] string saveText;
    public void LoadGameSettings()
    {
        if (PlayerPrefs.HasKey(playerPrefKey))
        {
            string playerPref = PlayerPrefs.GetString(playerPrefKey);
            gameSettings = JsonUtility.FromJson<GameSettings>(playerPref);
        }
    }
    public void SaveGameSettings()
    {

        string json = JsonUtility.ToJson(gameSettings, true);
        Debug.Log(json);
        PlayerPrefs.SetString(playerPrefKey, json);

    }
}
