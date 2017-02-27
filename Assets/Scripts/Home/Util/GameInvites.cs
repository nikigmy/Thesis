using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameInvites : MonoBehaviour
{
    public GameObject panel;
    public Text message;
    public static GameInvites singleton;
    private int currentGameId;
    private string currentInviterFbId;

    void Start()
    {
        singleton = this;
    }

    public void Invite(string fbId, int gameId)
    {
        panel.SetActive(true);
        currentInviterFbId = fbId;
        currentGameId = gameId;
        string name = DataStorage.People.First(x => x.ID == fbId).Name;
        message.text = name + " invited you to play Tic-Tac-Toe";
        Debug.Log("FINISH THIS");
    }

    public void Abort()
    {
        panel.SetActive(false);
    }

    public void OnAccept()
    {
        MyNetworkManager.SendMessageToServer(1003, DataStorage.ThisUser.ID + ";" + currentInviterFbId + ";" + currentGameId + ";accept");
        panel.SetActive(false);
    }

    public void OnDecline()
    {
        MyNetworkManager.SendMessageToServer(1003, DataStorage.ThisUser.ID + ";" + currentInviterFbId + ";" + currentGameId + ";decline");
        panel.SetActive(false);
    }
}
