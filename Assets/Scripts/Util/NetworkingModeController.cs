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
    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            networkManager.StartServer();
            SceneManager.LoadScene("Server", LoadSceneMode.Additive);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            GameObject.Destroy(server);
            networkManager.StartClient();
            SceneManager.LoadScene("Login", LoadSceneMode.Additive);
        }
    }

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
