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

    void Start ()
	{
	    singleton = this;
	}

    void Invite(string fbId)
    {
        string name = DataStorage.People.First(x => x.ID == fbId).Name;
        message.text = name + " invited you to play Tic-Tac-Toe";
        Debug.Log("FINISH THIS");
    }

    void OnAccept()
    {
        //MyNetworkManager.SendMessageToServer(1003, DataStorage.ThisUser.ID + ";" + ID + ";" + ";invite");
    }

    void OnDecline()
    {
        
    }
}
