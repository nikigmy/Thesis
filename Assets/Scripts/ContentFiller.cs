using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Facebook.MiniJSON;
using Facebook.Unity;
using UnityEngine.UI;

public class ContentFiller : MonoBehaviour
{
    public Text Username;
    public Image ProfilePicture;
    private Vector2 profilePictureSize;
    public RectTransform ProfilePicturerectTransform;
    public GameObject FriendsContainer;
    public GameObject FRIENDPREFAB; //Recources.load
    // Use this for initialization
    void Start()
    {
        FillUserData();
        FillFriendsData();
    }

    private void FillUserData()
    {

        //profilePictureSize.x = ProfilePicture.transform.parent.GetComponent<RectTransform>().rect.width;
        //profilePictureSize.y = ProfilePicture.transform.parent.GetComponent<RectTransform>().rect.height;
        profilePictureSize.x = ProfilePicturerectTransform.rect.width;
        profilePictureSize.y = ProfilePicturerectTransform.rect.height;
        //profilePictureSize = new Vector2(1080 , 1080);
        FB.API(Util.GetPictureURL("me", (int)profilePictureSize.x, (int)profilePictureSize.y), HttpMethod.GET, ProfilePictureCallback);
        FB.API("/me?fields=first_name", HttpMethod.GET, NameCallback);

        Debug.Log("Didn't get profile picture." + (int)profilePictureSize.x + ".l." + (int)profilePictureSize.y);
    }

    void NameCallback(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("Failed to get username");
            FB.API("/me?fields=first_name", HttpMethod.GET, NameCallback);
        }
        else
        {
            string fbname = result.ResultDictionary["first_name"].ToString();
            Username.text = fbname;
        }
    }
    void ProfilePictureCallback(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("Didn't get profile picture." + profilePictureSize.x + " " + profilePictureSize.y);
            FB.API(Util.GetPictureURL("me", (int)profilePictureSize.x, (int)profilePictureSize.y), Facebook.Unity.HttpMethod.GET, ProfilePictureCallback);
        }
        else
        {
            ProfilePicture.sprite = Sprite.Create(result.Texture, new Rect(0, 0, profilePictureSize.x, profilePictureSize.y), Vector2.zero);
        }
    }

    private void FillFriendsData()
    {
        FB.API("/me?fields=friends.limit(100).fields(first_name,id)", HttpMethod.GET, APICallback);
    }

    void APICallback(IResult result)
    {
        if (result.Error != null)
        {
            FB.API("/me?fields=id,first_name,friends.limit(100).fields(id)", HttpMethod.GET, APICallback);
            return;
        }

        var friends = Util.DeserializeJSONFriends(result.RawResult);
        foreach (var friend in friends)
        {
            FillFriend(friend as Dictionary<string, object>);
        }
    }

    void FillFriend(Dictionary<string, object> friend)
    {   
        string friendID = friend["id"].ToString();
        string friendName = friend["first_name"].ToString();
        FB.API(Util.GetPictureURL(friendID, (int)profilePictureSize.x, (int)profilePictureSize.y), HttpMethod.GET, ((IGraphResult result) => FillFriendCallback(result, friendID, friendName)));
    }

    void FillFriendCallback(IGraphResult res, string id, string name)
    {
        if (res.Error != null)
        {
            Debug.Log("Didn't get friend profile picture." + profilePictureSize.x + " " + profilePictureSize.y);
            FB.API(Util.GetPictureURL(id, (int)profilePictureSize.x, (int)profilePictureSize.y), HttpMethod.GET, ((IGraphResult result) => FillFriendCallback(result, id, name)));
        }
        else
        {
            GameObject a = (GameObject)Instantiate(FRIENDPREFAB, FriendsContainer.transform);
            Friend friend = a.GetComponent<Friend>();
            Sprite profilePicture = Sprite.Create(res.Texture, new Rect(0, 0, profilePictureSize.x, profilePictureSize.y), Vector2.zero);
            friend.FillData(name, id, profilePicture);
        }
    }
}
