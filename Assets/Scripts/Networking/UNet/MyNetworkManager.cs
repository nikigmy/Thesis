using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


public class MyNetworkManager : NetworkManager
{
    public static NetworkConnection connectionToTheServer;
    public GameObject clientPrefab;
    public Client currentClient;

    public override void OnClientConnect(NetworkConnection conn)
    {
        connectionToTheServer = conn;
        NetworkServer.SpawnObjects();
        //NetworkServer.co
        //Client.singleton.RegisterHandlers();
        //MyNetworkManager.SendMessageToServer(1000, "10208627137535602");
        ClientSpawner.singleton.SpawnNewObject();
        Debug.Log("conected(client)");
        base.OnClientConnect(conn);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("client conected(server)");
        ClientScene.Ready(conn);
        Server.singleton.AddPlayer(conn.connectionId);
        base.OnServerConnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("disconected(server)");
        Server.singleton.OnPlayerDisconnected();
        NetworkServer.DestroyPlayersForConnection(conn);
      
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("Disconnected(client)");
    }


    public static void SendMessageToServer(short chanel, string message)
    {
        var msg = new StringMessage(message);
        singleton.client.Send(chanel, msg);
    }

    public static void SendMessageToClient(int connectionId, short chanel, string message)
    {
        NetworkServer.SendToClient(connectionId, chanel, new StringMessage(message));
    }

    void Update()
    {
        //if (client != null)
        //{
        //    if (currentClient.gameObject.activeSelf == false)
        //    {
        //        currentClient.gameObject.SetActive(true);
        //    }
        //}
    }
}
