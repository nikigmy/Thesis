using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Friend : MonoBehaviour
{
    private string ID;
    public Text Username;
    public Image ProfilePicture;

    public void FillData(string name, string id, Sprite profilePicture)
    {
        ID = id;
        Username.text = name;
        ProfilePicture.sprite = profilePicture;
    }
}
