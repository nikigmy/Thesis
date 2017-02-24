using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.MiniJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Server : NetworkBehaviour
{
    public const short GetRecieveFriends = 48;
    public const short UpdateDescription = 49;
    private DatabaseLayer DatabaseLayer;
    public static Server singleton;
    private List<string> ConnectedIPs;

    public void AddPlayer(string IP)
    {
        //ConnectedIPs.Add(new Player{IpAdress = IP});
    }

    public void UpdatePlayer(string IP, string FacebookID, List<string> firends)
    {
        //Player player = ConnectedIPs.Find(x => x.IpAdress == IP);
    }

    public void OnPlayerDisconnected()
    {
        List<string> currentlyConnectedIPs = new List<string>();
        foreach (var connection in NetworkServer.connections)
        {
            if (connection != null)
            {
                currentlyConnectedIPs.Add(connection.address.Substring(7));
            }
        }
        for (int i = 0; i < this.ConnectedIPs.Count; i++)
        {
            if (!currentlyConnectedIPs.Contains(this.ConnectedIPs[i]))
            {
                DisconnectPlayer(this.ConnectedIPs[i]);
                this.ConnectedIPs.RemoveAt(i);
                i--;
            }
        }
    }

    void DisconnectPlayer(string ip)
    {
        DatabaseLayer.DisconectPlayer(ip);
        SendUpdateToFriends(ip, false);
    }

    /// <summary>
    /// Connect and Disconnect Updates
    /// </summary>
    /// <param name="player">Player to send the notification to</param>
    /// <param name="UpdatedPlayer">Player that connected/disconnected</param>
    /// <param name="connected">connceted/disconnected</param>
    private void SendUpdateToFriends(string adress, bool connected)
    {
        //TODO ImplementNotification
    }

    public void OnNewPlayerConnected()
    {
        //sendRequest
    }
    
    void Start()
    {
        NetworkServer.RegisterHandler(GetRecieveFriends, FillNewUserData);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
        //DatabaseLayer = DatabaseLayer.GetInstance();
    }

    [Server]
    void OnConnected(NetworkMessage netMsg)
    {
        string facebookID = netMsg.ReadMessage<StringMessage>().value;
        string ip = netMsg.conn.address;
        ConnectedIPs.Add(ip);
        DatabaseLayer.ConnectOrAddPlayer(facebookID ,ip);
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
