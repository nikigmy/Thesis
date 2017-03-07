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

    void Start()
    {
        LayoutElement element = GetComponent<LayoutElement>();
        float height = (transform.parent.parent.GetComponent<RectTransform>().rect.height - 20)/4;
        if (height < 150)
            height = 150;
        if (height > 300)
            height = 300;
        element.minHeight = element.preferredHeight = height;
    }

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
