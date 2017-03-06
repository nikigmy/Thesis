using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Client : NetworkBehaviour
{
    public Text text;
    const short connect = 1000;
    const short onlineStatusUpdate = 1001;
    const short offlineStatusUpdate = 1002;
    const short gameInvite = 1003;
    const short onlineFriends = 1004;
    const short inGame = 1005;
    const short flappyBird = 1006;
    const short endlessRunnerScore = 1007;

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

    private void OnEnable()
    {
        RegisterHandlers();
    }

    private void OnDisable()
    {
        ClientSpawner.singleton.ResetObject(gameObject);
    }

    public void RegisterHandlers()
    {
        Debug.Log(NetworkClient.allClients.Count);
        networkingManager.client.RegisterHandler(onlineStatusUpdate, FriendLoggedIn);
        networkingManager.client.RegisterHandler(onlineStatusUpdate, FriendLoggedIn);
        networkingManager.client.RegisterHandler(offlineStatusUpdate, FriendDisconected);
        networkingManager.client.RegisterHandler(gameInvite, GameInvite);
        networkingManager.client.RegisterHandler(onlineFriends, OnlineFriends);
        networkingManager.client.RegisterHandler(inGame, InGame);
        networkingManager.client.RegisterHandler(flappyBird, FlappyBirdScores);
        networkingManager.client.RegisterHandler(endlessRunnerScore, EndlessRunnerHighscore);
    }

    [Client]
    void EndlessRunnerHighscore(NetworkMessage msg)
    {
        EndlessRunnerMain.singleton.Highscore = int.Parse(msg.ReadMessage<StringMessage>().value);
    }

    [Client]
    void FriendLoggedIn(NetworkMessage msg)
    {
        string facebookID = msg.ReadMessage<StringMessage>().value;
        DataStorage.OnlineFriends.Add(facebookID);
        DataStorage.UpdateStatus();
        LowerPanel.instance.UpdateStatus();
    }

    [Client]
    void FlappyBirdScores(NetworkMessage msg)
    {
        Dictionary<Sprite, int> fbPictureAndScores = new Dictionary<Sprite, int>();
        string[] fbIdsAndScores = msg.ReadMessage<StringMessage>().value.Split(';');
        foreach (var fbIdsAndScore in fbIdsAndScores)
        {
            string[] fbIdAndScore = fbIdsAndScore.Split(',');
            Sprite fbPicture = DataStorage.People.First(x => x.ID == fbIdAndScore[0]).ProfilePicture;
            int score = int.Parse(fbIdAndScore[1]);
            fbPictureAndScores.Add(fbPicture, score);
        }
        Highscores.singleton.ShowHighscores(fbPictureAndScores);
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
        switch (gameInviteData[2])
        {
            case "invite":
                {
                    GameInvites.singleton.Invite(friendFbId, gameID);
                    break;
                }
            case "accept":
                {
                    SceneManager.LoadScene(6, LoadSceneMode.Additive);
                    Debug.Log("FINISH THIS");
                    break;
                }
            case "decline":
                {
                    PlayOnline.singleton.OnDecline();
                    break;
                }
            case "abort":
                {
                    GameInvites.singleton.Abort();
                    break;
                }
        }
        Debug.Log("FINISH THIS");

    }
    [Client]
    void InGame(NetworkMessage msg)
    {
        string message = msg.ReadMessage<StringMessage>().value;
        Debug.Log(message);
        if (message == "first")
        {
            MainOnline.singleton.thisPlayer = Main.Player.PlayerOne;
            MainOnline.singleton.EnableAll();

        }
        else if (message == "second")
        {
            MainOnline.singleton.thisPlayer = Main.Player.PlayerTwo;
            MainOnline.singleton.DisableAll();
        }
        else
        {
            int turn = int.Parse(message);
            MainOnline.singleton.PlaceTurn(turn);
        }
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
