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
    public bool Multiplayer;
    public bool HasHighscores;

    public void OnButtonPressed()
    {
        PageSwicher.instance.EnableGamePage();
        GamePanel.LoadPanel(new DataStorage.GameInfo(Name, Description, Image, SceneNumber, Multiplayer, HasHighscores));
    }
}
