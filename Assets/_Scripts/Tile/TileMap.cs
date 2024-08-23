using UnityEngine;

[System.Serializable]
public class TileMap
{
    public TileGrid grid;
    public int width;
    public int height;
    TilePosition emptyTile;
    public TileMap(int width, int height, Transform parentObject, TileObject tileObject,Sprite sprite)
    {
        this.width = width;
        this.height = height;
        grid = new(width, height);
        Vector2Int emptySpot = new Vector2Int(width - 1, 0);
        int order = 0;
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                order++;
                grid.SetTilePosition(new(x, y));
                grid.GetTilePosition(x, y).SetOrder(order);
                if (x == emptySpot.x && y == emptySpot.y)
                {
                    emptyTile = grid.GetTilePosition(x, y);
                    continue;
                }
                TileObject temp = GameObject.Instantiate(tileObject, parentObject);
                temp.SetTileData(new(order));
                temp.transform.position = new Vector2(x, y);
                temp.tileObjectData.NumberChanged(order);
                temp.tileObjectData.SetTileShape(sprite);

                grid.GetTilePosition(x, y).tileObject = temp.tileObjectData;
            }
        }
    }
    public TilePosition GetEmptyTile() => emptyTile;
    public void SetEmptyTile(int x, int y)
    {
        grid.GetTilePosition(x, y).tileObject = null;
        emptyTile = grid.GetTilePosition(x, y);
    }
}