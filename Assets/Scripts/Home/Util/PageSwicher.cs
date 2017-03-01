using UnityEngine;
using System.Collections;
using System.Security.Policy;

public class PageSwicher : MonoBehaviour
{
    public static PageSwicher instance;
    public GameObject HomePage;
    public GameObject FriendPage;
    public GameObject GamePage;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePage.activeSelf)
            {
                EnableHomePage();
            }
        }
    }

    public void EnableHomePage()
    {
        HomePage.SetActive(true);
        FriendPage.SetActive(false);
        GamePage.SetActive(false);
    }

    public void EnableUserPage()
    {
        HomePage.SetActive(false);
        FriendPage.SetActive(true);
        GamePage.SetActive(false);
    }

    public void EnableGamePage()
    {
        HomePage.SetActive(false);
        FriendPage.SetActive(false);
        GamePage.SetActive(true);
    }
}
