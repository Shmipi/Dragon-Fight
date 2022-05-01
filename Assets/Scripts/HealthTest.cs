using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTest : MonoBehaviour {

    public int maxHealth = 5;
    public int currentHealth;

    public HealthBar healthBar;

    private float startTime = 0f;
    [SerializeField] private float holdTime = 2.0f;
    private float timer = 0f;

    private bool collided = false;

    private EdgeCollider2D enemyCollider;

    void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update() {

        if(collided == true)
        {
            timer += Time.deltaTime;

            if (timer > (startTime + holdTime))
            {
                collided = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AttackCollider") == true && collided == false)
        {
            TakeDamage(1);
            collided = true;
            startTime = Time.time;
            timer = startTime;
        }
    }

    void TakeDamage(int damage) {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
