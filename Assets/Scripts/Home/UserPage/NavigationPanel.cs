using UnityEngine;
using System.Collections;

public class NavigationPanel : MonoBehaviour
{

    public GameObject HomePanel;
    public GameObject UserPanel;

    public void OnHomeCklicked()
    {
        HomePanel.SetActive(true);
        UserPanel.SetActive(false);
    }

    public void OnProfileClicked()
    {
        ContentFiller.instance.FillUserPage("me");
    }
}
