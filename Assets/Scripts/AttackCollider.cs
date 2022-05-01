using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{

    [SerializeField] private string enemyPlayerTag;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(enemyPlayerTag) == true)

        {
            print("Collision!");
            collision.gameObject.GetComponent<PlayerBehaviour>().TakeDamage();
        }

    }
}
