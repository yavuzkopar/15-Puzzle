using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovementController : MonoBehaviour {
     TileMap map;
    [SerializeField] GameSettingsSO gameSettingsSO;
    bool paused;
    public Action<bool> OnPaused;
    public Action<int> OnMoveCountChanged;
    public Action OnLevelSucceded;
    bool canMakeMove;
    public static TileMovementController Instance{get;private set;}
    private void Awake() {
        Instance = this;
    }
    private void Start() {
        map = MapGenerator.Instance.map;
        Pause(true);
        canMakeMove = true;
        MapGenerator.Instance.OnMapChanged+=()=> Reset();
    }
    void Reset(){
        map = MapGenerator.Instance.map;
        gameSettingsSO.gameSettings.moveCount = 0;
        OnMoveCountChanged?.Invoke(0);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (map == null) return;
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);
            if (x >= 0 && x < gameSettingsSO.gameSettings.wight && y >= 0 && y < gameSettingsSO.gameSettings.hight)
            {
                var tilePos = map.grid.GetTilePosition(x, y);
                if (tilePos != null && canMakeMove)
                {
                    MoveTile(tilePos);
                }
            }
        }
    }
    public void MoveTile(TilePosition tilePos)
    {
        if(IsMiss(tilePos)) return;
        if (tilePos.tileObject.isMoving) return;
        if (paused) return;
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
                objectsToMove[i].tileObject.Move(tilePosition.Position(),gameSettingsSO.gameSettings.moveType);
            }
            gameSettingsSO.gameSettings.moveCount += objectsToMove.Count;
            OnMoveCountChanged?.Invoke(gameSettingsSO.gameSettings.moveCount);
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
                objectsToMove[i].tileObject.Move(tilePosition.Position(),gameSettingsSO.gameSettings.moveType);
            }
            gameSettingsSO.gameSettings.moveCount += objectsToMove.Count;
            OnMoveCountChanged?.Invoke(gameSettingsSO.gameSettings.moveCount);
        }
        map.SetEmptyTile(tilePos.x, tilePos.y);
        if (CheckAll())
        {
            LevelSucceed();
        }
    }
    bool IsMiss(TilePosition tilePos)
    {
        bool isMiss = (tilePos.x != map.GetEmptyTile().x && tilePos.y != map.GetEmptyTile().y) || tilePos == map.GetEmptyTile();
        return isMiss;
    }
    void LevelSucceed()
    {
        StartCoroutine(LevelEnd());
    }
    IEnumerator LevelEnd()
    {
        canMakeMove = false;
        yield return new WaitForSeconds(0.5f);
        
        OnLevelSucceded?.Invoke();
        Pause(true);
        canMakeMove = true;
    }
    public void Pause(bool value)
    {
        StartCoroutine(OneFrameLaterPause(value));

    }
    IEnumerator OneFrameLaterPause(bool value)
    {

        paused = value;
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;

        yield return null;
        OnPaused?.Invoke(value);
    }
    public bool CheckAll()
    {
        List<TilePosition> listToCheck = map.grid.GetAll();
        foreach (var item in listToCheck)
        {
            if (!IsInRightPosition(item))
            {
                return false;
            }
        }
        return true;
    }
    bool IsInRightPosition(TilePosition tilePos)
    {
        if (tilePos == map.GetEmptyTile()) return true;

        if (tilePos.tileObject.number == tilePos.GetOrder())
        {
            return true;
        }
        return false;
    }
}