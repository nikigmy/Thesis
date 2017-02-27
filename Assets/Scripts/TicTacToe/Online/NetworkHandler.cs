using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkHandler : NetworkBehaviour
{
    private bool myturn = false;
    const short inGame = 1005;
    private MyNetworkManager NetworkManager;
    private MainOnline main;

	void Start () {
        main = FindObjectsOfType<MainOnline>()[0];
        NetworkManager = ClientSpawner.singleton.networkManager;
    }

    public void RegisterHandlers()
    {
        Debug.Log(NetworkClient.allClients.Count);
        NetworkManager.client.RegisterHandler(inGame, InGame);
    }

    [Client]
    void InGame(NetworkMessage msg)
    {
        int turn = int.Parse(msg.ReadMessage<StringMessage>().value);
        main.PlaceTurn(turn);
    }
}
