using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class NetworkingModeController : MonoBehaviour
{
    public MyNetworkManager networkManager;
    public GameObject client;
    public GameObject server;
    //	void Start () {
    //#if UNITY_EDITOR
    //        {
    //            SceneManager.LoadScene("Server");
    //        }
    //#else
    //        {
    //            SceneManager.LoadScene("Login");
    //        }
    //#endif
    //    }

    public void OnClientClicked()
    {
        GameObject.Destroy(server);
        networkManager.StartClient();
        SceneManager.LoadScene("Login", LoadSceneMode.Additive);
    }
    public void OnServerClicked()
    {
        networkManager.StartServer();
        SceneManager.LoadScene("Server", LoadSceneMode.Additive);
    }
}
