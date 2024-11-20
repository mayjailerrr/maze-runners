using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RenderTextureButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RawImage rawImage;         // The RawImage of the button
    public RenderTexture normalTexture;   // RenderTexture for default state
    public RenderTexture hoverTexture;    // RenderTexture for hover state
    public RenderTexture pressedTexture;  // RenderTexture for pressed state
    public float darkenLevel = 0.5f;      // Darkness level for pressed state

    private MaterialPropertyBlock propertyBlock;
    private Renderer rawImageRenderer;
    private Material rawImageMaterial;

    void Start()
    {
        if (rawImage == null)
            rawImage = GetComponent<RawImage>();

        rawImage.texture = normalTexture; // Set the default texture
        rawImageMaterial = rawImage.material; // Save the material

        // Get the Renderer and MaterialPropertyBlock
      //  rawImageRenderer = rawImage.GetComponent<CanvasRenderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetTextureAndDarkness(hoverTexture, 1.0f); // Hover with no darkness
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetTextureAndDarkness(normalTexture, 1.0f); // Reset to normal
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetTextureAndDarkness(pressedTexture, darkenLevel); // Apply darkness
    }

    private void SetTextureAndDarkness(RenderTexture texture, float darkness)
    {
        rawImage.texture = texture;

        // Apply changes to the MaterialPropertyBlock
        propertyBlock.SetTexture("_MainTex", texture);
        propertyBlock.SetFloat("_Darkness", darkness);
        rawImageRenderer.SetPropertyBlock(propertyBlock);
    }
}
