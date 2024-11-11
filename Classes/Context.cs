public class Context
{
    public Board Board { get; private set; }
    public Player CurrentPlayer { get; private set; }
    public Player EnemyPlayer { get; private set; }
    public List<Tile> CurrentPosition { get; private set; }
    public Tile CurrentTile { get; private set; }

    public Context(Player currentPlayer, Player currentEnemy)
    {
        this.Board = Board.Instance;
        this.CurrentPlayer = currentPlayer;
        this.EnemyPlayer = currentEnemy;
        this.CurrentPosition = new List<Tile>();
        this.CurrentTile = null;
        //this.CurrentTile = Utils.BaseTile;
    }

    public Context UpdatePlayerInstance(List<Tile> position, Tile tile)
    {
        this.CurrentPosition = position;
        this.CurrentCard = card;

        return this;
    }

    //extra
    public void ResetForNewTurn()
    {
        this.CurrentTile = null;
        this.CurrentPosition.Clear();
    }
}