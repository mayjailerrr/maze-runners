using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlindnessEffectHandler : MonoBehaviour
{
    public Canvas targetCanvas; 
    private GameObject blindnessOverlay;
    public TextMeshProUGUI blindnessText;
    private Context gameContext;
    private Player player;

    private void Start()
    {
        blindnessText.text = "";
        if (targetCanvas == null)
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            foreach (var canvas in canvases)
            {
                if (canvas.name == "MainCanvas")
                {
                    targetCanvas = canvas;
                    break;
                }
            }
        }
    }


    private void Update()
    {
        if (gameContext == null)
        {
            gameContext = FindObjectOfType<GameManager>().GameContext;
            if (gameContext != null)
            {
                player = gameContext.CurrentPlayer;
            }
            return;
        }

        if (player == null || gameContext.CurrentPlayer != player)
        {
            player = gameContext.CurrentPlayer;
            if (blindnessOverlay != null)
            {
                blindnessText.text = "";
                Destroy(blindnessOverlay);
            }
            return;
        }

        if (player.IsBlinded)
        {
            if (blindnessOverlay == null)
            {
                blindnessText.text = "You are under the effect of a Blindness Trap!";
                CreateBlindnessOverlay();
            }
        }
        else if (blindnessOverlay != null)
        {
            blindnessText.text = "";
            Destroy(blindnessOverlay);
        }
    }

    private void CreateBlindnessOverlay()
    {
        blindnessOverlay = new GameObject("BlindnessOverlay");
        RectTransform rectTransform = blindnessOverlay.AddComponent<RectTransform>();
        rectTransform.SetParent(targetCanvas.transform, false);
        rectTransform.sizeDelta = new Vector2(1980, 1090);

        Image image = blindnessOverlay.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 1f);

        blindnessText.transform.SetParent(targetCanvas.transform, false);
        blindnessText.transform.SetAsLastSibling();

    }

}
