using UnityEngine;
using System.Collections;

public class PannelSwitcher : MonoBehaviour
{

    public GameObject FriendsPanel;
    public GameObject GamesPanel;

   public void OnFriendsClicked()
    {
        Debug.Log("friends");
        GamesPanel.SetActive(false);
        FriendsPanel.SetActive(true);
    }

    public void OnGamesClicked()
    {
        Debug.Log("games");
        GamesPanel.SetActive(true);
        FriendsPanel.SetActive(false);
    }
}
