using UnityEngine;

public class ExampleCaller : MonoBehaviour
{
    public TransitionManager transitionManager;

    public void OnButtonPress()
    {
        transitionManager.StartTransition(() =>
        {
            Debug.Log("Transition Complete!");
        });
    }
}
