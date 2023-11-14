using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabRefTester : MonoBehaviour
{
    public GameObject prefabReference;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject == prefabReference) print("true");
        else print("false");
    }
}
