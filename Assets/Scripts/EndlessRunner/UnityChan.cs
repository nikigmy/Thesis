using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChan : MonoBehaviour
{
    private Animator anim;

    public static UnityChan singleton;
    public enum SwipeDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    public enum Lane
    {
        Left, Middle, Right
    }

    public enum Direction
    {
        Left, Right
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("Run", 0.5f);
        currentLane = Lane.Middle;
        moving = false;
        singleton = this;
        anim.speed = 0;
    }

    private Lane currentLane;
    private bool swiping = false;
    private bool eventSent = false;
    private Vector2 lastPosition;
    private bool moving;
    private bool jumping;
    private bool sliding;

    public void Die()
    {
        anim.SetTrigger("Die");
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            swiping = true;
            lastPosition = Input.GetTouch(0).position;
            return;
        }
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            if (!eventSent)
            {
                Vector2 direction = Input.GetTouch(0).position - lastPosition;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (direction.x > 0)
                        GetSwipe(SwipeDirection.Right);
                    else if (direction.x < 0)
                        GetSwipe(SwipeDirection.Left);
                }
                else
                {
                    if (direction.y > 0)
                        GetSwipe(SwipeDirection.Up);
                    else if (direction.y < 0)
                        GetSwipe(SwipeDirection.Down);
                }
                eventSent = true;
            }
        }
        if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
        {
            swiping = false;
            eventSent = false;
        }
    }

    void GetSwipe(SwipeDirection direction)
    {
        if (direction == SwipeDirection.Down)
        {
            if (!sliding)
            {
                anim.SetTrigger("Slide");
                sliding = true;
                if (jumping)
                    jumping = false;
            }
        }
        if (direction == SwipeDirection.Up)
        {
            if (!jumping)
            {
                anim.SetTrigger("Jump");
                jumping = true;
                if (sliding)
                    sliding = false;
            }
        }
        if (direction == SwipeDirection.Left)
        {
            if (!moving)
            {
                moving = true;
                StartCoroutine(Move(Direction.Left));
            }
        }
        if (direction == SwipeDirection.Right)
        {
            if (!moving)
            {
                moving = true;
                StartCoroutine(Move(Direction.Right));
            }
        }
    }

    IEnumerator Move(Direction dir)
    {
        float startposition = transform.position.x;
        anim.SetTrigger("Move");
        if (dir == Direction.Left)
        {
            if (currentLane != Lane.Left)
            {
                while (true)
                {
                    if (transform.position.x <= startposition - 4.6f)
                    {
                        transform.position = new Vector3(startposition - 4.6f, transform.position.y,
                            transform.position.z);
                        anim.SetFloat("Run", 0.5f);
                        anim.ResetTrigger("Move");
                        currentLane = currentLane == Lane.Right ? Lane.Middle : Lane.Left;
                        moving = false;
                        break;
                    }
                    else
                    {
                        transform.Translate(-transform.right * 0.3f);
                        anim.SetFloat("Run", 0.25f);
                    }

                    yield return new WaitForFixedUpdate();
                }
            }
            else moving = false;
        }
        else
        {
            if (currentLane != Lane.Right)
            {
                while (true)
                {
                    if (transform.position.x >= startposition + 4.6f)
                    {
                        transform.position = new Vector3(startposition + 4.6f, transform.position.y,
                            transform.position.z);
                        anim.SetFloat("Run", 0.5f);
                        anim.ResetTrigger("Move");
                        currentLane = currentLane == Lane.Left ? Lane.Middle : Lane.Right;
                        moving = false;
                        break;
                    }
                    else
                    {
                        transform.Translate(transform.right * 0.3f);
                        anim.SetFloat("Run", 1f);
                    }
                    yield return new WaitForFixedUpdate();
                }
            }
            else moving = false;
        }
    }

    void JumpEnded()
    {
        jumping = false;
    }

    void SlideEnded()
    {
        sliding = false;
    }

    public void PlayAnimations()
    {
        anim.speed = 1;
    }
}
