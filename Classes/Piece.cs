public abstract class Piece
{
  

    public string Name { get; protected set; }
    public int Speed { get; protected set; }
    public int Cooldown { get; protected set; }
    public int CurrentCooldown { get; private set; }
    public (int x, int y) Position { get; set; }
    public string Movie { get; private set; }

    public Piece (string name, int speed, int cooldown, string movie)
    {
        Name = name;
        Speed = speed;
        Cooldown = cooldown;
        CurrentCooldown = 0;
        Movie = movie;
    }

    public abstract void UseAbility();

    public void Move(int newX, int newY)
    {
        Position = (newX, newY);
        Console.WriteLine($"{Name} moved to ({newX}, {newY})");

    }

    public void UpdateCooldown()
    {
        if (CurrentCooldown > 0) CurrentCooldown--;
    }

    public bool CanUseAbility()
    {
        return CurrentCooldown == 0;
    }

    protected void ActivateAbility()
    {
        CurrentCooldown = Cooldown;
    }

}

// public class Piece
// {
//     public string Name { get; protected set; }
//     public int Speed { get; protected set; }
//     public int Cooldown { get; protected set; }
//     public int CurrentCooldown { get; private set; }
//     public (int x, int y) Position { get; private set; }
//     public string Movie { get; private set; } // Película asociada
//     private Tile[,] board; // Referencia al tablero

//     public Piece(string name, int speed, int cooldown, string movie, Tile[,] board)
//     {
//         Name = name;
//         Speed = speed;
//         Cooldown = cooldown;
//         CurrentCooldown = 0;
//         Movie = movie;
//         this.board = board;
//     }

//     public bool Move(int newX, int newY)
//     {
//         // Verifica que la posición esté dentro del tablero
//         if (newX < 0 || newY < 0 || newX >= board.GetLength(0) || newY >= board.GetLength(1))
//         {
//             Console.WriteLine("Movimiento fuera de los límites del tablero.");
//             return false;
//         }

//         // Obtiene la casilla de destino
//         Tile targetTile = board[newX, newY];

//         // Llama a OnEnter para verificar si la ficha puede entrar
//         if (targetTile is ObstacleTile)
//         {
//             Console.WriteLine($"{Name} no puede moverse a una casilla de obstáculo en ({newX}, {newY}).");
//             return false;
//         }

//         targetTile.OnEnter(this); // Ejecuta lógica de la casilla al entrar

//         // Si pasa las verificaciones, mueve la pieza
//         Position = (newX, newY);
//         Console.WriteLine($"{Name} se ha movido a la posición ({newX}, {newY}).");
//         return true;
//     }
// }
