using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoMovement : MonoBehaviour
{
    public float moveSpeed;

    public float attackSpeed;

    public Rigidbody2D body;

    private Vector3 moveDirection;

    private Vector3 newPosition;

    private bool attack = false;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        attack = false;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float moveY = Input.GetAxisRaw("Vertical2");

        moveDirection = new Vector3(0, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            destination();
            attack = true;
        }

    }

    private void destination()
    {
        newPosition = gameObject.transform.position;
        newPosition.x = -newPosition.x;
    }


    void FixedUpdate()
    {
        body.velocity = new Vector3(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

        if (attack == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * attackSpeed);

            if (transform.position == newPosition && transform.position.x != startPosition.x)
            {
                destination();
                transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * attackSpeed);

            }

            if (transform.position.x == startPosition.x)
            {
                attack = false;
            }
        }
    }
}
