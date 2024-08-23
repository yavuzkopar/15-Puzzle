using System.Collections.Generic;

[System.Serializable]
public class TileGrid//I ısed this method instead of TilePosition[,] because I wanted to serialize to json 
{
    public NestedArray[] grid;
    public int width, height;
    public TileGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
        grid = new NestedArray[width];
        for (int i = 0; i < width; i++)
        {
            grid[i] = new NestedArray(height);
            for (int j = 0; j < height; j++)
            {
                grid[i].testTileX[j] = new TilePosition(i, j);
            }
        }
    }
    public List<TilePosition> GetAll()
    {
        List<TilePosition> result = new();
        foreach (var item in grid)
        {
            foreach (var tp in item.testTileX)
            {
                result.Add(tp);
            }
        }
        return result;
    }
    public TilePosition GetTilePosition(int x, int y)
    {
        return grid[x].testTileX[y];
    }
    public void SetTilePosition(TilePosition tilePosition)
    {
        grid[tilePosition.x].testTileX[tilePosition.y] = tilePosition;
    }
    public void SetTilePosition(int x, int y, TilePosition tilePosition)
    {
        grid[x].testTileX[y] = tilePosition;
    }
}
