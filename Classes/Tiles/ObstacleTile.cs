public class ObstacleTile : Tile
{
    public ObstacleTile(int x, int y) : base(x, y) { }

    public override void OnEnter(Piece piece)
    {
        //IMPELEMENT: Pieces cannot step on obstacles
    }
}