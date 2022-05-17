using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{

    private string enemyHealthTag;
    private string enemyAttackTag;
    [SerializeField] PlayerBehaviour playerBehaviour;

    private void Awake()
    {
        if(gameObject.tag == "P1Attack")
        {
            enemyHealthTag = "Player2";
            enemyAttackTag = "P2Attack";
        }

        else if(gameObject.tag == "P2Attack")
        {
            enemyHealthTag = "Player1";
            enemyAttackTag = "P1Attack";
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(enemyHealthTag))
        {
            collider.gameObject.GetComponentInParent<PlayerBehaviour>().TakeDamage();
            playerBehaviour.ForceReturn();
        }

        if (collider.gameObject.CompareTag(enemyAttackTag))
        {
            playerBehaviour.ForceReturn();
        }
    }
}
