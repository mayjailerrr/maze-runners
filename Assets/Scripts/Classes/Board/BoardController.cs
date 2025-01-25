using UnityEngine;
using System.Collections.Generic;
using MazeRunners;

public class BoardController : MonoBehaviour
{
    public int boardSize = 10;
    private Board board;
    private BoardView boardView;

    public void ExternalInitialize(Board board, BoardView boardView)
    {
        this.board = board;
        this.boardView = boardView;

        if (boardView != null)
        {
            boardView.InitializeTileBoardView(board);
        }
        else
        {
            Debug.LogError("BoardView is not assigned or is not in the same GameObject.");
        }
    }
}

