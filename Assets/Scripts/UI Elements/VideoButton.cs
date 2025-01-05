using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RenderTextureButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RawImage rawImage;         
    public RenderTexture normalTexture;   
    public RenderTexture hoverTexture;   
    public RenderTexture pressedTexture;  
    public float darkenLevel = 0.5f;    

    private MaterialPropertyBlock propertyBlock;
    private Renderer rawImageRenderer;
    private Material rawImageMaterial;

    void Start()
    {
        if (rawImage == null)
            rawImage = GetComponent<RawImage>();

        rawImage.texture = normalTexture; 
        rawImageMaterial = rawImage.material;

        propertyBlock = new MaterialPropertyBlock();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetTextureAndDarkness(hoverTexture, 1.0f); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetTextureAndDarkness(normalTexture, 1.0f); 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetTextureAndDarkness(pressedTexture, darkenLevel); 
    }

    private void SetTextureAndDarkness(RenderTexture texture, float darkness)
    {
        rawImage.texture = texture;

        propertyBlock.SetTexture("_MainTex", texture);
        propertyBlock.SetFloat("_Darkness", darkness);
    }
}
