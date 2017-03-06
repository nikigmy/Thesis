using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanColider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (!EndlessRunnerMain.singleton.GameOver)
        {
            UnityChan.singleton.Die();
            EndlessRunnerMain.singleton.EndGame();
        }
    }
}
