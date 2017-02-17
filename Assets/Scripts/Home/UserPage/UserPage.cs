using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserPage : MonoBehaviour {

    public Image Picture;
    public Text Name;

    public void FillUserPage(Sprite picture, string name)
    {
        Picture.sprite = picture;
        Name.text = name;
    }

    public void OnHomeClicked()
    {
        PageSwicher.instance.EnableHomePage();
    }

    public void OnProfileClicked()
    {
        FillUserPage(new Sprite(), "");
    }
}
