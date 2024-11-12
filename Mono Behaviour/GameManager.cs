public class GameManager : MonoBehaviour
{
    //Clase central para gestionar el flujo general del juego (inicialización, turnos, condiciones de victoria).

    // Propiedades: Players, Board, TurnManager
    // Métodos: StartGame(), EndGame(), CheckWinCondition()

    public int numPlayers;
    public int boardSize;
    public Board board;
    private Dictionary<int, Player> players;
    private int currentPlayerIndex;
    private List<string> availableMovies = new List<string>
    {
        "Ponyo", "Spirited Away", "Kiki's Delivery Service", "Howl's Moving Castle", "Princess Mononoke", "My Neighbor Totoro"
    };

    //change it to list of pieces
    private Dictionary<string, List<string>> movieCharacters = new Dictionary<string, List<string>>
    {
        { "Ponyo", new List<string> { "Ponyo", "Sosuke", "Gran Mamare" } },
        { "Spirited Away", new List<string> { "Chihiro", "Haku", "No-Face" } },
        { "Kiki's Delivery Service", new List<string> { "Kiki", "Jiji", "Tombo" } },
        { "Howl's Moving Castle", new List<string> { "Sophie", "Howl", "Turnip Head" } },
        { "Princess Mononoke", new List<string> { "Ashitaka", "San", "Lady Eboshi" } },
        { "My Neighbor Totoro", new List<string> { "Totoro", "Satsuki", "Mei" } }
    };
   
    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        board = new Board(boardSize);
        players = new Dictionary<int, Player>();

        for (int i = 0; i < numPlayers; i++)
        {
            Player player = new Player(i);
            //AssignPiecesToPlayer(player);
            players.Add(i, player);
        }

        board.GenerateBoard();
        board.PlaceObstacles();
        board.PlaceTraps();
        board.PlaceExits();

        currentPlayerIndex = 0;
    }

    public void SelectPlayerMovie(int playerId, string selectedMovie)
    {
        if (players.ContainsKey(playerId) && availableMovies.Contains(selectedMovie))
        {
            Player player = players[playerId];
            player.SelectedMovie = selectedMovie;
            availableMovies.Remove(selectedMovie);

            AssignPiecesToPlayerBasedOnMovie(player, selectedMovie);
        }

        else Debug.Log("Invalid movie selection or player ID");
    }

    //TO-DO: CHANGE THIS
    private void AssignPiecesToPlayerBasedOnMovie(Player player, string movie)
    {
        if (movieCharacters.TryGetValue(movie, out List<string> characters))
        {
            foreach (string characterName in characters)
            {
                player.AddPiece(new Piece(characterName));
            }
        }
        else Debug.Log($"No characters found for movie")
       
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