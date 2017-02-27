using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour {

    public GameObject clientPrefab;

    void Start()
    {
        Instantiate(clientPrefab);
    }
}
