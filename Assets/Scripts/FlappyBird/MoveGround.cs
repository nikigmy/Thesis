using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour {
    
	void Update () {
		if(transform.position.x <= -18)
            transform.position = new Vector3(36, transform.position.y, transform.position.z);
        if(!FlappyBirdMain.singleton.GameOver)
		transform.Translate(-Vector3.right * 0.05f);
	}
}
