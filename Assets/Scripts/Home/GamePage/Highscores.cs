using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Highscores : MonoBehaviour {

    public GameObject HighscoreContent;
    public GameObject HighscorePrefab;
    public List<Highscore> Highscores;

    public static Highscores singleton;

    void Start()
    {
        singleton = this;
    }

    public void ShowHighscores(Dictionary<Sprite, int> data)
    {
        if (Highscores.Count > data.Count)
        {
            for (int i = data.Count; i < Highscores.Count; i++)
            {
                Destroy(Highscores[i].gameObject);
            }
        }
        else if (Highscores.Count < data.Count)
        {
            for (int i = Highscores.Count; i < data.Count; i++)
            {
                GameObject highscore = Instantiate(HighscorePrefab, HighscoreContent.transform);
                Highscores.Add(highscore.GetComponent<Highscore>());
            }
        }

        int index = 0;
        foreach (var i in data)
        {
            Highscores[index].FillData(i.Value, i.Key);
            index++;
        }
    }
}
