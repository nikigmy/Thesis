using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndlessRunnerMain : MonoBehaviour
{

    public static EndlessRunnerMain singleton;
    public bool GameOver;
    public int Points;
    public Text HighscoreText;
    public Text ScoreText;
    public Button PlayButton;
    private Coroutine scoreCoroutine;
    public int Highscore;

    void Start ()
	{
	    singleton = this;
        GameOver = true;
        //MyNetworkManager.SendMessageToServer(1007, "GetHighscore");
	}

    public void EndGame()
    {
        StopCoroutine(scoreCoroutine);
        GameOver = true;
        ObstacleSpawner.singleton.StopSpawning();
        //MyNetworkManager.SendMessageToServer(1007, Points.ToString());
    }

    IEnumerator Score()
    {
        while (true)
        {
            ScoreText.text = "Points: " + Points;
            Points++;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StartGame()
    {
        Points = 0;
        UnityChan.singleton.PlayAnimations();
        PlayButton.gameObject.SetActive(false);
        HighscoreText.gameObject.SetActive(false);
        ScoreText.gameObject.SetActive(true);
        scoreCoroutine = StartCoroutine(Score());
        ObstacleSpawner.singleton.StartSpawning();
        GameOver = false;
    }

   public  void OnPlayClicked()
    {
        StartGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("HomeLoggedin");
        }
    }
}
