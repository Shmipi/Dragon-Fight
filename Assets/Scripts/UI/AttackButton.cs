using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    private PlayerBehaviour playerBehaviour;

    public void Attack()
    {
        playerBehaviour.AttackButton();
    }

    public void SetPlayer(PlayerBehaviour playerBehaviour)
    {
        this.playerBehaviour = playerBehaviour;
    }
}
