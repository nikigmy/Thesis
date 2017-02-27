using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayOnline : MonoBehaviour
{
    public GameObject FriendContent;
    public GameObject friendPrefab;
    public List<FriendForInvites> friends;

    public static PlayOnline singleton;
    private string currentInvited;
    private int currentGameId;
    public GameObject invitePanel;
    public Text message;

    void Start()
    {
        singleton = this;
    }

    public void ShowFriends()
    {
        DataStorage.Person[] onlineFriends = DataStorage.People.Where(x => x.OnlineStatus).ToArray();

        if (friends.Count > onlineFriends.Length)
        {
            for (int i = onlineFriends.Length; i < friends.Count; i++)
            {
                Destroy(friends[i].gameObject);
            }
        }
        else if (friends.Count < onlineFriends.Length)
        {
            for (int i = friends.Count; i < onlineFriends.Length; i++)
            {
                GameObject friend = Instantiate(friendPrefab, FriendContent.transform);
                friends.Add(friend.GetComponent<FriendForInvites>());
            }
        }

        for (int i = 0; i < onlineFriends.Length; i++)
        {
            friends[i].FillData(onlineFriends[i]);
        }
    }

    public void ShowInvitedPanel(string fbId)
    {
        invitePanel.SetActive(true);
        currentInvited = fbId;
        message.text = "You have invited " + DataStorage.People.First(x => x.ID == fbId).Name + " to play Tic-Tac-Toe.";
        Debug.Log("FINISH THIS");
    }

    public void OnAbort()
    {
        MyNetworkManager.SendMessageToServer(1003, DataStorage.ThisUser.ID + ";" + currentInvited + ";" + GamePanel.GameID + ";abort");
        invitePanel.SetActive(false);
    }

    public void OnDecline()
    {
        message.text = DataStorage.People.First(x => x.ID == currentInvited).Name + "have declined your request.";
    }
}
