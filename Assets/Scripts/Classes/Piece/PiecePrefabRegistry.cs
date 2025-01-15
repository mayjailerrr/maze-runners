using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PiecePrefabRegistry", menuName = "Game/PiecePrefabRegistry")]
public class PiecePrefabRegistry : ScriptableObject
{
    public List<PiecePrefabMapping> piecePrefabs;

    private Dictionary<string, GameObject> prefabDictionary;

    private void OnEnable()
    {
        prefabDictionary = new Dictionary<string, GameObject>();
        foreach (var mapping in piecePrefabs)
        {
            prefabDictionary[mapping.pieceName] = mapping.prefab;
        }
    }

    public GameObject GetPrefab(string pieceName)
    {
        if (prefabDictionary.TryGetValue(pieceName, out var prefab))
        {
            return prefab;
        }

        Debug.LogError($"Prefab not found for piece: {pieceName}");
        return null;
    }
}

[System.Serializable]
public class PiecePrefabMapping
{
    public string pieceName;
    public GameObject prefab;
}
