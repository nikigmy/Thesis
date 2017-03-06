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
    const short inGame = 1005;
    const short flappyBird = 1006;
    const short endlessRunnerScore = 1007;

    public MyNetworkManager networkManager;
    public const short GetRecieveFriends = 48;
    public const short UpdateDescription = 49;
    private DatabaseLayer DatabaseLayer;
    public static Server singleton;
    private List<int> ConnectionIDs;

    internal class Game
    {
        public bool chosen;
        public int firstId;
        public int secondId;
    }

    private List<Game> currentGames;

    public void AddPlayer(int connectionId)
    {
        ConnectionIDs.Add(connectionId);
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
        NetworkServer.RegisterHandler(inGame, InGame);
        NetworkServer.RegisterHandler(flappyBird, FlappyBird);
        NetworkServer.RegisterHandler(endlessRunnerScore, EndlessRunner);
        DatabaseLayer = DatabaseLayer.GetInstance();
        currentGames = new List<Game>();
    }

    [Server]
    void FlappyBird(NetworkMessage netMsg)
    {
        string message = netMsg.ReadMessage<StringMessage>().value;
        StringBuilder returnMessage = new StringBuilder();
        if (message == "getscores")
        {
            Dictionary<string, int> fbIdsAndScores = DatabaseLayer.GetFlapyBirdScores(netMsg.conn.connectionId);
            if (fbIdsAndScores.Count == 0)
                return;
            foreach (var fbIdsAndScore in fbIdsAndScores)
            {
                returnMessage.Append(";" + fbIdsAndScore.Key + "," + fbIdsAndScore.Value);
            }
            returnMessage.Remove(0, 1);
            MyNetworkManager.SendMessageToClient(netMsg.conn.connectionId, flappyBird, returnMessage.ToString());
        }
        else
        {
            DatabaseLayer.SaveFlapyBirdScore(netMsg.conn.connectionId, int.Parse(message));
        }
    }

    [Server]
    void EndlessRunner(NetworkMessage netMsg)
    {
        string message = netMsg.ReadMessage<StringMessage>().value;
        if (message == "GetHighscore")
        {
            MyNetworkManager.SendMessageToClient(netMsg.conn.connectionId, endlessRunnerScore, DatabaseLayer.GetEndlessRunnerHighscore(netMsg.conn.connectionId).ToString());
        }
        else
        {
            DatabaseLayer.SaveEndlessRunnerScore(netMsg.conn.connectionId, int.Parse(message));
        }
    }

    [Server]
    void GameInvites(NetworkMessage netMsg)
    {
        string[] data = netMsg.ReadMessage<StringMessage>().value.Split(';');
        int invitedConnectionId = DatabaseLayer.GetConnectionID(data[1]);
        if (data[3] == "accept")
        {
            currentGames.Add(new Game() {firstId = netMsg.conn.connectionId, secondId = invitedConnectionId});
        }
        MyNetworkManager.SendMessageToClient(invitedConnectionId, gameInvite, data[0] + ";" + data[2] + ";" + data[3]);
    }
    [Server]
    void InGame(NetworkMessage netMsg)
    {
        string message = netMsg.ReadMessage<StringMessage>().value;
        int senderId = netMsg.conn.connectionId;
        int recieverId = 0;
        Game game =
            currentGames.First(x => x.firstId == senderId || x.secondId == senderId);
        recieverId = game.firstId == senderId ? game.secondId : game.firstId;
        if (message == "chose")
        {
            if (game.chosen)
                return;
            MyNetworkManager.SendMessageToClient(netMsg.conn.connectionId, inGame, "first");
            MyNetworkManager.SendMessageToClient(recieverId, inGame, "second");
            game.chosen = true;
        }
        else if (message == "over")
        {
            currentGames.Remove(game);
        }
        else
        {
            MyNetworkManager.SendMessageToClient(recieverId, inGame, message);
        }
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
            //register user
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
}
