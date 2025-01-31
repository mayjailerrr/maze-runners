using System;
using MazeRunners;
using UnityEngine;
using UnityEngine.UI;

public class ShieldAbility : IAbility
{
    public string Description => "Shield the piece from damage for a specified number of turns.";
    private int shieldTurns = 3;
    private GameObject shieldIndicator;

    public bool Execute(Context context)
    {
        var targetPiece = context.CurrentPiece;
        if (targetPiece == null)
        {
            Debug.LogError("No piece selected to shield.");
            return false;
        }

        Debug.Log($"Applying shield to piece {targetPiece.Name} for {shieldTurns} turns.");

        ShowShield(targetPiece);

        var shieldEffect = new ActionTemporaryEffect(
            targetPiece,
            () => targetPiece.IsShielded = true,
            () =>
            {
                targetPiece.IsShielded = false;
                HideShield();
            },
            shieldTurns
        );

        context.TurnManager.ApplyTemporaryEffect(shieldEffect);
        context.CurrentPlayer.RecordAbilityUse();
        
        Debug.Log($"{targetPiece.Name} is now shielded for {shieldTurns} turns.");
        return true;
    }

    private void ShowShield(Piece targetPiece)
    {
        shieldIndicator = new GameObject("ShieldIndicator");
        var image = shieldIndicator.AddComponent<Image>();

        image.sprite = CreateCircleSprite(8);
        image.color = new Color(1f, 0.84f, 0f, 0.4f);

        shieldIndicator.transform.SetParent(targetPiece.View.transform, false);
        shieldIndicator.transform.localScale = Vector3.one * 1.5f;
        shieldIndicator.transform.localPosition = Vector3.zero;
        shieldIndicator.transform.SetAsLastSibling();

        LeanTween.rotateAroundLocal(shieldIndicator, Vector3.forward, 360f, 2f)
            .setLoopClamp();

        shieldIndicator.SetActive(true);

        Debug.Log("Shield effect activated!");
    }

    private void HideShield()
    {
        if (shieldIndicator != null)
        {
            LeanTween.cancel(shieldIndicator);
            GameObject.Destroy(shieldIndicator);
            shieldIndicator = null;
        }

        Debug.Log("Shield effect deactivated!");
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
