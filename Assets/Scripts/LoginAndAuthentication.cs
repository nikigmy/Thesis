using System;
using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginAndAuthentication : MonoBehaviour {

    void Awake()
    {
        FB.Init(SetInit);
    }

    private void SetInit()
    {
        Debug.Log("FB init done.");

        if (FB.IsLoggedIn)
        {
            Debug.Log("User was logged in");
            LoadHomeScene();
        }
    }

    public void OnFBLoginClicked()
    {
        FBLogin();
    }

    public void OnOfflineClicked()
    {
        SceneManager.LoadScene("HomeOffline");
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
            Debug.Log("Succesfully logged in");
            LoadHomeScene();
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    private void LoadHomeScene()
    {
        SceneManager.LoadScene("HomeLoggedin");
        GlobalVariables.CurrentAccessToken = Facebook.Unity.AccessToken.CurrentAccessToken;
    }
}
