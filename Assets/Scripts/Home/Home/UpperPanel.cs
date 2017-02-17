using UnityEngine;
using System.Collections;

public class UpperPanel : MonoBehaviour {
    
    public void OnClick()
    {
        ContentFiller.instance.FillUserPage("me");
        PageSwicher.instance.EnableUserPage();
    }
}
