using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    #region Attributes
    [SerializeField] private GameObject player;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform topPosition;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private Transform returnPosition;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private int maxHP;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackMovementSpeed;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float hurtTime;
    [SerializeField] private MatchController matchController;
    [SerializeField] private Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip attackClip;

    private Vector3 hitBoxReference;
    private int currentHP;
    private string currentState;
    private bool busy;
    private const string SPAWN = "Spawn";
    private const string IDLE = "Idle";
    private const string MOVE = "Move";
    private const string RETURN = "Return";
    private const string ATTACKF = "AttackFwd";
    private const string ATTACKB = "AttackBck";
    private const string HURT = "Hurt";
    private const string WIN = "Win";
    private const string LOSE = "Lose";
    private const string IDLESPRITE = "IdleSprite";
    private const string HURTSPRITE = "HurtSprite";
    private const string ATKFSPRITE = "AttackFwdSprite";
    private const string ATKBSPRITE = "AttackBckSprite";
    private const string LOSESPRITE = "LoseSprite";
    private PlayerState playerState;
    private Rigidbody2D rigidBody2D;
    private EdgeCollider2D hitBoxCollider;
    private bool canMove;
    private bool canAttack;
    private string currentAnimation;
    private Vector2 offset;
    public HealthBar healthBar;
    [SerializeField] private float timeController = 0f;
    [SerializeField] private float currentSpeed;
    private float attackCooldownF;
    private float attackCooldownB;
    private bool reseted = false;
    private string playerName;
    private bool canPressAtk = true;

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

    #endregion

    #region Settling and Update Voids

    private void Awake()

    {
        playerState = PlayerState.Locked;
        healthBar.SetMaxHealth(maxHP);
        hitBoxCollider = gameObject.GetComponentInChildren<EdgeCollider2D>();
        hitBoxReference = gameObject.transform.localPosition;
        currentHP = maxHP;
        canMove = false;
        player.transform.position = spawnPosition.transform.position;
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        offset = hitBoxCollider.points[1];
        ResetState();
        CalculateCooldown();
        currentAnimation = IDLE;
        playerName = transform.GetChild(0).tag;

        if(playerName == "Player1")
        {
            audioSource.panStereo = -0.75f;
        }
        else if(playerName == "Player2")
        {
            audioSource.panStereo = 0.75f;
        }


        /*
        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip animClip in animationClips)
        {
            Debug.Log(animClip.name + ": " + animClip.length);
        }
        */
    }

    private void Update()
    {
        Move(currentTarget, currentSpeed);
        Vector2 zeros = Vector2.zero;
        Vector2 setReference = new Vector2(hitBoxReference.x - gameObject.transform.localPosition.x,
            hitBoxReference.y - gameObject.transform.localPosition.y);
        hitBoxCollider.points = new Vector2[2] { zeros, setReference + offset };
    }

    private void FixedUpdate()
    {    
        if (!busy)
        {
            ChangeState();
        }
    }
    #endregion

    #region Public methods

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
        canPressAtk = false;
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
        canPressAtk = true;
        if (!canMove)
        {
            return;
        }

        if (!busy)
        {
            timeController = exitAnimationFrame(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            ChangeAnimation(RETURN, timeController);

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
        animator.Play(HURTSPRITE, 2, 0f);

    }

    public void ForceReturn()
    {
        string clipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (clipName == IDLE) return;
        if(clipName == "Return0")
        {
            float exitTime = exitAnimationFrame(animator.GetCurrentAnimatorStateInfo(1).normalizedTime);
            StopAllCoroutines();
            StartCoroutine(ReturnFromAttacked(exitTime));
        }
        else
        {
            canMove = true;
            ChangeAnimation(RETURN, timeController);
            playerState = PlayerState.Standby;
        }
    }

    public void Win()
    {
        playerState = PlayerState.Win;
    }

    public void ResetPlayer()
    {
        gameObject.transform.position = spawnPosition.transform.position;
        ResetState();
    }
    #endregion

    #region StateMachine

    private void ChangeState()
    {
        if (playerState.ToString() != currentState)
        {
            print(animator.name.ToString() + ": " + playerState.ToString());
            currentState = playerState.ToString();
        }
        if(playerState != PlayerState.Standby)
        {
            reseted = false;
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
                MoveState();
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
        if (!reseted)
        {
            currentSpeed = movementSpeed * 0.8f;
            currentTarget = spawnPosition;
            animator.Play(IDLESPRITE, 2, 0f);
            reseted = true;
        }
    }

    private void MoveState()
    {
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == IDLE)
        {
            ChangeAnimation(MOVE, 0f);
        }
        else
        {
            ChangeAnimation(MOVE, exitAnimationFrame(animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
        }
        currentTarget = topPosition;
        currentSpeed = movementSpeed;
        
    }

    private void Attack()
    {
        if (canAttack)
        {           
            string exitAnimation;
            canAttack = false;
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == IDLE)
            {
                timeController = 1f;
                exitAnimation = IDLE;
            }
            else
            {
                timeController = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                exitAnimation = RETURN;
            }          
            StartCoroutine(AttackForward(exitAnimation));
        }     
    }

    private IEnumerator AttackForward(string exitAnimation)
    {
        canMove = false;
        audioSource.time = 1.2f;
        audioSource.Play();
        animator.Play(ATKFSPRITE, 2, 0f);
        animator.Play("Return0", 0, timeController);
        ChangeAnimation(ATTACKF);         
        currentTarget = attackPosition;
        currentSpeed = attackMovementSpeed;
        yield return new WaitForSeconds(attackCooldownF);
        animator.Play(ATKBSPRITE, 2, 0f);
        ChangeAnimation(ATTACKB);
        currentTarget = returnPosition;
        currentSpeed = attackMovementSpeed * attackCooldownF / attackCooldownB;
        yield return new WaitForSeconds(attackCooldownB);
        currentTarget = spawnPosition;
        currentSpeed = movementSpeed;
        if (exitAnimation == RETURN)
        {
            ChangeAnimation(RETURN, timeController);
        }
        else
        {
            ChangeAnimation(IDLE, 0f);
        }
        canAttack = true;
        canMove = true;
        playerState = PlayerState.Standby;
    }

    private IEnumerator ReturnFromAttacked(float exitTime)
    {
        currentTarget = returnPosition;
        animator.Play("Return0", 0, timeController);
        animator.Play(ATKBSPRITE, 2, 0f);
        animator.Play(ATTACKB, 1, exitTime);
        currentSpeed = attackMovementSpeed * attackCooldownF / attackCooldownB;
        yield return new WaitForSeconds(attackCooldownB * (1f - exitTime));
        canAttack = true;
        canMove = true;
        currentTarget = spawnPosition;
        ChangeAnimation(RETURN, timeController);
        playerState = PlayerState.Standby;
    }

    private IEnumerator Hurt()
    {   
        canMove = false;
        busy = true;
        hitBoxCollider.gameObject.SetActive(false);
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == MOVE)
        {
            timeController = exitAnimationFrame(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        else if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == RETURN)
        {
            timeController = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        healthBar.ReduceHealth();
        currentHP--;
        if(currentHP <= 0)
        {
            playerState = PlayerState.Lose;
            busy = false;
            yield break;
        }
        Transform oldTarget = currentTarget;
        currentTarget = gameObject.transform;

        yield return new WaitForSeconds(hurtTime);
        hitBoxCollider.gameObject.SetActive(true);
        currentTarget = oldTarget;
        busy = false;
        ForceReturn();
    }

    private IEnumerator WinState()
    {
        hitBoxCollider.gameObject.SetActive(false);
        canMove = false;
        busy = true;
        yield break;
    }

    private IEnumerator Lose()
    {
        hitBoxCollider.gameObject.SetActive(false);
        animator.Play(LOSESPRITE, 2, 0f);
        matchController.MatchFinished(playerName);
        canMove = false;
        busy = true;
        yield break;
    }

    
    private void ChangeAnimation(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;
        animator.Play(newAnimation, 1, 0f);
        currentAnimation = newAnimation;
    }

    private void ChangeAnimation(string newAnimation, float animationTime)
    {
        if (currentAnimation == newAnimation) return;
        animator.Play(newAnimation, 0, animationTime);
        currentAnimation = newAnimation;
    }
    

    private float exitAnimationFrame(float animation)
    {
        if (animation >= 1f)
            return 0f;
        else
        {
            return 1f - animation;
        }
    }

    /*
    private IEnumerator LockedTime(float lockTime)
    {
        playerState = PlayerState.Locked;
        yield return new WaitForSeconds(lockTime);
        playerState = PlayerState.Standby;
    }
    */

    private void Move(Transform target, float speed)
    {
        player.transform.position
              = Vector2.MoveTowards(player.transform.position, target.position, Time.deltaTime * 3f * speed);
    }

    private void CalculateCooldown()
    {
        attackCooldownF = attackCooldown * 0.4f;
        attackCooldownB = attackCooldown * 0.6f;
    }

    private void ResetState()
    {
        ChangeAnimation(IDLE, 0f);
        animator.Play(IDLESPRITE, 2, 0f);
        currentSpeed = movementSpeed;
        currentTarget = spawnPosition;
        currentHP = maxHP;
        hitBoxCollider.gameObject.SetActive(true);
        playerState = PlayerState.Locked;
        busy = false;
    }

    #endregion
}
