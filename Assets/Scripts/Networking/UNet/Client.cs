using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Client : NetworkBehaviour
{

    public static Client singleton;
    void Start ()
	{
        //NetworkClient.RegisterHandler(Server.GetName, GetName);
        singleton = this;
    }

    [Client]
    void GetName(NetworkMessage msg)
    {
        //SendMessageToServer(Server.GetName, "FacebookName");
        //throw new Exception();
    }

    public void SendMessageToServer(short chanel, string message)
    {

        var msg = new StringMessage(message);
        MyNetworkManager.singleton.client.Send(chanel, msg);
    }
}
