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
    private Transform attackPosition;
    [SerializeField] private Transform returnPosition;
    [SerializeField] private int maxHP;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackMovementSpeed;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float hurtTime;
    [SerializeField] private MatchController matchController;

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

    public HealthBar healthBar;

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
        healthBar.SetMaxHealth(maxHP);
        hitBoxCollider = gameObject.GetComponent<EdgeCollider2D>();
        hitBoxReference = gameObject.transform.localPosition;
        currentHP = maxHP;
        canMove = false;
        SoundFXController.PlaySound(SoundFXController.Sound.BackgroundMusik);
        player.transform.position = spawnPosition.transform.position;
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        offset = hitBoxCollider.points[1];
        attackPosition = targetPosition;
    }

    private void FixedUpdate()
    {
        Vector2 zeros = Vector2.zero;
        Vector2 setReference = new Vector2(hitBoxReference.x - gameObject.transform.localPosition.x,
            hitBoxReference.y - gameObject.transform.localPosition.y);
        hitBoxCollider.points = new Vector2[2]{zeros, setReference + offset};
  //      print(gameObject.transform.localPosition.y - hitBoxReference.y + ", " + player.ToString());
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

    public void ForceReturn()
    {
        StopCoroutine(AttackForward());
        StartCoroutine(ReturnFromAttack());
    }

    public void Win()
    {
        playerState = PlayerState.Win;
    }

    public void ResetPosition()
    {
        gameObject.transform.position = spawnPosition.transform.position;
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
                StartCoroutine(WinState());
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
        yield return new WaitForSeconds(0.1f);
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
        if (canAttack)
        {
            canAttack = false;
            StartCoroutine(AttackForward());
        }
        player.transform.position
            = Vector2.MoveTowards(player.transform.position, targetPosition.position, Time.fixedDeltaTime * attackMovementSpeed);
       
    }

    private IEnumerator AttackForward()
    {
        targetPosition = attackPosition;
        print("Attacking");
        canMove = false;      
        yield return new WaitForSeconds(attackCooldown / 2);
        targetPosition = returnPosition;
        yield return new WaitForSeconds(attackCooldown / 2);
        targetPosition = attackPosition;
        canAttack = true;
        canMove = true;
        playerState = PlayerState.Standby;
    }

    private IEnumerator ReturnFromAttack()
    {
        targetPosition = returnPosition;
        yield return new WaitForSeconds(attackCooldown / 3);
        canAttack = true;
        canMove = true;
        playerState = PlayerState.Standby;
    }

        private IEnumerator Hurt()
    {
        canMove = false;
        busy = true;
        print("Getting Hurt");
        healthBar.ReduceHealth();
        currentHP--;
        if(currentHP <= 0)
        {
            busy = false;
            playerState = PlayerState.Lose;
            yield break;
        }
        yield return new WaitForSeconds(hurtTime);
        canMove = true;
        busy = false;
        playerState = PlayerState.Standby;
    }

    private IEnumerator WinState()
    {      
        busy = true;
        canMove = false;
        yield break;
    }

    private IEnumerator Lose()
    {
        matchController.MatchFinished(player.tag);
        busy = true;
        canMove = false;
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
