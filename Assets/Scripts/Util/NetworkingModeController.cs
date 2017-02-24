using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkingModeController : MonoBehaviour
{

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
        SceneManager.LoadScene("Login");
    }
    public void OnServerClicked()
    {
        SceneManager.LoadScene("Server");
    }
}
