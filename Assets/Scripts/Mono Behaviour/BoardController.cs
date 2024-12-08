using UnityEngine;

public class BoardController : MonoBehaviour
{
    public int boardSize = 10;
    private Board board;
    private BoardView boardView;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        board = new Board(boardSize); 
        boardView = GetComponent<BoardView>();

        if (boardView != null)
        {
            boardView.InitializeView(board);
        }
        else
        {
            Debug.LogError("BoardView is not assigned or is not in the same GameObject.");
        }
    }

}
