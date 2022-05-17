using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{

    [SerializeField] private string enemyHealthTag;
    [SerializeField] private string enemyAttackTag;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(enemyHealthTag))
        {
            print(enemyAttackTag + "is taking dmg");
            collider.gameObject.GetComponent<PlayerBehaviour>().TakeDamage();
            collider.gameObject.GetComponent<PlayerBehaviour>().ForceReturn();
            gameObject.GetComponentInParent<PlayerBehaviour>().ForceReturn();
        }

        if (collider.gameObject.CompareTag(enemyAttackTag))
        {
            print("Head Collision");
            collider.gameObject.GetComponentInParent<PlayerBehaviour>().ForceReturn();
        }
    }
}
