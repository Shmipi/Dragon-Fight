using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    float timeLeft = 5.0f;
    float randomNr;

    private void Update()
    {
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
}