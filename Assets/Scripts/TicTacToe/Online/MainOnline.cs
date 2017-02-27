using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainOnline : MonoBehaviour
{
    public Main.Player thisPlayer = Main.Player.PlayerOne;

    public GameObject EndGamePanel;
    public Text EndGameText;

    private Main.Player turn;//Should be global
    public static MainOnline singleton;
    private CellOnline[] cells;
    // Use this for initialization
    void Start()
    {
        singleton = this;
        RectTransform rectTransform = GetComponent<RectTransform>();
        Debug.Log(rectTransform.rect.width);
        GridLayoutGroup layoutGroup = GetComponent<GridLayoutGroup>();
        layoutGroup.cellSize = new Vector2(rectTransform.rect.width / 3, rectTransform.rect.width / 3);
        cells = new CellOnline[9];
        for (int i = 0; i < transform.childCount; i++)
        {
            cells[i] = transform.GetChild(i).GetComponent<CellOnline>();
        }
        turn = Main.Player.PlayerOne;
    }

    public void PlaceTurn(int cellIndex)
    {
        if (thisPlayer != turn)
            return;
        if (cells[cellIndex].PlaceTurn(turn))
        {
            if (turn == thisPlayer)
            {
                foreach (var cell in cells)
                {
                    cell.Deactivate();
                }

                MyNetworkManager.SendMessageToServer(1005, cellIndex.ToString());
            }
            else
            {
                foreach (var cell in cells)
                {
                    cell.Activate();
                }
            }
            if (turn == Main.Player.PlayerOne)
            {
                turn = Main.Player.PlayerTwo;
            }
            else
            {
                turn = Main.Player.PlayerOne;
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
            if (cell.taken == Main.Player.None)
            {
                allTaken = false;
                break;
            }
        }

        if (allTaken)
        {
            EndGame(Main.Player.None);
        }
    }

    void CheckCells(int indexOne, int indexTwo, int indexThree)
    {
        if (cells[indexOne].taken == cells[indexTwo].taken && cells[indexOne].taken == cells[indexThree].taken && cells[indexOne].taken != Main.Player.None)
        {
            EndGame(cells[indexOne].taken);
        }
    }

    void EndGame(Main.Player winner)
    {
        if (winner == Main.Player.PlayerOne)
        {
            EndGameText.text = "Player one wins";
        }
        else if (winner == Main.Player.PlayerTwo)
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
