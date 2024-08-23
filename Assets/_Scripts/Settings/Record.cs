[System.Serializable]
public class Record{
    public readonly int width, height;
    public readonly int moveCount;
    public readonly float time;
    public Record(int width, int height, int moveCount,float time) {
        this.width = width;
        this.height = height;
        this.moveCount = moveCount;
        this.time = time;
    }
}
