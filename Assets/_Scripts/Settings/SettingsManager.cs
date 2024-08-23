using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameSettingsSO gameSettingsSO;
    TileMap map;
    public static SettingsManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    // Setted in Map Gnerator
    public void SetMap(TileMap map)
    {
        this.map = map;
    }
    
    public void ChangeColor()
    {
        List<TilePosition> listToCheck = map.grid.GetAll();
        foreach (var item in listToCheck)
        {
            if (item == map.GetEmptyTile())
                continue;
            item.tileObjectData.SetColor(gameSettingsSO.gameSettings.color);
        }
    } 
    public void ChangeShape()
    {
        gameSettingsSO.gameSettings.tileShapeIndex++;
        if (gameSettingsSO.gameSettings.tileShapeIndex >= gameSettingsSO.tileshapes.Length)
        {
            gameSettingsSO.gameSettings.tileShapeIndex = 0;
        }
        SetShape();
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