using UnityEngine;
using MazeRunners;
public class TurnManager : MonoBehaviour
{
    //    TurnManager: Administra los turnos de los jugadores y controla las secuencias de juego.

    //     Propiedades: CurrentPlayer, TurnNumber
    //     Métodos: NextTurn(), ProcessTurnActions()

    public Player CurrentPlayer { get; private set; }
    
}