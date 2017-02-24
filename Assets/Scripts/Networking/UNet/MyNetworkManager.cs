using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


public class MyNetworkManager : NetworkManager
{
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("conected(client)");
        base.OnClientConnect(conn);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("client conected(server)");
        Server.singleton.AddPlayer(conn.address);
        base.OnServerConnect(conn);
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        Debug.Log("ERROR(server)");
        base.OnServerError(conn, errorCode);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("disconected(server)");
        Server.singleton.OnPlayerDisconnected();
        NetworkServer.DestroyPlayersForConnection(conn);
    }
}
