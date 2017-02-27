using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    public class GameInfo
    {
        public string Name;
        public string Description;
        public Sprite Image;
        public int Index;

        public GameInfo(string name, string description, Sprite image, int index)
        {
            Name = name;
            Description = description;
            Image = image;
            Index = index;
        }
    }
    Player thisPlayer = Player.PlayerOne;
    public enum Player
    {
        PlayerOne, PlayerTwo, None
    }

    public GameObject EndGamePanel;
    public Text EndGameText;

    private Player turn;//Should be global

    private Cell[] cells;
    // Use this for initialization
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Debug.Log(rectTransform.rect.width);
        GridLayoutGroup layoutGroup = GetComponent<GridLayoutGroup>();
        layoutGroup.cellSize = new Vector2(rectTransform.rect.width / 3, rectTransform.rect.width / 3);
        cells = new Cell[9];
        for (int i = 0; i < transform.childCount; i++)
        {
            cells[i] = transform.GetChild(i).GetComponent<Cell>();
        }
        turn = Player.PlayerOne;
    }

    public void PlaceTurn(int cellIndex)
    {
        if (thisPlayer != turn)
            return;
        if (cells[cellIndex].PlaceTurn(turn))
        {
            if (turn == Player.PlayerOne)
            {
                turn = Player.PlayerTwo;
                thisPlayer = Player.PlayerTwo;
            }
            else
            {
                turn = Player.PlayerOne;
                thisPlayer = Player.PlayerOne;
            }
            CheckForWin();
        }
    }

    void CheckForWin()
    {
        //stupid way
        CheckCells(0, 1, 2);
        CheckCells(3, 4, 5);
        CheckCells(6, 7, 8);

        CheckCells(0, 3, 6);
        CheckCells(1, 4, 7);
        CheckCells(2, 5, 8);

        CheckCells(0, 4, 8);
        CheckCells(2, 4, 6);

        bool allTaken = true;
        foreach (var cell in cells)
        {
            if (cell.taken == Player.None)
            {
                allTaken = false;
                break;
            }
        }

        if (allTaken)
        {
            EndGame(Player.None);
        }
    }

    void CheckCells(int indexOne, int indexTwo, int indexThree)
    {
        if (cells[indexOne].taken == cells[indexTwo].taken && cells[indexOne].taken == cells[indexThree].taken && cells[indexOne].taken != Player.None)
        {
            EndGame(cells[indexOne].taken);
        }
    }

    void EndGame(Player winner)
    {
        if (winner == Player.PlayerOne)
        {
            EndGameText.text = "Player one wins";
        }
        else if (winner == Player.PlayerTwo)
        {
            EndGameText.text = "Player two wins";
        }
        else
        {
            EndGameText.text = "Draw";
        }
        EndGamePanel.SetActive(true);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
