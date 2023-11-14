using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabRefTesterSpawner : MonoBehaviour
{
    public GameObject prefab;
    public PrefabRefChecker checker;

    // Start is called before the first frame update
    void Start()
    {
        GameObject instantiatedPrefab = Instantiate(prefab);
        checker.CheckReference(instantiatedPrefab);
    }
}
