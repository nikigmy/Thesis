using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    public Sprite Image;
    public string Name;
    public string Description;
    public int SceneNumber;
    public GamePanel GamePanel;

    public void OnButtonPressed()
    {
        PageSwicher.instance.EnableGamePage();
        GamePanel.LoadPanel(new Main.GameInfo(Name, Description, Image, SceneNumber));     
    }
}
