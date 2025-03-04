using UnityEngine;
public class CollectibleFloatingEffect : MonoBehaviour
{
    private float floatSpeed = 5.3f;
    private float floatHeight = 3f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}