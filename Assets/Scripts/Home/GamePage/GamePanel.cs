using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private Main.GameInfo CurrentGame;
    public Text Name;
    public Image Image;
    public Text Description;
    public void LoadPanel(Main.GameInfo info)
    {
        CurrentGame = info;
        Name.text = info.Name;
        Image.sprite = info.Image;
        Description.text = info.Description;
    }

    public void PlayGame(int index)
    {
        SceneManager.LoadScene(CurrentGame.Index);
    }
}
