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
    
}

