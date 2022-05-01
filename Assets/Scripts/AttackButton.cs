using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    [SerializeField] private GameObject attackCollider;

    private float startTime = 0f;
    [SerializeField] private float holdTime = 4.0f;
    private float timer = 0f;

    private bool attack = false;

    private void Update()
    {
        if (attack == true)
        {
            timer += Time.deltaTime;

            if (timer > (startTime + holdTime))
            {
                attackCollider.GetComponent<BoxCollider2D>().enabled = false;
                attackCollider.GetComponent<CapsuleCollider2D>().enabled = false;
                attack = false;
            }
        }
    }

    public void Attack()
    {
        playerBehaviour.AttackButton();
        SoundFXController.PlaySound(SoundFXController.Sound.PlayerAttack);

        attackCollider.GetComponent<BoxCollider2D>().enabled = true;
        attackCollider.GetComponent<CapsuleCollider2D>().enabled = true;
        attack = true;
        startTime = Time.time;
        timer = startTime;
    }
}
