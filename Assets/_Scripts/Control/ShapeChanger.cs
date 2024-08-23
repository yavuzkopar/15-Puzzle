using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapeChanger : MonoBehaviour
{
    [SerializeField] GameSettingsSO gameSettingsSO;
    TileMap map;
    public static ShapeChanger Instance { get; private set; }
    public Action OnColorChanged;
    public Action OnShapeChanged;
    [SerializeField] Color[] availableColors;
    int colorIndex;
    int shapeIndex;
    private void Awake()
    {
        Instance = this;
    }
    private void Start() {
        
        MapGenerator.Instance.OnMapChanged+=()=> Reset();
        map = MapGenerator.Instance.map;
        colorIndex = gameSettingsSO.gameSettings.colorIndex;
        shapeIndex = gameSettingsSO.gameSettings.tileShapeIndex;
        SetColor();
        SetShape();
    }
    // Setted in Map Gnerator
    public void Reset()
    {
        map = MapGenerator.Instance.map;
        SetColor();
        SetShape();
    }
    
    public void ChangeColor()
    {
      /*  OnColorChanged?.Invoke();
        List<TilePosition> listToCheck = map.grid.GetAll();
        foreach (var item in listToCheck)
        {
            if (item == map.GetEmptyTile())
                continue;
            item.tileObjectData.SetColor(gameSettingsSO.gameSettings.color);
        }*/
        colorIndex++;
        if(colorIndex >= availableColors.Length)
        {
            colorIndex = 0;
        }
        gameSettingsSO.gameSettings.colorIndex = colorIndex;
        SetColor();
        
    } 
    void SetColor()
    {
        colorIndex = gameSettingsSO.gameSettings.colorIndex;
        gameSettingsSO.gameSettings.color= availableColors[colorIndex];
        List<TilePosition> listToCheck = map.grid.GetAll();
        foreach (var item in listToCheck)
        {
            if (item == map.GetEmptyTile())
                continue;
            item.tileObjectData.SetColor(availableColors[colorIndex]);
        }
        OnColorChanged?.Invoke();
    }
    public void ChangeShape()
    {
        gameSettingsSO.gameSettings.tileShapeIndex++;
        if (gameSettingsSO.gameSettings.tileShapeIndex >= gameSettingsSO.tileshapes.Length)
        {
            gameSettingsSO.gameSettings.tileShapeIndex = 0;
        }
        SetShape();
        OnShapeChanged?.Invoke();
    }
    private void SetShape()
    {
        List<TilePosition> listToCheck = map.grid.GetAll();
        foreach (var item in listToCheck)
        {
            if (item == map.GetEmptyTile())
                continue;
            item.tileObjectData.SetTileShape(gameSettingsSO.tileshapes[gameSettingsSO.gameSettings.tileShapeIndex]);
        }
    }
}