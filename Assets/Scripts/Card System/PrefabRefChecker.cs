using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabRefChecker : MonoBehaviour
{
    public GameObject prefabReference;

    // Start is called before the first frame update
    public void CheckReference(GameObject instantiatedPrefab)
    {
        if (prefabReference == instantiatedPrefab) print("prefab ref == instantiated prefab");
        else print("prefab ref != instantiated prefab");
    }
}
