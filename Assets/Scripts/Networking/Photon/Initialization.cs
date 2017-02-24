using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Initialization : MonoBehaviour
{

    void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;

        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
        {
            PhotonNetwork.ConnectUsingSettings("1");
            PhotonNetwork.JoinLobby();
            Debug.Log("Here");
        }
    }

    void OnJoinedLobby()
    {
        PhotonNetwork.playerName = PhotonNetwork.player.NickName = DataStorage.ThisUser.ID;
        RoomOptions a = new RoomOptions();
        a.IsOpen = a.IsVisible = true;
        a.MaxPlayers = 0;
        PhotonNetwork.JoinOrCreateRoom("Home", a, TypedLobby.Default);
    }

    void Update()
    {
        Debug.Log(PhotonNetwork.countOfPlayers);
    }
}
