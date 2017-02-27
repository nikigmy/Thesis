using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ClientNetworkManager : NetworkManager {

    void Start()
    {
        NetworkServer.SpawnObjects();
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        //SendMessageToServer(1000, "10208627137535602");
        Debug.Log("conected(client)");
        base.OnClientConnect(conn);
    }
    public static void SendMessageToServer(short chanel, string message)
    {
        var msg = new StringMessage(message);
        singleton.client.Send(chanel, msg);
    }
}
