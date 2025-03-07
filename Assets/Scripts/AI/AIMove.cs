using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIMove : MonoBehaviour
{
    private PlayerBehaviour playerBehaviour;

    float timeLeft = 3.0f;
    float moveOnRelease = 2.0f;
    float randomNr;
    bool released = false;
    private float delay = 3.0f;

    public PlayerBehaviour PlayerBehaviour { get => playerBehaviour; set => playerBehaviour = value; }

    private void Update()
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
            return;
        }

        if (timeLeft > 0f)
        {
            MoveOnHold();
            timeLeft -= Time.deltaTime;
            randomNr = Random.Range(0, 5);
            moveOnRelease = randomNr;
            released = false;
        }

        if (timeLeft <= 0f)
        {
            if (!released)
            {
                MoveOnRelease();
                released = true;
            }
            moveOnRelease -= Time.deltaTime;

            if (moveOnRelease <= 0)
            {
                randomNr = Random.Range(0, 2);
                timeLeft = randomNr;
            }

        }


    }

    public void MoveOnHold()
    {
        playerBehaviour.MoveOnHoldButton();
    }

    public void MoveOnRelease()
    {
        playerBehaviour.MoveOnReleaseButton();
    }

    public void ResetDelay()
    {
        delay = 3f;
    }
}
