using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Highscores : MonoBehaviour {

    public GameObject HighscoreContent;
    public GameObject HighscorePrefab;
    public List<Highscore> HighscoreObjects;

    public static Highscores singleton;

    void Start()
    {
        singleton = this;
    }

    public void ShowHighscores(Dictionary<Sprite, int> data)
    {
        if (HighscoreObjects.Count > data.Count)
        {
            for (int i = data.Count; i < HighscoreObjects.Count; i++)
            {
                Destroy(HighscoreObjects[i].gameObject);
            }
        }
        else if (HighscoreObjects.Count < data.Count)
        {
            for (int i = HighscoreObjects.Count; i < data.Count; i++)
            {
                GameObject highscore = Instantiate(HighscorePrefab, HighscoreContent.transform);
                HighscoreObjects.Add(highscore.GetComponent<Highscore>());
            }
        }

        int index = 0;
        foreach (var i in data)
        {
            HighscoreObjects[index].FillData(i.Value, i.Key);
            index++;
        }
    }
}
