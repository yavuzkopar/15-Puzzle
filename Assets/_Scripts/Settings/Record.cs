[System.Serializable]
public class Record{
    public int width, height;
    public int moveCount;
    public float time;
    public Record(int width, int height, int moveCount,float time) {
        this.width = width;
        this.height = height;
        this.moveCount = moveCount;
        this.time = time;
    }
}
