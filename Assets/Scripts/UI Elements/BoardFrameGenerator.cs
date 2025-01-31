using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BoardFrameGenerator : MonoBehaviour
{
    public GameObject numberPrefab;
    public GridLayoutGroup boardContainer;
    public Transform topRow, bottomRow, leftColumn, rightColumn;

    public int boardSize = 0;

    private readonly Color[] numberColors = 
    {
        Color.white,
        new Color(0.6f, 0.8f, 1f),
        new Color(0.7f, 1f, 0.7f), 
        new Color(1f, 0.85f, 0.6f),
        new Color(0.8f, 0.7f, 1f)
    };

    private void Start()
    {
        StartCoroutine(WaitForBoardGeneration());
    }

    private IEnumerator WaitForBoardGeneration()
    {
        while (boardContainer.transform.childCount == 0)
        {
            yield return null; 
        }

        boardSize = Mathf.RoundToInt(Mathf.Sqrt(boardContainer.transform.childCount));

        GenerateFrame();
    }

    private void GenerateFrame()
    {
        for (int x = 0; x < boardSize; x++)
        {
            int colorIndex = x; 

            CreateNumber(topRow, x, colorIndex);

            CreateNumber(bottomRow, x, colorIndex);
        }

        for (int y = 0; y < boardSize; y++)
        {
            int colorIndex = y; 

            CreateNumber(leftColumn, y, colorIndex);

            CreateNumber(rightColumn, y, colorIndex);
        }
    }


    private void CreateNumber(Transform parent, int value, int colorIndex)
    {
        GameObject numberObject = Instantiate(numberPrefab, parent);
        TextMeshProUGUI numberText = numberObject.GetComponent<TextMeshProUGUI>();

        if (numberText == null)
        {
            Debug.LogError("El prefabNumber no tiene un componente TextMeshProUGUI.");
            return;
        }

        numberText.text = value.ToString();
        numberText.color = numberColors[colorIndex % numberColors.Length];
    }
}
