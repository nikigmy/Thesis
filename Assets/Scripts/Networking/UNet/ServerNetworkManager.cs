using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ServerNetworkManager : NetworkManager {

    void Start()
    {
        NetworkServer.SpawnObjects();
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("client conected(server)");
        Server.singleton.AddPlayer(conn.connectionId);
        base.OnServerConnect(conn);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("disconected(server)");
        Server.singleton.OnPlayerDisconnected();
        NetworkServer.DestroyPlayersForConnection(conn);
    }
    public static void SendMessageToClient(int connectionId, short chanel, string message)
    {
        NetworkServer.SendToClient(connectionId, chanel, new StringMessage(message));
    }
}
