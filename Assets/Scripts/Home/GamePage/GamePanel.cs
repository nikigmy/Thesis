using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private DataStorage.GameInfo CurrentGame;
    public static int GameID;
    public Text Name;
    public Image Image;
    public Text Description;
    public GameObject PlayButton;
    public GameObject PlayOnlineButton;
    public GameObject HighScores;
    public GameObject DescriptionObject;
    public void LoadPanel(DataStorage.GameInfo info)
    {
        CurrentGame = info;
        Name.text = info.Name;
        Image.sprite = info.Image;
        Description.text = info.Description;
        GameID = info.Index;
        RectTransform playButtonRectTransform = PlayButton.GetComponent<RectTransform>();
        PlayOnlineButton.SetActive(info.Multiplayer);

        float leftAnchor;
        float rightAnchorl;

        if (info.Multiplayer)
        {
            leftAnchor = 0.05f;
            rightAnchorl = 0.45f;
        }
        else
        {
            leftAnchor = 0.3f;
            rightAnchorl = 0.7f;
        }

        playButtonRectTransform.anchorMin = new Vector2(leftAnchor, playButtonRectTransform.anchorMin.y);
        playButtonRectTransform.anchorMax = new Vector2(rightAnchorl, playButtonRectTransform.anchorMax.y);
        playButtonRectTransform.offsetMin = playButtonRectTransform.offsetMax = Vector2.zero;
        if (info.HasHighscores)
        {
            DescriptionObject.SetActive(false);
            HighScores.SetActive(true);
            MyNetworkManager.SendMessageToServer(1006, "getscores");
        }
    }

    public void PlayGame(int index)
    {
        SceneManager.LoadScene(CurrentGame.Index);
    }
}
