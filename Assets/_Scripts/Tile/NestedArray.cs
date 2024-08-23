[System.Serializable]
public class NestedArray
{
    public TilePosition[] testTileX;
    public NestedArray(int height)
    {
        testTileX = new TilePosition[height];
    }
}
