using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameSettingsSO gameSettingsSO;
    [SerializeField] TextMeshProUGUI widthText, heightText;
    [SerializeField] GameObject settingsObject;
    [SerializeField] Image colorImange;
    [SerializeField] Image topMenu;
    [SerializeField] Image pauseScreen;
    [SerializeField] Transform bg;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI moveTypeText;
    [SerializeField] TextMeshProUGUI moveCountText;
    [SerializeField] GameObject recordsParent;
    [SerializeField] TextMeshProUGUI recordTextes;
    [SerializeField] GameObject OnLevelSuccededPanel;
    [SerializeField] Image tileShape;
    [SerializeField] TextMeshProUGUI diffucultyText;
    public void UpgradeWidth()
    {
        gameSettingsSO.gameSettings.wight++;
        if (gameSettingsSO.gameSettings.wight > 10)
        {
            gameSettingsSO.gameSettings.wight = 3;
        }
        widthText.text = gameSettingsSO.gameSettings.wight.ToString();
        MapGenerator.Instance.CreateNewMapTile();
    }
    private void Start()
    {
        timeText.text = "Time   : " + GetTime(gameSettingsSO.gameSettings.currentTime);
        TileMovementController.Instance.OnPaused += OnPaused;
        TileMovementController.Instance.OnMoveCountChanged += OnMoveCountChanged;
        TileMovementController.Instance.OnLevelSucceded += OnLevelSucceded;
        TimeManager.Instance.OnTimeUpdated += OnTimeUpdated;
        colorImange.color = gameSettingsSO.gameSettings.color;
        topMenu.color = gameSettingsSO.gameSettings.color;
        moveCountText.text = gameSettingsSO.gameSettings.moveCount.ToString();
        OnTimeUpdated(gameSettingsSO.gameSettings.currentTime);
        OnMoveCountChanged(gameSettingsSO.gameSettings.moveCount);
    }

    private void OnLevelSucceded()
    {
        OnLevelSuccededPanel.gameObject.SetActive(true);
        gameSettingsSO.gameSettings.records.Add(new(gameSettingsSO.gameSettings.wight, gameSettingsSO.gameSettings.hight,
         gameSettingsSO.gameSettings.moveCount, TimeManager.Instance.GetCurrentTime()));
    }

    private void OnMoveCountChanged(int obj)
    {
        moveCountText.text = "Move Count: " + obj.ToString();
    }

    private void OnTimeUpdated(float obj)
    {
        timeText.text = "Time   : " + GetTime(obj);
    }

    private void OnDestroy()
    {
        TileMovementController.Instance.OnPaused -= OnPaused;
    }

    private void OnPaused(bool obj)
    {
        if (obj)
        {
            OpenPauseScreen();
        }
    }
    string GetTime(float currentTime)
    {
        var minutes = ((int)currentTime / 60).ToString("0");
        string seconds = (currentTime % 60).ToString("0.#");
        return $" {minutes} : {seconds}";
    }
    public void UpgradeHeight()
    {
        gameSettingsSO.gameSettings.hight++;
        if (gameSettingsSO.gameSettings.hight > 10)
        {
            gameSettingsSO.gameSettings.hight = 3;
        }
        heightText.text = gameSettingsSO.gameSettings.hight.ToString();
        MapGenerator.Instance.CreateNewMapTile();

    }
    public void OpenSettingsPanel()
    {
        settingsObject.gameObject.SetActive(true);
        widthText.text = gameSettingsSO.gameSettings.wight.ToString();
        heightText.text = gameSettingsSO.gameSettings.hight.ToString();
        colorImange.color = gameSettingsSO.gameSettings.color;
        moveTypeText.text = gameSettingsSO.gameSettings.moveType.ToString();
        tileShape.sprite = gameSettingsSO.tileshapes[gameSettingsSO.gameSettings.tileShapeIndex];
        diffucultyText.text = gameSettingsSO.gameSettings.difficulty.ToString();
        TileMovementController.Instance.Pause(true);
    }
    public void SetDifficulty()
    {
        int index = (int)gameSettingsSO.gameSettings.difficulty;
        index++;
        if (index >= Enum.GetValues(typeof(Difficulty)).Length)
        {
            index = 0;
        }
        gameSettingsSO.gameSettings.difficulty = (Difficulty)index;
        diffucultyText.text = gameSettingsSO.gameSettings.difficulty.ToString();

    }
    public void ChangeShape()
    {
        SettingsManager.Instance.ChangeShape();
        tileShape.sprite = gameSettingsSO.tileshapes[gameSettingsSO.gameSettings.tileShapeIndex];
    }
    [SerializeField] Color[] availableColors;
    int colorIndex;
    public void ChangeColor()
    {
        colorIndex++;
        if (colorIndex >= availableColors.Length)
        {
            colorIndex = 0;
        }
        gameSettingsSO.gameSettings.color = availableColors[colorIndex];
        colorImange.color = gameSettingsSO.gameSettings.color;
        topMenu.color = gameSettingsSO.gameSettings.color;
        SettingsManager.Instance.ChangeColor();
    }
    public void OpenPauseScreen()
    {

        pauseScreen.gameObject.SetActive(true);
        Vector2 tp1 = bg.position - (new Vector3(bg.localScale.x, bg.localScale.y, 0) * .5f);
        Vector2 tp2 = bg.position + (new Vector3(bg.localScale.x, bg.localScale.y, 0) * .5f); ;
        Vector2 tp11 = Camera.main.WorldToScreenPoint(tp1);
        Vector2 tp12 = Camera.main.WorldToScreenPoint(tp2);
        Debug.Log(tp12 - tp11);
        pauseScreen.rectTransform.position = (tp12 + tp11) / 2;
        pauseScreen.rectTransform.sizeDelta = (tp12 - tp11) / transform.localScale;

    }
    public void SetMoveType()
    {
        int index = (int)gameSettingsSO.gameSettings.moveType;
        index++;
        if (index >= Enum.GetValues(typeof(MoveType)).Length)
        {
            index = 0;
        }
        gameSettingsSO.gameSettings.moveType = (MoveType)index;
        moveTypeText.text = gameSettingsSO.gameSettings.moveType.ToString();
    }
    public void OpenRecordsPanel()
    {
        recordsParent.gameObject.SetActive(true);
        string text = "";
        if (gameSettingsSO.gameSettings.records.Count == 0)
        {
            recordTextes.text = "No record";
            return;
        }
        foreach (var item in gameSettingsSO.gameSettings.records)
        {
            text += $"W:{item.width} * H:{item.height}   Move Count: {item.moveCount}   Time: {GetTime(item.time)} \n";
        }
        recordTextes.text = text;
    }
    public void GoToGithub()
    {
        Application.OpenURL("https://github.com/yavuzkopar/15-Puzzle");
    }
}
