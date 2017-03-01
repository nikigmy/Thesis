using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    public Text Username;
    public Image ProfilePicture;

    public void FillData(int score, Sprite profilePicture)
    {
        Username.text = score.ToString();
        ProfilePicture.sprite = profilePicture;
    }
}
