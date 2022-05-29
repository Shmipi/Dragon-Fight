using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIMove : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    float timeLeft = 3.0f;
    float moveOnRelease = 2.0f;
    float randomNr;
    bool released = false;

    private void Update()
    {
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
            if (released == false)
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
}
