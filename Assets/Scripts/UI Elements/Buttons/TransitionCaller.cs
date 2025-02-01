using UnityEngine;

public class TransitionCaller : MonoBehaviour
{
    public TransitionManager transitionManager;

    public void OnButtonPress()
    {
        transitionManager.CheckAndStartTransition(null);
    }
}
