using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    public void Attack()
    {
        playerBehaviour.AttackButton();
    }
}
