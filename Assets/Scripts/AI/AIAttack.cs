using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    private PlayerBehaviour playerBehaviour;

    float timeLeft = 5.0f;
    float randomNr;
    private float delay = 3.0f;

    public PlayerBehaviour PlayerBehaviour { get => playerBehaviour; set => playerBehaviour = value; }

    private void Update()
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
            return;
        }


        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            Attack();
            randomNr = Random.Range(0, 3);
            timeLeft = randomNr;
        }
    }

    public void Attack()
    {
        playerBehaviour.AttackButton();
    }

    public void ResetDelay()
    {
        delay = 3f;
    }


}