using System;
using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    public Image ProfilePicture;
    private void SetInit()
    {
        Debug.Log("fb init done");

        if (FB.IsLoggedIn)
        {
            Debug.Log("Logged in");
        }
        else
        {
            FBLogin();
        }
    }
    
    private void OnHideUnity(bool isGameShown)
    {
        if(!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void FBLogin()
    {
        var perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Worked");
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            FB.API(Util.GetPictureURL("me", 128, 128), Facebook.Unity.HttpMethod.GET, ProfilePictureCallback);
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    void ProfilePictureCallback(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("Didn't get profile picture.");
            FB.API(Util.GetPictureURL("me", 128, 128), Facebook.Unity.HttpMethod.GET, ProfilePictureCallback);
        }
        else
        {
            ProfilePicture.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), Vector2.zero);
        }
    }
}