using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Facebook.MiniJSON;
using Facebook.Unity;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ContentFiller : MonoBehaviour
{
    public GameObject clientPrefab;
    private static bool initialized = false;
    public static ContentFiller instance;
    public Text Username;
    public Image ProfilePicture;
    private Vector2 profilePictureSize;
    public RectTransform ProfilePictureRectTransform;
    public GameObject FriendsContainer;
    public GameObject FriendPrefab; //Recources.load
    public GameObject UserPage;
    void Start()
    {
        instance = this;
        if (!initialized)
        {
            FillUserData();
            FillFriendsData();
            initialized = true;
        }
        else
        {
            FillOldUserData();
        }
        ClientScene.Ready(MyNetworkManager.connectionToTheServer);
    }

    private void FillOldUserData()
    {
        Username.text = DataStorage.ThisUser.Name;
        ProfilePicture.sprite = DataStorage.ThisUser.ProfilePicture;

        foreach (var person in DataStorage.People.OrderByDescending(x => x.OnlineStatus))
        {
            GameObject a = (GameObject)Instantiate(FriendPrefab, FriendsContainer.transform);
            Friend friend = a.GetComponent<Friend>();
            LowerPanel.instance.Friends.Add(friend);
            Sprite profilePicture = new Sprite();
            friend.FillData(person);
        }
    }

    private void FillUserData()
    {
        profilePictureSize.x = ProfilePictureRectTransform.rect.width;
        profilePictureSize.y = ProfilePictureRectTransform.rect.height;
        FB.API("/me?fields=first_name,id", HttpMethod.GET, NameCallback);

    }

    private void NameCallback(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("Failed to get username");
            FB.API("/me?fields=first_name", HttpMethod.GET, NameCallback);
        }
        else
        {
            string userID = result.ResultDictionary["id"].ToString();
            string fbname = result.ResultDictionary["first_name"].ToString();
            Username.text = fbname;
            DataStorage.Person person = new DataStorage.Person(userID, fbname, new Sprite());
            FB.API(Util.GetPictureURL(userID, (int)profilePictureSize.x, (int)profilePictureSize.y), HttpMethod.GET,
                ProfilePictureCallback);
            DataStorage.ThisUser = person;
            MyNetworkManager.SendMessageToServer(1000, userID);
        }
    }
    private void ProfilePictureCallback(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("Didn't get profile picture." + profilePictureSize.x + " " + profilePictureSize.y);
            FB.API(Util.GetPictureURL("me", (int)profilePictureSize.x, (int)profilePictureSize.y), Facebook.Unity.HttpMethod.GET, ProfilePictureCallback);
        }
        else
        {
            Sprite picture = Sprite.Create(result.Texture, new Rect(0, 0, profilePictureSize.x, profilePictureSize.y), Vector2.zero);
            ProfilePicture.sprite = DataStorage.ThisUser.ProfilePicture = picture;
        }
    }

    private void FillFriendsData()
    {
        FB.API("/me?fields=friends.limit(100).fields(first_name,id)", HttpMethod.GET, FillFriendCallback);
    }

    private void FillFriendCallback(IResult result)
    {
        if (result.Error != null)
        {
            FB.API("/me?fields=id,first_name,friends.limit(100).fields(id)", HttpMethod.GET, FillFriendCallback);
            return;
        }

        var friends = Util.DeserializeJSONFriends(result.RawResult);
        foreach (var friend in friends)
        {
            FillFriend(friend as Dictionary<string, object>);
        }
        InitOthers();
    }

    private void FillFriend(Dictionary<string, object> friend)
    {
        string friendID = friend["id"].ToString();
        string friendName = friend["first_name"].ToString();
        FB.API(Util.GetPictureURL(friendID, (int)profilePictureSize.x, (int)profilePictureSize.y), HttpMethod.GET, (IGraphResult result) => FillFriendCallback(result, friendID, friendName));
    }

    private void FillFriendCallback(IGraphResult res, string id, string name)
    {
        if (res.Error != null)
        {
            Debug.Log("Didn't get friend profile picture." + profilePictureSize.x + " " + profilePictureSize.y);
            FB.API(Util.GetPictureURL(id, (int)profilePictureSize.x, (int)profilePictureSize.y), HttpMethod.GET, ((IGraphResult result) => FillFriendCallback(result, id, name)));
        }
        else
        {
            GameObject a = (GameObject)Instantiate(FriendPrefab, FriendsContainer.transform);
            Friend friend = a.GetComponent<Friend>();
            LowerPanel.instance.Friends.Add(friend);
            Sprite profilePicture = new Sprite();

            try
            {
                profilePicture = Sprite.Create(res.Texture, new Rect(0, 0, profilePictureSize.x, profilePictureSize.y), Vector2.zero);
            }
            catch (Exception) { }
            DataStorage.Person person = new DataStorage.Person(id, name, profilePicture);
            DataStorage.People.Add(person);
            DataStorage.UpdateStatus();
            friend.FillData(person);
        }
    }

    public void FillUserPage(string userID)
    {
        DataStorage.Person person;
        if (userID == "me")
        {
            person = DataStorage.ThisUser;
        }
        else
        {
            person = DataStorage.People.First(x => x.ID == userID);
        }
        UserPage.transform.GetChild(0).GetComponent<Image>().sprite = person.ProfilePicture;
        UserPage.transform.GetChild(1).GetComponent<Text>().text = person.Name;
    }

    private void InitOthers()
    {
        //Status.instance.StartUpdates();
    }
}