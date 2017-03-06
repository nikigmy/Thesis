using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {
	void Update () {
	    if (!EndlessRunnerMain.singleton.GameOver)
	    {
	        transform.Translate(-Vector3.forward*0.5f, Space.World);
	        if (transform.position.z <= -10)
	        {
	            transform.position = new Vector3(transform.position.x, transform.position.y, 100);
	        }
	    }
	}
}
