using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings {
    public int wight;
    public int hight;
    public Color color;
    public int colorIndex;
    public MoveType moveType;
    public TileMap mapTile;
    public float currentTime;
    public int moveCount;
    public List<Record> records = new();
    public int tileShapeIndex;
    public Difficulty difficulty;
}
