using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] TileObject tilePrefab;
    public TileMap map;
    [SerializeField] Transform _backGround;
    public static MapGenerator Instance { get; private set; }
    [SerializeField] GameSettingsSO gameSettingsSO;
    public Action OnMapChanged;
    void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey(gameSettingsSO.playerPrefKey))
            CopyMap(gameSettingsSO.gameSettings.mapTile);
        else
        {
            CreateNewMapTile();
        }
    }
    void CopyMap(TileMap mapTile)
    {
        map = mapTile;
        SettingsManager.Instance.SetMap(map);
        foreach (var item in map.grid.GetAll())
        {
            if (item.tileObject == null || item.tileObject.number == 0)
            {
                map.SetEmptyTile(item.x, item.y);
                continue;
            }
            TileObject temp = GameObject.Instantiate(tilePrefab, transform);
            temp.SetTileData(item.tileObject);
            temp.transform.position = item.Position();
            temp.tileObjectData.NumberChanged(item.tileObject.number);
            temp.tileObjectData.SetTileShape(gameSettingsSO.tileshapes[gameSettingsSO.gameSettings.tileShapeIndex]);
        }
        Camera.main.transform.position = new Vector3(map.width / 2f - .5f, map.height / 2f - 0.5f, -10);
        int bigger = Mathf.Max(map.width, map.height);
        Camera.main.orthographicSize = bigger;
        _backGround.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        _backGround.localScale = new Vector3(map.width + 0.1f, map.height + 0.1f, 1);
    }
  
    private void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           // TimeManager.Instance.SaveCurrentTime();
            gameSettingsSO.SaveGameSettings();
            Application.Quit();
        }
    }
    public void CreateNewMapTile()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        int width = gameSettingsSO.gameSettings.wight;
        int height = gameSettingsSO.gameSettings.hight;
        map = new TileMap(width, height, transform, tilePrefab,gameSettingsSO.tileshapes[gameSettingsSO.gameSettings.tileShapeIndex]);
        SettingsManager.Instance.SetMap(map);
        OnMapChanged?.Invoke();
        gameSettingsSO.gameSettings.mapTile = map;
       // gameSettingsSO.gameSettings.moveCount = 0;
       // gameSettingsSO.gameSettings.currentTime = 0;
        Camera.main.transform.position = new Vector3(width / 2f - .5f, height / 2f - 0.5f, -10);
        int bigger = Mathf.Max(width, height);
        Camera.main.orthographicSize = bigger;
        _backGround.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        _backGround.localScale = new Vector3(width + 0.1f, height + 0.1f, 1);
        int repeatTime = map.width * map.height;
        if (gameSettingsSO.gameSettings.difficulty == Difficulty.Hard)
        {
            repeatTime *= 5;
        }
        for (int i = 0; i < repeatTime; i++)
        {
            RandomMove();
        }

    }
    bool IsMiss(TilePosition tilePos)
    {
        bool isMiss = (tilePos.x != map.GetEmptyTile().x && tilePos.y != map.GetEmptyTile().y) || tilePos == map.GetEmptyTile();
        return isMiss;
    }
    void RandomMove()
    {
        List<TilePosition> listToCheck = new List<TilePosition>(map.grid.GetAll());
        TilePosition tilePos = listToCheck[UnityEngine.Random.Range(0, listToCheck.Count)];
        while (IsMiss(tilePos))
        {
            listToCheck.Remove(tilePos);
            tilePos = listToCheck[UnityEngine.Random.Range(0, listToCheck.Count)];
        }

        if (tilePos.x == map.GetEmptyTile().x)
        {
            int amount = map.GetEmptyTile().y - tilePos.y;
            int carpan = 1;
            if (amount < 0)
            { carpan = -1; }
            amount = Mathf.Abs(amount);
            List<TilePosition> objectsToMove = new();
            for (int i = 0; i < amount; i++)
            {
                TilePosition tp = map.grid.GetTilePosition(tilePos.x, tilePos.y + (i * carpan));
                objectsToMove.Add(tp);
            }
            for (int i = objectsToMove.Count - 1; i >= 0; i--)
            {
                TilePosition tilePosition = map.grid.GetTilePosition(objectsToMove[i].x, objectsToMove[i].y + (1 * carpan));
                tilePosition.tileObject = objectsToMove[i].tileObject;
                objectsToMove[i].tileObject.Teleport(tilePosition.Position());
            }
        }
        else if (tilePos.y == map.GetEmptyTile().y)
        {
            int amount = map.GetEmptyTile().x - tilePos.x;
            int carpan = 1;
            if (amount < 0)
            { carpan = -1; }
            amount = Mathf.Abs(amount);
            List<TilePosition> objectsToMove = new();
            for (int i = 0; i < amount; i++)
            {
                TilePosition tp = map.grid.GetTilePosition(tilePos.x + (i * carpan), tilePos.y);
                objectsToMove.Add(tp);
            }
            for (int i = objectsToMove.Count - 1; i >= 0; i--)
            {
                TilePosition tilePosition = map.grid.GetTilePosition(objectsToMove[i].x + (1 * carpan), objectsToMove[i].y);
                tilePosition.tileObject = objectsToMove[i].tileObject;
                objectsToMove[i].tileObject.Teleport(tilePosition.Position());
            }
        }
        map.SetEmptyTile(tilePos.x, tilePos.y);
    }
    private void OnDestroy()
    {
       // TimeManager.Instance.SaveCurrentTime();
        gameSettingsSO.SaveGameSettings();
    }
}