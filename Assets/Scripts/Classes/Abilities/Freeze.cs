
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FreezeAbility : IAbility
{
    private int selectedPieceIndex;
    public string Description => "Freezes the target piece, preventing movement and abilities for a specified number of turns.";

    public bool Execute(Context context)
    {
        Player nextPlayer = context.TurnManager.GetNextPlayer(context.CurrentPlayer);

        if (nextPlayer == null || nextPlayer.Pieces.Count == 0)
        {
            Debug.LogError("No valid target pieces.");
            return false;
        }

        var validTargets = nextPlayer.Pieces.Where(piece => !piece.IsShielded).ToList();

        if (validTargets.Count == 0)
        {
            Debug.LogWarning("All target pieces are shielded. No freeze applied.");
            return false;
        }

        System.Random random = new System.Random();
        selectedPieceIndex = random.Next(0, validTargets.Count);

        Piece targetPiece = validTargets[selectedPieceIndex];
        Piece currentPiece = context.CurrentPiece;
        Debug.Log($"Target piece: {targetPiece?.Name}");

        int freezeTurns = 3;
        var turnManager = context.TurnManager;

        ShowFreezeIndicator(targetPiece);

        var freezeMovementEffect = new PropertyTemporaryEffect(targetPiece, "Speed", 0, freezeTurns);
        turnManager.ApplyTemporaryEffect(freezeMovementEffect);

        var freezeAbilitiesEffect = new ActionTemporaryEffect(
            targetPiece,
            () => targetPiece.AbilitiesBlocked = true,
            () =>
            {
                targetPiece.AbilitiesBlocked = false;
                HideFreezeIndicator(targetPiece);
            },
            freezeTurns
        );
        turnManager.ApplyTemporaryEffect(freezeAbilitiesEffect);

        context.CurrentPlayer.RecordAbilityUse();
        currentPiece.View.PlayAbilityEffect(new Color(0.68f, 0.85f, 1f, 0.8f));
        currentPiece.View.PlayAbilitySound();
        
        Debug.Log($"Piece {targetPiece.Name} has been frozen for {freezeTurns} turns.");
       
        return true;
    }

    private void ShowFreezeIndicator(Piece targetPiece)
    {
        var freezeIndicator = new GameObject("FreezeIndicator");
        var image = freezeIndicator.AddComponent<Image>();

        image.color = new Color(0f, 0.5f, 1f, 0.5f);
        image.sprite = CreateCircleSprite(8);

        freezeIndicator.transform.SetParent(targetPiece.View.transform, false);
        freezeIndicator.transform.localPosition = Vector3.zero;
        freezeIndicator.transform.localScale = Vector3.one * 1.5f;
        freezeIndicator.transform.SetAsLastSibling();

        targetPiece.View.FreezeIndicator = freezeIndicator;

        LeanTween.alpha(freezeIndicator.GetComponent<RectTransform>(), 0.2f, 0.5f)
            .setLoopPingPong();
    }

    private void HideFreezeIndicator(Piece targetPiece)
    {
        if (targetPiece.View.FreezeIndicator != null)
        {
            LeanTween.cancel(targetPiece.View.FreezeIndicator);
            GameObject.Destroy(targetPiece.View.FreezeIndicator);
            targetPiece.View.FreezeIndicator = null;
        }
    }

    private Sprite CreateCircleSprite(int diameter)
    {
        Texture2D texture = new Texture2D(diameter, diameter, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[diameter * diameter];

        int radius = diameter / 2;
        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                Vector2 pixelPos = new Vector2(x, y);
                float distance = Vector2.Distance(pixelPos, center);

                pixels[y * diameter + x] = distance <= radius ? Color.white : Color.clear;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        Rect rect = new Rect(0, 0, diameter, diameter);
        return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }
}
