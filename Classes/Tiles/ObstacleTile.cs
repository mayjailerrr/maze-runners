public class ObstacleTile : Tile
{
    public ObstacleTile(int x, int y) : base(x, y) { }

    public override void OnEnter(Piece piece)
    {
       Debug.Log("You hit an obstacle!");
    }
}