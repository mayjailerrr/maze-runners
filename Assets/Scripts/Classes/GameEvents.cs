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
}
