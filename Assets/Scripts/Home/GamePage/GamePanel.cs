using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private Main.GameInfo CurrentGame;
    public static int GameID;
    public Text Name;
    public Image Image;
    public Text Description;
    public void LoadPanel(Main.GameInfo info)
    {
        CurrentGame = info;
        Name.text = info.Name;
        Image.sprite = info.Image;
        Description.text = info.Description;
        GameID = info.Index;
    }

    public void PlayGame(int index)
    {
        SceneManager.LoadScene(CurrentGame.Index);
    }
}
