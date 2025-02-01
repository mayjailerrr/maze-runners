using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;
using System.Collections;

public class TeleportAbility : IAbility
{
    public string Description => "Teleports to a random tile.";

    public bool Execute(Context context)
    {
        var pieceView = context.CurrentPiece.View;
        if (pieceView == null)
        {
            Debug.LogError("Piece view is null.");
            return false;
        }

        var randomTile = context.Board.GetRandomTile();
        if (randomTile == null)
        {
            Debug.LogError("No valid tile to teletransport to.");
            return false;
        }

        context.CurrentPiece.Position = (randomTile.Position.x, randomTile.Position.y);
        context.CurrentPlayer.RecordAbilityUse();
        pieceView.PlayAbilitySound();
        
        Debug.Log($"Piece teleported to ({randomTile.Position.x}, {randomTile.Position.y})");

        pieceView.StartCoroutine(TeleportEffect(pieceView, randomTile, context));
       
        return true;
    }

    private IEnumerator TeleportEffect(PieceView pieceView, Tile targetTile, Context context)
    {
        LeanTween.scale(pieceView.gameObject, Vector3.zero, 0.3f).setEaseInOutQuad();

        yield return new WaitForSeconds(0.3f);

        context.CurrentPiece.Position = (targetTile.Position.x, targetTile.Position.y);

        var targetTileGO = context.BoardView.GetTileObject(targetTile.Position.x, targetTile.Position.y);
        if(targetTileGO != null)
        {
            pieceView.transform.SetParent(targetTileGO.transform, false);
            pieceView.transform.localScale = Vector3.one;
            pieceView.transform.localPosition = Vector3.zero;
        }
        else 
        {
            Debug.LogError("Target tile game object is null.");
        }
      
        LeanTween.scale(pieceView.gameObject, Vector3.one, 0.3f).setEaseInOutQuad();

        Debug.Log($"Piece teleported to ({targetTile.Position.x}, {targetTile.Position.y})");
    }
}