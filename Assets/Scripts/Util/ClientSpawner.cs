using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    public GameObject clientPrefab;
    public MyNetworkManager networkManager;
    public static ClientSpawner singleton;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        singleton = this;
    }

    public void SpawnNewObject()
    {
        Client client = Instantiate(clientPrefab).GetComponent<Client>();
        client.networkingManager = networkManager;
    }

    public void ResetObject(GameObject oldObject)
    {
        Destroy(oldObject);
        SpawnNewObject();
    }
}
