using UnityEngine;

public class BoardController : MonoBehaviour
{
    public int boardSize = 10;
    private Board board;
    private BoardView boardView;

    private Context gameContext;

    private void Start()          //to-do: i want to take this away, but when i do the board doesnt show up on scene
    {
        Initialize();
        InitializeContext();
    }

    public void Initialize()
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

    private void InitializeContext()
    {
        
        //to - do
    }

}
