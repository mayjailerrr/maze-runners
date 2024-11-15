// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class MovieSelectionMenu : MonoBehaviour
// {
//     // Diccionario que asocia el nombre de la película con la lista de piezas
//     private Dictionary<string, List<Piece>> moviePieces = new Dictionary<string, List<Piece>>();

//     public GameManager gameManager;
//     public TMP_InputField inputField;

//     private int currentPlayerIndex = 0;

//     void Start()
//     {
//         InitializeMoviePieces();
//     }

//     // Inicializa el diccionario con las piezas correspondientes para cada película
//     private void InitializeMoviePieces()
//     {
//         moviePieces["Spirited Away"] = new List<Piece> { new Piece("Chihiro"), new Piece("Haku"), new Piece("No-Face") };
//         moviePieces["Ponyo"] = new List<Piece> { new Piece("Ponyo"), new Piece("Sosuke"), new Piece("Fujimoto") };
//         moviePieces["Howl's Moving Castle"] = new List<Piece> { new Piece("Howl"), new Piece("Sophie"), new Piece("Calcifer") };
//         moviePieces["Princess Mononoke"] = new List<Piece> { new Piece("Ashitaka"), new Piece("San"), new Piece("Moro") };
//         moviePieces["Kiki's Delivery Service"] = new List<Piece> { new Piece("Kiki"), new Piece("Jiji"), new Piece("Tombo") };
//         moviePieces["My Neighbor Totoro"] = new List<Piece> { new Piece("Totoro"), new Piece("Mei"), new Piece("Satsuki") };
//         moviePieces["The Wind Rises"] = new List<Piece> { new Piece("Jiro"), new Piece("Naoko"), new Piece("Kurokawa") };
//         moviePieces["Castle in the Sky"] = new List<Piece> { new Piece("Pazu"), new Piece("Sheeta"), new Piece("Dola") };
//     }

//     // Método para seleccionar una película para el jugador actual
//     public void SelectMovie(string movieName)
//     {
//         if (currentPlayerIndex < gameManager.numPlayers && moviePieces.ContainsKey(movieName))
//         {
//             Player currentPlayer = gameManager.players[currentPlayerIndex];
//             gameManager.AssignPiecesToPlayer(currentPlayer, moviePieces[movieName]);

//             Debug.Log($"Player {currentPlayer.ID} has selected {movieName}.");

//             currentPlayerIndex++; // Pasar al siguiente jugador
//         }
//         else
//         {
//             Debug.LogWarning("Movie not found or maximum players reached.");
//         }

//         CheckStartGame();
//     }

//     // Verifica si todos los jugadores han seleccionado una película para comenzar el juego
//     private void CheckStartGame()
//     {
//         if (currentPlayerIndex >= gameManager.numPlayers)
//         {
//             gameManager.StartGame(); // Llamar al método de inicio del juego en el GameManager
//         }
//     }

//     // Métodos para los botones de selección de películas
//     public void SelectSpiritedAway() => SelectMovie("Spirited Away");
//     public void SelectPonyo() => SelectMovie("Ponyo");
//     public void SelectMovingCastle() => SelectMovie("Howl's Moving Castle");
//     public void SelectMononoke() => SelectMovie("Princess Mononoke");
//     public void SelectKikis() => SelectMovie("Kiki's Delivery Service");
//     public void SelectTotoro() => SelectMovie("My Neighbor Totoro");
//     public void SelectWindRises() => SelectMovie("The Wind Rises");
//     public void SelectCastleInTheSky() => SelectMovie("Castle in the Sky");
// }

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovieSelectionMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField input;
    public List<string> availableMovies = new List<string> 
    { 
        "Spirited Away", 
        "My Neighbor Totoro", 
        "Howl's Moving Castle", 
        "Princess Mononoke", 
        "Kiki's Delivery Service", 
        "Ponyo", 
        "The Wind Rises", 
        "Castle in the Sky" 
    };
    
    private int currentPlayerIndex = 0;

    private void Start()
    {
        UpdateInputVisibility();
    }

    // Muestra el campo de texto solo si hay más de 4 jugadores.
    private void UpdateInputVisibility()
    {
        input.gameObject.SetActive(currentPlayerIndex >= 4);
    }

    public void SelectMovie(string movieName)
    {
        if (currentPlayerIndex >= GameManager.Instance.MaxPlayers)
        {
            Debug.Log("Maximum players reached.");
            return;
        }

        if (availableMovies.Contains(movieName))
        {
            List<Piece> pieces = PieceFactory.CreatePieces(movieName);
            Player player = new Player(currentPlayerIndex, pieces); // Crea el jugador con sus piezas
            GameManager.Instance.AddPlayer(player); // Añade el jugador al juego

            availableMovies.Remove(movieName); // Elimina la película seleccionada de la lista
            currentPlayerIndex++; // Avanza al siguiente jugador
            UpdateInputVisibility();

            Debug.Log($"Player {currentPlayerIndex} selected {movieName}");
        }
        else
        {
            Debug.Log("Movie not available or already selected.");
        }
    }

    public bool CanStartGame()
    {
        return currentPlayerIndex >= 2; // Permite empezar solo si hay al menos 2 jugadores.
    }

    public void StartGameIfReady()
    {
        if (CanStartGame())
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            Debug.Log("At least 2 players are needed to start the game.");
        }
    }
}

