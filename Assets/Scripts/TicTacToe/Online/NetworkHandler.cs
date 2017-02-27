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
        MyNetworkManager.SendMessageToServer(inGame, "chose");
    }

    public void RegisterHandlers()
    {
        Debug.Log(NetworkClient.allClients.Count);
        NetworkManager.client.RegisterHandler(inGame, InGame);
    }

    [Client]
    void InGame(NetworkMessage msg)
    {
        string message = msg.ReadMessage<StringMessage>().value;
        if (message == "first")
        {
            main.thisPlayer  = Main.Player.PlayerOne;
        }
        else if (message == "second")
        {
            main.thisPlayer = Main.Player.PlayerTwo;
        }
        int turn = int.Parse(message);
        main.PlaceTurn(turn);
    }
}
