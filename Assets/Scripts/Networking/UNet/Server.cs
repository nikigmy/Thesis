using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook.MiniJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Server : NetworkBehaviour
{

    const short connect = 1000;
    const short onlineStatusUpdate = 1001;
    const short offlineStatusUpdate = 1002;
    const short gameInvite = 1003;
    const short onlineFriends = 1004;
    private int shit = 0;
    public MyNetworkManager networkManager;
    public const short GetRecieveFriends = 48;
    public const short UpdateDescription = 49;
    private DatabaseLayer DatabaseLayer;
    public static Server singleton;
    private List<int> ConnectionIDs;

    public void AddPlayer(int connectionId)
    {
        ConnectionIDs.Add(connectionId);
        shit = connectionId;
    }
    public void UpdatePlayer(int connectionId, string FacebookId, List<string> firends)
    {
        //Player player = ConnectedIPs.Find(x => x.IpAdress == IP);
    }

    public void OnPlayerDisconnected()
    {
        List<int> currentConnectionIDs = new List<int>();
        foreach (var connection in NetworkServer.connections)
        {
            if (connection != null)
            {
                currentConnectionIDs.Add(connection.connectionId);
            }
        }
        for (int i = 0; i < this.ConnectionIDs.Count; i++)
        {
            if (!currentConnectionIDs.Contains(this.ConnectionIDs[i]))
            {
                DisconnectPlayer(this.ConnectionIDs[i]);
                this.ConnectionIDs.RemoveAt(i);
                i--;
            }
        }
    }

    void DisconnectPlayer(int connectionID)
    {
        Dictionary<string, List<int>> onlineFriendsConnectionAndFbIds = DatabaseLayer.GetOnlineFriendsIds(connectionID);
        SendUpdateToFriends(onlineFriendsConnectionAndFbIds.FirstOrDefault().Value, onlineFriendsConnectionAndFbIds.FirstOrDefault().Key, false);
        DatabaseLayer.DisconectPlayer(connectionID);
        //SendUpdateToFriends(connectionID, false);
    }

    /// <summary>
    /// Connect and Disconnect Updates
    /// </summary>
    /// <param name="player">Player to send the notification to</param>
    /// <param name="UpdatedPlayer">Player that connected/disconnected</param>
    /// <param name="connected">connceted/disconnected</param>
    private void SendUpdateToFriends(List<int> onlineFriendsConnectionIds, string updatedClientFbId, bool connected)
    {
        short chanel = connected ? onlineStatusUpdate : offlineStatusUpdate;
        if (onlineFriendsConnectionIds == null || updatedClientFbId == null)
            return;

        foreach (var onlineFriendsConnectionId in onlineFriendsConnectionIds)
        {
            MyNetworkManager.SendMessageToClient(onlineFriendsConnectionId, chanel, updatedClientFbId);
        }
    }

    public void OnNewPlayerConnected()
    {
        //sendRequest
    }

    void Awake()
    {
        singleton = this;
        ConnectionIDs = new List<int>();
        NetworkServer.RegisterHandler(GetRecieveFriends, FillNewUserData);
        NetworkServer.RegisterHandler(connect, OnConnected);
        NetworkServer.RegisterHandler(gameInvite, GameInvites);
        DatabaseLayer = DatabaseLayer.GetInstance();
        //networkManager.StartServer();
    }

    [Server]
    void GameInvites(NetworkMessage netMsg)
    {
        string[] data = netMsg.ReadMessage<StringMessage>().value.Split(';');
        int inviterConnectionId = netMsg.conn.connectionId;
        int invitedConnectionId = DatabaseLayer.GetConnectionID(data[1]);
        int gameId = int.Parse(data[2]);
        MyNetworkManager.SendMessageToClient(invitedConnectionId, gameInvite, data[0] + ";" + data[2] + ";" + data[3]);
    }

    [Server]
    void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("here");
        NetworkServer.SpawnObjects();
        string facebookID = netMsg.ReadMessage<StringMessage>().value;
        int connectionId = netMsg.conn.connectionId;
        ConnectionIDs.Add(connectionId);
        int result = DatabaseLayer.ConnectPlayer(facebookID, connectionId);
        if (result > 0)
        {
            Dictionary<string, List<int>> onlineFriendsConnectionAndFbIds = DatabaseLayer.GetOnlineFriendsIds(connectionId);
            SendUpdateToFriends(onlineFriendsConnectionAndFbIds.First().Value, onlineFriendsConnectionAndFbIds.First().Key, true);
            SendOnlineFriends(connectionId, DatabaseLayer.GetOnlineFriends(connectionId));
        }
        else
        {
        }
    }

    void SendOnlineFriends(int connectionId, List<string> fbFriendsIds)
    {
        if (fbFriendsIds == null || fbFriendsIds.Count == 0)
            return;
        StringBuilder friendIds = new StringBuilder();
        friendIds.Append(fbFriendsIds[0]);
        for (int i = 1; i < fbFriendsIds.Count; i++)
        {
            friendIds.Append(";" + fbFriendsIds[i]);
        }
        MyNetworkManager.SendMessageToClient(connectionId, onlineFriends, friendIds.ToString());
    }

    [Server]
    void FillNewUserData(NetworkMessage message)
    {
        //var data = (Dictionary<>)Json.Deserialize(message);

    }
    // Update is called once per frame
    void Update()
    {
    }
}
