using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rig;
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !FlappyBirdMain.singleton.GameOver)
        {
            anim.SetTrigger("Flap");
            rig.velocity = Vector2.zero;
            rig.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        FlappyBirdMain.singleton.OnGameOver();
        anim.SetTrigger("Die");

    }
    void OnTriggerEnter2D(Collider2D other)
    {
       FlappyBirdMain.singleton.OnScore();
    }
}
