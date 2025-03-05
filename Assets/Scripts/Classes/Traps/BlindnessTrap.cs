using UnityEngine;

public class BlindnessTrap : ITrapEffect
{
    private readonly int blindnessDuration = 3;
    private Player affectedPlayer;
    private Context gameContext;

    public string Description => "The affected player rests blind for several turns.";

    public BlindnessTrap() {}

    public void ApplyEffect(Context context)
    {
        affectedPlayer = context.CurrentPlayer;
        gameContext = context;

        var blindnessEffect = new PropertyTemporaryEffect(affectedPlayer, "IsBlinded", true, blindnessDuration);
        gameContext.TurnManager.ApplyTemporaryEffect(blindnessEffect);

        Debug.Log($"Player {affectedPlayer.ID + 1} have been blinded for {blindnessDuration} turn(s) by a Blindness Trap!");
    }
}
