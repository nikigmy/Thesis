using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class Client : NetworkBehaviour
{
    public Text text;
    const short connect = 1000;
    const short onlineStatusUpdate = 1001;
    const short offlineStatusUpdate = 1002;
    const short gameInvite = 1003;
    const short onlineFriends = 1004;
    public NetworkIdentity a;
    public MyNetworkManager networkingManager;
    public static Client singleton;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        singleton = this;
        networkingManager = ClientSpawner.singleton.networkManager;
        RegisterHandlers();
    }

    private void OnDisable()
    {
        ClientSpawner.singleton.ResetObject(gameObject);
    }

    public void RegisterHandlers()
    {
        Debug.Log(NetworkClient.allClients.Count);
        networkingManager.client.RegisterHandler(1000, Test);
        networkingManager.client.RegisterHandler(onlineStatusUpdate, FriendLoggedIn);
        networkingManager.client.RegisterHandler(onlineStatusUpdate, FriendLoggedIn);
        networkingManager.client.RegisterHandler(offlineStatusUpdate, FriendDisconected);
        networkingManager.client.RegisterHandler(gameInvite, GameInvite);
        networkingManager.client.RegisterHandler(onlineFriends, OnlineFriends);
    }

    [Client]
    void Test(NetworkMessage msg)
    {
        string facebookID = msg.ReadMessage<StringMessage>().value;
        Debug.Log(facebookID);
        Debug.Log("sad");
    }

    [Client]
    public void FriendLoggedIn(NetworkMessage msg)
    {
        string facebookID = msg.ReadMessage<StringMessage>().value;
        DataStorage.OnlineFriends.Add(facebookID);
        DataStorage.UpdateStatus();
        LowerPanel.instance.UpdateStatus();
    }

    [Client]
    void FriendDisconected(NetworkMessage msg)
    {
        string facebookID = msg.ReadMessage<StringMessage>().value;
        DataStorage.OnlineFriends.Remove(facebookID);
        DataStorage.UpdateStatus();
        LowerPanel.instance.UpdateStatus();
    }

    [Client]
    void GameInvite(NetworkMessage msg)
    {
        string[] gameInviteData = msg.ReadMessage<StringMessage>().value.Split(';');
        string friendFbId = gameInviteData[0];
        int gameID = int.Parse(gameInviteData[1]);
        Debug.Log("FINISH THIS");

    }

    public void InviteFriend(string cliendFbId, string friendFbId, int gameIndex)
    {
        MyNetworkManager.SendMessageToServer(gameInvite, cliendFbId + ";" + friendFbId + ";" + gameIndex);
    }

    [Client]
    void OnlineFriends(NetworkMessage msg)
    {
        string[] friendIds = msg.ReadMessage<StringMessage>().value.Split(';');
        foreach (var friendId in friendIds)
        {
            DataStorage.OnlineFriends.Add(friendId);
        }
        DataStorage.UpdateStatus();
        LowerPanel.instance.UpdateStatus();
    }
}
