using System;
using Facebook.Unity;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginAndAuthentication : MonoBehaviour {

    void Awake()
    {
        ClientScene.Ready(MyNetworkManager.connectionToTheServer);
        FB.Init(SetInit);
        DataStorage.People = new List<DataStorage.Person>();
        DataStorage.Games = new List<DataStorage.Game>();
        DataStorage.OnlineFriends = new List<string>();
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
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Facebook.Unity.AccessToken.CurrentAccessToken =
                new AccessToken(
                    "EAAGVo1lrQ7kBAAa9ZBmcAXP0toBj1BCO88EhrF6t2WV4akrjFUpU0lkRGreraP3NtHCpmI5fRSg36SkqaPAN0zHtwXb4avlBjQw1J6FZBoJyX2QGwI3KRoiqDVbZA90ILEPOCajCocezvAkl718AfvgJDfViRYZD",
                    "10208627137535602",
                    new DateTime(2017, 4, 21),
                    new List<string> {"user_friends", "email", "public_profile"},
                    null);
            LoadHomeScene();
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            FBLogin();
        }
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
        SceneManager.LoadScene("HomeLoggedin", LoadSceneMode.Additive);
        //GlobalVariables.CurrentAccessToken = Facebook.Unity.AccessToken.CurrentAccessToken;
    }
}
