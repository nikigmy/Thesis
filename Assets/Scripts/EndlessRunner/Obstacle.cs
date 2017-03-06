using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    public enum ObstacleType
    {
        Slide, Jump, Block
    }

    public ObstacleType Type;

    public int TakenLanes;
    public bool Blocker;

    public float MovementMultiplyer = 0.5f;
	void Update () {
	    if (!EndlessRunnerMain.singleton.GameOver)
	    {
	        transform.Translate(-Vector3.forward*MovementMultiplyer, Space.World);
	        if (transform.position.z <= -10)
	            Destroy(gameObject);
	    }
	}
}
