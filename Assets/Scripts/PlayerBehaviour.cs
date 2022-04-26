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

    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackMovementSpeed;
    [SerializeField] private float attackCooldown;
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
    private bool canAttack;

    private enum PlayerState
    {
        Spawn,
        Standby,
        Move,
        Attack,
        Hurt,
        Win,
        Lose,
    }

    private void Start()
    {
        SoundFXController.PlaySound(SoundFXController.Sound.BackgroundMusik);
        player.transform.position = spawnPosition.transform.position;
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!busy)
        {
            ChangeState();
        }
    }

    public void AttackButton()
    {
        if (!busy)
        {               
            playerState = PlayerState.Attack;
        }
    }

    public void MoveOnHoldButton()
    {
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

    #region StateMachine

    private void ChangeState()
    {
       
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
        print("Ready for battle!");
        print("3");
        yield return new WaitForSeconds(1f);
        print("2");
        yield return new WaitForSeconds(1f);
        print("1");
        yield return new WaitForSeconds(1f);
        print("Go!");
        busy = false;
        canAttack = true;
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
        yield return new WaitForSeconds(attackCooldown / 2);
        Transform oldTarget = targetPosition;
        targetPosition = returnPosition;
        yield return new WaitForSeconds(attackCooldown / 2);
        targetPosition = oldTarget;
        canAttack = true;
        playerState = PlayerState.Standby;
    }

    private IEnumerator Hurt()
    {
        print("Getting Hurt");
        yield break;
    }

    private IEnumerator Win()
    {
        yield break;
    }

    private IEnumerator Lose()
    {
        yield break;
    }

    #endregion

}
