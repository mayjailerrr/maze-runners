using System;

public static class GameEvents
{
    public static event Action OnTrapTriggered;
    public static event Action OnCollectiblePicked;

    public static event Action OnPatternIlluminate;
    public static event Action OnPatternButtonPressed;
    public static event Action OnMemoryButtonPressed;
    public static event Action OnMemoryPairFound;
    public static event Action<bool> OnMinigameEnd;

    public static event Action OnMinigameStarted;
    public static event Action OnMinigameEnded;

    public static event Action OnAbsorbUsed;
    public static event Action OnAbsorbDamageUsed;
    public static event Action OnCloneUsed;
    public static event Action OnFreezeUsed;
    public static event Action OnHealthDamageUsed;
    public static event Action OnInvisibilityUsed;
    public static event Action OnRampartBuilderUsed;
    public static event Action OnShieldUsed;
    public static event Action OnSpeedBoostUsed;
    public static event Action OnTeleportUsed;
    public static event Action OnWallBombUsed;
    public static event Action OnWallBuilderUsed;
    public static event Action OnWallDestroyerUsed;


    public static void TriggerTrap()
    {
        OnTrapTriggered?.Invoke();
    }

    public static void TriggerCollectible()
    {
        OnCollectiblePicked?.Invoke();
    }

    public static void TriggerPatternIlluminate()
    {
        OnPatternIlluminate?.Invoke();
    }

    public static void TriggerPatternButtonPressed()
    {
        OnPatternButtonPressed?.Invoke();
    }

    public static void TriggerMemoryButtonPressed()
    {
        OnMemoryButtonPressed?.Invoke();
    }

    public static void TriggerMemoryPairFound()
    {
        OnMemoryPairFound?.Invoke();
    }

    public static void TriggerMinigameEnd(bool success)
    {
        OnMinigameEnd?.Invoke(success);
    }

    public static void TriggerMinigameStarted()
    {
        OnMinigameStarted?.Invoke();
    }

    public static void TriggerMinigameEnded()
    {
        OnMinigameEnded?.Invoke();
    }

    public static void TriggerAbsorbUsed()
    {
        OnAbsorbUsed?.Invoke();
    }

    public static void TriggerAbsorbDamageUsed()
    {
        OnAbsorbDamageUsed?.Invoke();
    }

    public static void TriggerCloneUsed()
    {
        OnCloneUsed?.Invoke();
    }

    public static void TriggerFreezeUsed()
    {
        OnFreezeUsed?.Invoke();
    }

    public static void TriggerHealthDamageUsed()
    {
        OnHealthDamageUsed?.Invoke();
    }

    public static void TriggerInvisibilityUsed()
    {
        OnInvisibilityUsed?.Invoke();
    }

    public static void TriggerRampartBuilderUsed()
    {
        OnRampartBuilderUsed?.Invoke();
    }

    public static void TriggerShieldUsed()
    {
        OnShieldUsed?.Invoke();
    }

    public static void TriggerSpeedBoostUsed()
    {
        OnSpeedBoostUsed?.Invoke();
    }

    public static void TriggerTeleportUsed()
    {
        OnTeleportUsed?.Invoke();
    }

    public static void TriggerWallBombUsed()
    {
        OnWallBombUsed?.Invoke();
    }

    public static void TriggerWallBuilderUsed()
    {
        OnWallBuilderUsed?.Invoke();
    }

    public static void TriggerWallDestroyerUsed()
    {
        OnWallDestroyerUsed?.Invoke();
    }

}
