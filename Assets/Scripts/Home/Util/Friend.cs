using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Friend : MonoBehaviour
{
    public Image StatusImage;
    public Sprite OnlineSprite;
    public Sprite OfflineSprite;
    private string ID;
    public Text Username;
    public Image ProfilePicture;
    private bool onlineStatus;
    public bool OnlineStatus
    {
        get { return onlineStatus; }
        set
        {
            StatusImage.sprite = value ? OnlineSprite : OfflineSprite;
            onlineStatus = value;
        }
    }

    public void FillData(string name, string id, Sprite profilePicture)
    {
        ID = id;
        Username.text = name;
        ProfilePicture.sprite = profilePicture;
        OnlineStatus = false;
    }

    public void FillData(DataStorage.Person friend)
    {
        ID = friend.ID;
        Username.text = friend.Name;
        ProfilePicture.sprite = friend.ProfilePicture;
        OnlineStatus = friend.OnlineStatus;
    }

    public void OnClick()
    {
        ContentFiller.instance.FillUserPage(ID);
        PageSwicher.instance.EnableUserPage();
    }

}
