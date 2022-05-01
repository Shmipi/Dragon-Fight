using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform topPosition;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Transform returnPosition;
    [SerializeField] private int maxHP;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackMovementSpeed;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float hurtTime;

    private Vector3 hitBoxReference;
    private int currentHP;
    private string currentState;
    private bool busy;
    private const string SPAWN = "Spawn";
    private const string STAND_BY = "Standby";
    private const string MOVE = "Move";
    private const string ATTACK = "Attack";
    private const string HURT = "Hurt";
    private const string WIN = "Win";
    private const string LOSE = "Lose";
    private PlayerState playerState;
    private Rigidbody2D rigidBody2D;
    private Animator animator;
    private EdgeCollider2D hitBoxCollider;
    private bool canMove;
    private bool canAttack;
    private string currentAnimation;
    private Vector2 offset;

    private enum PlayerState
    {
        Spawn,
        Standby,
        Move,
        Attack,
        Hurt,
        Win,
        Lose,
        Locked,
    }

    private void Awake()

    {
        hitBoxCollider = gameObject.GetComponent<EdgeCollider2D>();
        hitBoxReference = gameObject.transform.localPosition;
        currentHP = maxHP;
        canMove = false;
        SoundFXController.PlaySound(SoundFXController.Sound.BackgroundMusik);
        player.transform.position = spawnPosition.transform.position;
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        offset = hitBoxCollider.points[1];

    }

    private void FixedUpdate()
    {
        Vector2 zeros = Vector2.zero;
        Vector2 setReference = new Vector2(hitBoxReference.x - gameObject.transform.localPosition.x,
            hitBoxReference.y - gameObject.transform.localPosition.y);
        hitBoxCollider.points = new Vector2[2]{zeros, setReference + offset};
        print(gameObject.transform.localPosition.y - hitBoxReference.y + ", " + player.ToString());
        if (!busy)
        {
            ChangeState();
        }
    }

    public void AttackButton()
    {
        if (!canMove)
        {
            return;
        }

        if (!busy)
        {               
            playerState = PlayerState.Attack;
        }
    }

    public void MoveOnHoldButton()
    {
        if (!canMove)
        {
            return;
        }

        if (!busy)
        {   
            playerState = PlayerState.Move;
        }
    }

    public void MoveOnReleaseButton()
    {
        if (!busy)
        {
            playerState = PlayerState.Standby;
        }
    }

    public void StartSpawn()
    {
        playerState = PlayerState.Spawn;
    }

    public void TakeDamage()
    {
        playerState = PlayerState.Hurt;
    }


    #region StateMachine

    private void ChangeState()
    {
        if (playerState.ToString() != currentState)
        {
            print(playerState.ToString());
            currentState = playerState.ToString();
        }


        switch (playerState)
        {
            case PlayerState.Spawn:
                StartCoroutine(Spawn());
                break;
            case PlayerState.Standby:
                Standby();
                break;
            case PlayerState.Move:
                Move();
                break;
            case PlayerState.Attack:
                Attack();
                break;
            case PlayerState.Hurt:
                StartCoroutine(Hurt());
                break;
            case PlayerState.Win:
                StartCoroutine(Win());
                break;
            case PlayerState.Lose:
                StartCoroutine(Lose());
                break;
            default:
                break;
        }
    }

    #endregion 

    #region Void Behaviour

    private IEnumerator Spawn()
    {
        busy = true;
        yield return new WaitForSeconds(1f);
        //spawn animation
        busy = false;
        canAttack = true;
        canMove = true;
        playerState = PlayerState.Standby;
    }

    private void Standby()
    {
        player.transform.position
            = Vector2.MoveTowards(player.transform.position, spawnPosition.position, Time.fixedDeltaTime * movementSpeed * 0.8f);
    }

    private void Move()
    {
        player.transform.position 
            = Vector2.MoveTowards(player.transform.position, topPosition.position, Time.fixedDeltaTime * movementSpeed);
    }

    private void Attack()
    {              
        player.transform.position
            = Vector2.MoveTowards(player.transform.position, targetPosition.position, Time.fixedDeltaTime * attackMovementSpeed);
        if (canAttack)
        {
            canAttack = false;
            StartCoroutine(AttackCooldown());
        }
       
    }

    private IEnumerator AttackCooldown()
    {
        
        print("Attacking");
        canMove = false;
        yield return new WaitForSeconds(attackCooldown / 2);
        Transform oldTarget = targetPosition;
        targetPosition = returnPosition;
        yield return new WaitForSeconds(attackCooldown / 2);
        targetPosition = oldTarget;
        canAttack = true;
        canMove = true;
        playerState = PlayerState.Standby;
    }

    private IEnumerator Hurt()
    {
        canMove = false;
        busy = true;
        print("Getting Hurt");
        currentHP--;
        yield return new WaitForSeconds(hurtTime);
        canMove = false;
        busy = true;
    }

    private IEnumerator Win()
    {
        yield break;
    }

    private IEnumerator Lose()
    {
        yield break;
    }

    protected void ChangeAnimation(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    private IEnumerator LockedTime(float lockTime)
    {
        playerState = PlayerState.Locked;
        yield return new WaitForSeconds(lockTime);
        playerState = PlayerState.Standby;
    }

    #endregion
}
