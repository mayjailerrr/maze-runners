using UnityEngine;
using MazeRunners;
public class ExitTile : Tile
{
   // public string Name { get; private set; }
    public string RepresentingMovie { get; private set; }
    public Sprite ObjectSprite { get; private set; }

    public ExitTile(int x, int y) : base(x, y)
    {
        //Name = name;
      //  RepresentingMovie = representingMovie;
        //ObjectSprite = objectSprite;
    }

    public override void OnEnter(Piece piece)
    {
        base.OnEnter(piece);

       // if (IsWinningPiece(piece)) TriggerVictory(piece);
    }

    // private bool IsWinningPiece(Piece piece)
    // {
    //    // return piece.Movie == RepresentingObject;
    // }

    private void TriggerVictory(Piece piece)
    {
      //  Debug.Log($"{piece.Name} ha alcanzado el objeto {RepresentingObject} y ha ganado el juego!");
        // Aquí podrías agregar lógica para mostrar la imagen flotante, lanzar un evento de victoria, etc.
    }
}