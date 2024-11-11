using UnityEngine;

[CreateAssetMenu(fileName = "NewPieceData", menuName = "MazeGame/PieceData")]
public class PieceData : ScriptableObject
{
    public string pieceName;
    public int speed;
    public int cooldown;
    public string movie;
    public Sprite pieceSprite;
    public AudioClip abilitySound;
    // Agrega otras propiedades según sea necesario, como habilidades
}

// public class Piece : MonoBehaviour
// {
//     public PieceData pieceData;

//     private void Start()
//     {
//         // Inicializa las propiedades usando el ScriptableObject
//         if (pieceData != null)
//         {
//             Name = pieceData.pieceName;
//             Speed = pieceData.speed;
//             Cooldown = pieceData.cooldown;
//             Movie = pieceData.movie;
//             GetComponent<SpriteRenderer>().sprite = pieceData.pieceSprite;
//         }
//     }

//     // Otros métodos como Move(), UseAbility(), etc.
// }
