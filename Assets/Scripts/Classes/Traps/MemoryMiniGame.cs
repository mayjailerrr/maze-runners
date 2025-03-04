using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class MemoryMinigame : MonoBehaviour
{
    public GameObject panel;
    public List<Sprite> symbols;
    public Transform gridParent;
    public GameObject symbolPrefab;
    public float displayTime = 2f;

    private List<GameObject> symbolObjects = new List<GameObject>();
    private Dictionary<int, Sprite> assignedSymbols = new Dictionary<int, Sprite>();
    private GameObject firstSelected = null;
    private GameObject secondSelected = null;
    private bool isActive = false;
    private Action<bool> onComplete;
    
    public void StartMinigame(Action<bool> callback)
    {
        onComplete = callback;
        panel.SetActive(true);
        GenerateSymbols();
        StartCoroutine(HideSymbolsAfterDelay());
    }
    
    private void GenerateSymbols()
    {
        List<Sprite> pairs = new List<Sprite>();
        for (int i = 0; i < 4; i++)
        {
            pairs.Add(symbols[i]);
            pairs.Add(symbols[i]);
        }
        pairs = ShuffleList(pairs);
        
        for (int i = 0; i < pairs.Count; i++)
        {
            GameObject symbolGO = Instantiate(symbolPrefab, gridParent);
            Image img = symbolGO.GetComponent<Image>();
            img.sprite = pairs[i];
            assignedSymbols[i] = pairs[i];
            int index = i;
            symbolGO.GetComponent<Button>().onClick.AddListener(() => OnSymbolSelected(symbolGO, index));
            symbolObjects.Add(symbolGO);
        }
    }
    
    private IEnumerator HideSymbolsAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        foreach (var obj in symbolObjects)
        {
            obj.GetComponent<Image>().sprite = null;
        }
        isActive = true;
    }
    
    private void OnSymbolSelected(GameObject selected, int index)
    {
        if (!isActive || selected == firstSelected) return;
        
        selected.GetComponent<Image>().sprite = assignedSymbols[index];
        
        if (firstSelected == null)
        {
            firstSelected = selected;
        }
        else
        {
            secondSelected = selected;
            StartCoroutine(CheckMatch());
        }
    }
    
    private IEnumerator CheckMatch()
    {
        isActive = false;
        yield return new WaitForSeconds(0.5f);
        
        if (firstSelected.GetComponent<Image>().sprite == secondSelected.GetComponent<Image>().sprite)
        {
            firstSelected.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => Destroy(firstSelected));
            secondSelected.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => Destroy(secondSelected));
            symbolObjects.Remove(firstSelected);
            symbolObjects.Remove(secondSelected);
        }
        else
        {
            ClearRemainingSymbols();
            EndMinigame(false);
            yield break;
        }
        
        firstSelected = null;
        secondSelected = null;
        isActive = true;
        
        if (symbolObjects.Count == 0)
        {
            EndMinigame(true);
        }
    }
    
    private void ClearRemainingSymbols()
    {
        foreach (var obj in symbolObjects)
        {
            obj.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => Destroy(obj));
        }
        symbolObjects.Clear();
    }
    
    private void EndMinigame(bool success)
    {
        isActive = false;
        panel.SetActive(false);
        onComplete?.Invoke(success);
    }
    
    private List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }
}
