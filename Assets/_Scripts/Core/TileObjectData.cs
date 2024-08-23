using System;
using UnityEngine;

[System.Serializable]
public class TileObjectData
{
    public int number;
    public Color color;
    public int shapeIndex;
    public bool isMoving;
    public Action<Vector2,MoveType> OnMove;
    public Action<Color> OnColorChanged;
    public Action<int> OnNumberChanged;
    public Action<Sprite> OnShapeChanged;
    public TileObjectData(int number)
    {
        this.number = number;
    }
    public Action<Vector2> OnTelepoerted;
    public void Teleport(Vector2 vector2){
        OnTelepoerted?.Invoke(vector2);
    }
    public void Move(Vector2 vector2,MoveType moveType)
    {
        OnMove?.Invoke(vector2,moveType);
    }
    public void SetColor(Color color)
    {
        this.color = color;
        OnColorChanged?.Invoke(color);
    }
    public void NumberChanged(int number)
    {
        this.number = number;
        OnNumberChanged?.Invoke(number);
    }

    public void SetTileShape(Sprite tileShape)
    {
        OnShapeChanged?.Invoke(tileShape);
    }
}
