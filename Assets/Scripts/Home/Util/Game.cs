using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public Sprite Image;
    public string Name;
    public string Description;
    public int SceneNumber;
    public GamePanel GamePanel;
    public bool Multiplayer;
    public bool HasHighscores;

    void Start()
    {
        LayoutElement element = GetComponent<LayoutElement>();

        int size = (Screen.width - 30) / 3;
        if (size < 150)
            size = (Screen.width - 25) / 2;
        element.minHeight = element.minWidth = element.preferredHeight = element.preferredWidth = size;
    }
    public void OnButtonPressed()
    {
        PageSwicher.instance.EnableGamePage();
        GamePanel.LoadPanel(new DataStorage.GameInfo(Name, Description, Image, SceneNumber, Multiplayer, HasHighscores));

    }
}
