using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendForInvites : MonoBehaviour
{

    private string ID;
    public Text Username;
    public Image ProfilePicture;

    public void FillData(string name, string id, Sprite profilePicture)
    {
        ID = id;
        Username.text = name;
        ProfilePicture.sprite = profilePicture;
    }

    public void FillData(DataStorage.Person friend)
    {
        ID = friend.ID;
        Username.text = friend.Name;
        ProfilePicture.sprite = friend.ProfilePicture;
    }

    public void OnClick()
    {
        MyNetworkManager.SendMessageToServer(1003, DataStorage.ThisUser.ID + ";" + ID + ";" + GamePanel.GameID + ";invite");
        PlayOnline.singleton.ShowInvitedPanel(ID);
    }
}
