using UnityEngine;

public class BoardFloatingEffect : MonoBehaviour
{
    public Transform boardContainer;
    public Transform frameContainer;
    public float floatHeight = 10f;
    public float floatSpeed = 0.8f;

    private Vector3 initialBoardLocalPos;
    private Vector3 initialFrameLocalPos;

    private void Start()
    {
        initialBoardLocalPos = boardContainer.localPosition;
        initialFrameLocalPos = frameContainer.localPosition;
    }

    private void Update()
    {
        float floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        Vector3 floatingOffset = new Vector3(0, floatOffset, 0);

        boardContainer.localPosition = initialBoardLocalPos + floatingOffset;
        frameContainer.localPosition = initialFrameLocalPos + floatingOffset;
    }
}
