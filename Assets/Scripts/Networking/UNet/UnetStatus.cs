using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

public class UnetStatus : NetworkBehaviour {

    const short login = 1000;
    NetworkClient _client;
    public SyncListString PlayerNames = new SyncListString();
    public override void OnStartClient()
    {
        PlayerNames.Callback = OnChatUpdated;
    }
    public void Start()
    {
        _client = NetworkManager.singleton.client;
        NetworkServer.RegisterHandler(login, Login);

        //var msg = new StringMessage(conn.address);
        //singleton.client.Send(1000, msg);
    }
    void Update()
    {
        StringBuilder str = new StringBuilder();
        foreach (var playerName in PlayerNames)
        {
            str.Append(playerName + ";");
        }

        Debug.Log(NetworkManager.singleton.numPlayers);
    }
    [Server]
    void Login(NetworkMessage netMsg)
    {
        string message = netMsg.ReadMessage<StringMessage>().value;
        PlayerNames.Add(message);
    }


    private void OnChatUpdated(SyncListString.Operation op, int index)
    {
        StringBuilder str = new StringBuilder();
        foreach (var playerName in PlayerNames)
        {
            str.Append(playerName + ";");
        }
    }
}
