using UnityEngine;

[System.Serializable]
public class TilePosition
{
    public int x;
    public int y;
    public TileObjectData tileObject;
    [SerializeField] int order;
    public TilePosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void SetOrder(int order)
    {
        this.order = order;
    }
    public int GetOrder()
    {
        return order;
    }
    public Vector2 Position()
    {
        return new Vector2(x, y);
    }
}