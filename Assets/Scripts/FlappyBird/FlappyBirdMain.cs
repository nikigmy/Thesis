using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlappyBirdMain : MonoBehaviour
{

    public GameObject BirdPrefab;
    public static FlappyBirdMain singleton;
    public bool GameOver;
    public int Score;
    private Spawner spawner;
    public GameObject CurrentBird;
    public Text PointsText;
    public Text GameOverText;
    public GameObject PlayButton;
	void Start ()
	{
	    spawner = GetComponent<Spawner>();
	    singleton = this;
	}

    public void OnGameOver()
    {
        PointsText.text = "";
        GameOver = true;
        GameOverText.text = "Game Over \n Points: " + Score;
        PlayButton.SetActive(true);
        spawner.StopSpawning();
        MyNetworkManager.SendMessageToServer(1006, Score.ToString());
    }

    public void OnScore()
    {
        Score++;
        PointsText.text = Score.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("HomeLoggedin");
        }
    }

    public void OnPlayClicked()
    {
        if (GameOver)
        {
            CleanupScene();
            CurrentBird = Instantiate(BirdPrefab);
            GameOverText.text = "";
        }
        Score = 0;
        PointsText.text = Score.ToString();
        PlayButton.SetActive(false);
        CurrentBird.GetComponent<Rigidbody2D>().simulated = true;
        GameOver = false;
        spawner.StartSpawning();
    }

    void CleanupScene()
    {
        Collumn[] cols = GameObject.FindObjectsOfType<Collumn>();
        foreach (var collumn in cols)
        {
            Destroy(collumn.gameObject);
        }
        Destroy(CurrentBird);
        GameOverText.text = "";
    }
}
