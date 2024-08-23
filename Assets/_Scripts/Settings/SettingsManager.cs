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
    private void Start()
    {
      //  map = MapGenerator.Instance.map;
      //  MapGenerator.Instance.OnMapChanged += () => map = MapGenerator.Instance.map;
        ChangeColor();
    }
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
            item.tileObject.SetColor(gameSettingsSO.gameSettings.color);
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
            item.tileObject.SetTileShape(gameSettingsSO.tileshapes[gameSettingsSO.gameSettings.tileShapeIndex]);
        }
    }
}