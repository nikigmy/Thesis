using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collumn : MonoBehaviour
{

    void Update()
    {
        if (transform.position.x <= -6)
            Destroy(gameObject);
        if (!FlappyBirdMain.singleton.GameOver)
            transform.Translate(-Vector3.right * 0.05f);
    }
}
