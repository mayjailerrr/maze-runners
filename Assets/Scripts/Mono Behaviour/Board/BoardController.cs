using UnityEngine;
using System.Collections.Generic;
using MazeRunners;

public class BoardController : MonoBehaviour
{
    public int boardSize = 10;
    private Board board;
    private BoardView BoardView;

    public void ExternalInitialize(Board board, BoardView boardView)
    {
        this.board = board;
        this.BoardView = boardView;

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

