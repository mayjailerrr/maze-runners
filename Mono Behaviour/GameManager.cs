public class GameManager : MonoBehaviour
{
    

    public int numPlayers;
    public int boardSize;
    public Board board;
    public List<Player> players;
    private int currentPlayerIndex;

    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        board = new Board(boardSize);
        players = new List<Player>();

        // Create players and assign pieces
        for (int i = 0; i < numPlayers; i++)
        {
            Player player = new Player(i);
            AssignPiecesToPlayer(player);
            players.Add(player);
        }

        //generate board and put elements in the labrynth
        board.GenerateBoard();
        board.PlaceObstacles();
        board.PlaceTraps();
        board.PlaceExits();

        currentPlayerIndex = 0; // Start with the first player
    }

    private void AssignPiecesToPlayer(Player player)
    {
        player.AddPiece(new Piece("VacaParacaidista"));  
        player.AddPiece(new Piece("SoldadoTanque"));
         //player.AddFicha(new VacaParacaidista());
        // player.AddFicha(new SoldadoTanque());
    }

    private void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }

    public void PlayTurn(Piece selectedPiece, int targetX, int targetY)
    {
        Player currentPlayer = players[currentPlayerIndex];

        //move piece and use ability if possible
        currentPlayer.MovePiece(selectedPiece, targetX, targetY, board);
        currentPlayer.UsePieceAbility(selectedPiece);

        //check win condition
        if(CheckWinCondition(currentPlayer)) EndGame(currentPlayer);
       
        else NextTurn();
    
    }

    private bool CheckWinCondition(Player player)
    {
        //IMPLEMENT: Condition for winning the game
        return board.AreAllPiecesAtExits(player);
    }
    
    private void EndGame(Player winner)
    {
        Debug.Log($"Player {winner.ID} wins!");
        //IMPLEMENT: Game over logic 
    }
}