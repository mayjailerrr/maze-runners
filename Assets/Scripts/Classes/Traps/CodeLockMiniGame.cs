using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CodeLockMinigame : MonoBehaviour
{
    public GameObject panel;
    public Transform gridParent;
    public GameObject nodePrefab;
    public float displayTime = 2f;
    public float timeLimit = 5f;
    
    private List<int> correctPattern = new List<int>();
    private List<int> playerPattern = new List<int>();
    private Dictionary<int, Button> nodes = new Dictionary<int, Button>();
    private bool isActive = false;
    private float timer;
    private Action<bool> onComplete;
    
    private void Start()
    {
        panel.SetActive(false);
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject node = Instantiate(nodePrefab, gridParent);
            Button button = node.GetComponent<Button>();
            int index = i;
            button.onClick.AddListener(() => OnNodePressed(index));
            nodes.Add(index, button);
        }
    }

    public void StartMinigame(Action<bool> callback)
    {
        onComplete = callback;
        panel.SetActive(true);

        if (nodes.Count == 0)
        {
            GenerateGrid();
        }

        GeneratePattern();
        StartCoroutine(DisplayPattern());
    }
    
    private void GeneratePattern()
    {
        correctPattern.Clear();
        playerPattern.Clear();
        
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < 9; i++) availableIndexes.Add(i);
        
        for (int i = 0; i < UnityEngine.Random.Range(3, 6); i++)
        {
            int index = availableIndexes[UnityEngine.Random.Range(0, availableIndexes.Count)];
            correctPattern.Add(index);
            availableIndexes.Remove(index);
        }
    }
    
    private IEnumerator DisplayPattern()
    {
        isActive = false;
        foreach (int index in correctPattern)
        {
            nodes[index].GetComponent<Image>().DOColor(Color.yellow, 0.3f).SetLoops(2, LoopType.Yoyo);
            yield return new WaitForSeconds(0.6f);
        }
        yield return new WaitForSeconds(0.5f);
        isActive = true;
        timer = timeLimit;
    }
    
    private void Update()
    {
        if (!isActive) return;
        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            EndMinigame(false);
        }
    }
    
    public void OnNodePressed(int index)
    {
        if (!isActive) return;
        
        playerPattern.Add(index);
        
        if (playerPattern.Count == correctPattern.Count)
        {
            EndMinigame(CheckPattern());
        }
    }
    
    private bool CheckPattern()
    {
        for (int i = 0; i < correctPattern.Count; i++)
        {
            if (correctPattern[i] != playerPattern[i]) return false;
        }
        return true;
    }
    
    private void EndMinigame(bool success)
    {
        isActive = false;
        panel.SetActive(false);
        ClearGrid();
        onComplete?.Invoke(success);
    }

    private void ClearGrid()
    {
        foreach (var node in nodes.Values)
        {
            Destroy(node.gameObject);
        }
        nodes.Clear();
    }

}
