using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static System.Collections.Specialized.BitVector32;

public class PlayerBehaviour : MonoBehaviour
{
    #region Attributes
    public Player player;

    private PlayerUIManager playerUIManager;
    private MatchController matchController;

    [SerializeField] private Transform neckTopPosition;
    [SerializeField] private Transform headTopLocalPosition;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float hurtTime = 0.2f;

    [SerializeField] private Transform neckBoneContainer;
    [SerializeField] private Transform headBoneContainer;

    [SerializeField] private Transform neckAtkTarget;

    [SerializeField] private NeckCollider neckCollider;
    [SerializeField] private AttackCollider attackCollider;

    //[SerializeField] private Transform rootBoneContainer;

    private PlayerAnimationManager animationManager;
    private PlayerSoundManager playerSoundManager;
    private AIController ai_controller;

    private const int maxHP = 5;
    private int currentHP;
    [SerializeField] private bool changeStateNext;

    //private PlayerStateEnum playerState;
    //private PlayerStateEnum previousState;
    private State currentState;
    private State previousState;

    [SerializeField] private PlayerStateEnum showState;

    private Vector3 neckTargetPosition;
    private Vector3 headTargetPosition;
    private Vector3 neckOriginalPosition;
    private Vector3 headOriginalPosition;
    [SerializeField] private float elapsedTime = 0f;
    [SerializeField] private float currentAnimationTimeDuration;

    private readonly Vector3 neckAtkPositionWorldOffset = new Vector3(14f, 0.33f, 0f);
    private readonly Vector3 headAtkPositionLocalOffset = new Vector3(1.1f, 0.05f, 0f);

    private Vector3 neckSpawnWorldPosition;
    private Vector3 headSpawnLocalPosition;

    private Vector3 neckBoneOriginalLocalPosition;
    private Vector3 headBoneOriginalLocalPosition;

    private Quaternion neckBoneOriginalLocalRotation;
    private Quaternion headBoneOriginalLocalRotation;

    private Quaternion neckBoneTargetLocalRotation;
    private Quaternion headBoneTargetLocalRotation;

    private float targetNeckRotation;
    private float targetHeadRotation;

    private const float headRotation = 30f;
    private const float neckRotation = 4f;

    private StateManager stateManager;
    private DragonSkin dragonSkin;
    private bool isAI;

    //private DebugControls debugControls;

    public PlayerUIManager PlayerUIManager { get => playerUIManager; set => playerUIManager = value; }
    public MatchController MatchController { get => matchController; set => matchController = value; }


    public bool GetIsDead()
    {
        return currentHP <= 0;
    }

    public AIController GetAi_controller()
    {
        return ai_controller;
    }

    public void SetAi_controller(AIController value)
    {
        isAI = true;
        ai_controller = value;
    }



    #endregion

    #region Settling and Update Voids

    private void Awake()
    {
        stateManager = new StateManager();
        currentState = new SpawnState();
    }

    private void Start()
    {
        //debugControls = new DebugControls();
        currentHP = maxHP;

        currentAnimationTimeDuration = currentState.AnimationDuration;

        animationManager = GetComponentInChildren<PlayerAnimationManager>();
        animationManager.PlayerBehaviour = this;
        animationManager.SetStateManager(stateManager, currentState);
        SetPlayerSkin();
        animationManager.SpriteSkin = dragonSkin.ToString();

        playerSoundManager = GetComponent<PlayerSoundManager>();

        neckBoneOriginalLocalPosition = neckBoneContainer.localPosition;
        headBoneOriginalLocalPosition = headBoneContainer.localPosition;

        neckSpawnWorldPosition = neckBoneContainer.position;
        headSpawnLocalPosition = headBoneContainer.localPosition;

        attackCollider.PlayerBehaviour = this;
        neckCollider.PlayerBehaviour = this;
        neckCollider.AttackCollider = attackCollider;

        //SetDebugControls();
    }

    /*
    private void SetDebugControls()
    {
        debugControls.DebugActionMap.Enable();
        debugControls.DebugActionMap.Idle.performed += OnIdlePerformed;
        debugControls.DebugActionMap.Hurt.performed += OnHurtPerformed;
        debugControls.DebugActionMap.MoveUp.performed += OnMoveUpPerformed;
        debugControls.DebugActionMap.MoveDown.performed += OnMoveDownPerformed;
        debugControls.DebugActionMap.AttackF.performed += OnAttackFPerformed;
        debugControls.DebugActionMap.AttackB.performed += OnAttackBPerformed;
    }

    private void OnIdlePerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("Idle Performed");
        ChangeState(stateManager.GetState(PlayerStateEnum.Idle));
    }

    private void OnMoveUpPerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("MoveUp Performed");
        ChangeState(stateManager.GetState(PlayerStateEnum.MoveUp));
    }

    private void OnMoveDownPerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("MoveDown Performed");
        ChangeState(stateManager.GetState(PlayerStateEnum.MoveDown));
    }

    private void OnHurtPerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("Hurt Performed");
        ChangeState(stateManager.GetState(PlayerStateEnum.Hurt));
    }

    private void OnAttackFPerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("AttackF Performed");
        ChangeState(stateManager.GetState(PlayerStateEnum.AttackF));
    }

    private void OnAttackBPerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("AttackB Performed");
        ChangeState(stateManager.GetState(PlayerStateEnum.AttackB));
    }

    */

    private void Update()
    {
      
    }

    private void FixedUpdate()
    {    
        StateUpdate();
        showState = currentState.PlayerState;
    }
    #endregion

    #region Public methods

    public void AttackButton()
    {
        // Attack only if the player is in MoveUp, MoveDown, or Idle state
        if (currentState is MoveUpState || currentState is MoveDownState || currentState is IdleState)
        {
            ChangeState(stateManager.GetState(PlayerStateEnum.AttackF));
        }
    }

    public void MoveOnHoldButton()
    {
        // Move up only if not already moving up
        if (currentState is MoveDownState || currentState is IdleState)
        {
            ChangeState(stateManager.GetState(PlayerStateEnum.MoveUp));
        }
    }

    public void MoveOnReleaseButton()
    {
        if (currentState is MoveUpState)
        {
            ChangeState(stateManager.GetState(PlayerStateEnum.MoveDown));
        }
    }

    public void StartSpawn()
    {
        ChangeState(stateManager.GetState(PlayerStateEnum.Spawn));     
    }

    public void TakeDamage()
    {
        currentState = stateManager.GetState(PlayerStateEnum.Hurt);
        animationManager.PlayAnimation(stateManager.GetState(PlayerStateEnum.Hurt), 0f);
    }

    public void ForceReturn()
    {

        switch (currentState.PlayerState)
        {
            case PlayerStateEnum.Idle:
                ChangeState(stateManager.GetState(PlayerStateEnum.Idle));
                break;

            case PlayerStateEnum.MoveUp:
                ChangeState(stateManager.GetState(PlayerStateEnum.MoveDown));
                break;

            case PlayerStateEnum.MoveDown:
                ChangeState(stateManager.GetState(PlayerStateEnum.MoveDown));
                break;

            case PlayerStateEnum.AttackF:
                ChangeState(stateManager.GetState(PlayerStateEnum.AttackB));
                break;

            case PlayerStateEnum.AttackB:
                ChangeState(stateManager.GetState(PlayerStateEnum.AttackB));
                break;

        }
    }

    public void Win()
    {
        ChangeState(stateManager.GetState(PlayerStateEnum.Win));
    }

    public void Lose()
    {
        ChangeState(stateManager.GetState(PlayerStateEnum.Lose));
    }

    public void Hurt()
    {
        ChangeState(stateManager.GetState(PlayerStateEnum.Hurt)); 
    }

    public void ResetPlayer()
    {
        animationManager.Restart();

        ChangeState(stateManager.GetState(PlayerStateEnum.Spawn));
    }

    #endregion

    #region StateMachine

    public void ChangeState(State newState)
    {
        StartCoroutine(ChangeStateTimer(newState, 0f));
    }

    private void ChangeState(State newState, float delay)
    {
        StartCoroutine(ChangeStateTimer(newState, delay));
    }

    private void NextState()
    {
        switch(currentState.PlayerState)
        {
            case PlayerStateEnum.MoveUp:
                ChangeState(stateManager.GetState(PlayerStateEnum.MoveDown));

                if(isAI)
                {
                    ai_controller.ForceRelease();
                }

                break;

            case PlayerStateEnum.MoveDown:
                ChangeState(stateManager.GetState(PlayerStateEnum.Idle));
                break;

            case PlayerStateEnum.AttackF:
                ChangeState(stateManager.GetState(PlayerStateEnum.AttackB));
                break;

            case PlayerStateEnum.AttackB:

                float exitTime = stateManager.GetState(PlayerStateEnum.MoveDown).AnimationExitTime;

                if (Mathf.Approximately(0f, exitTime))
                {
                    ChangeState(stateManager.GetState(PlayerStateEnum.Idle));
                }

                else
                {
                    ChangeState(stateManager.GetState(PlayerStateEnum.MoveDown));
                }
                break;
        }

        changeStateNext = false;
    }

    private void OnExitState(State oldState)
    {
        //oldState.AnimationExitTime = elapsedTime;
        /* Debug.Log("Old state was: " + oldState + " with time: " + currentAnimationTimeDuration +
            " and elapsed time: " + elapsedTime);
        */

        oldState.AnimationExitTime = elapsedTime;

        State mirrorState = stateManager.MirrorState(oldState.PlayerState);

        if (mirrorState != null)
        {
            mirrorState.AnimationExitTime = InverseAnimationTime(oldState, mirrorState);
        }

    }

    private IEnumerator ChangeStateTimer(State newState, float waitTIme)
    {
        if (currentState != newState)
        {
            yield return new WaitForSeconds(waitTIme);
            Debug.Log(player + " New state: " + newState);
            previousState = currentState;
            OnExitState(previousState);
            currentState = newState;
            EnterNewState();
        }
        yield return null;
    }

    private void EnterNewState()
    {
        currentAnimationTimeDuration = currentState.AnimationDuration;
        CalculateElapsedTime();

        switch (currentState.PlayerState)
        {
            case PlayerStateEnum.Spawn:
                OnEnterSpawn();
                break;
            case PlayerStateEnum.Idle:
                OnEnterIdle();
                break;
            case PlayerStateEnum.MoveUp:
                OnEnterMoveUp();
                break;
            case PlayerStateEnum.MoveDown:
                OnEnterMoveDown();
                break;
            case PlayerStateEnum.AttackF:
                OnEnterAttackF();
                break;
            case PlayerStateEnum.AttackB:
                OnEnterAttackB();
                break;
            case PlayerStateEnum.Hurt:
                OnEnterHurt();
                break;
            case PlayerStateEnum.Win:
                OnEnterWin();
                break;
            case PlayerStateEnum.Lose:
                OnEnterLose();
                break;
            case PlayerStateEnum.Locked:
                OnEnterLocked();
                break;
        }
    }


    private void StateUpdate()
    {
        if (changeStateNext)
        {
            NextState();
            return;
        }

        float elapsedNormTime = CalculateNormalTime(elapsedTime);

        switch (currentState.PlayerState)
        {
            case PlayerStateEnum.MoveUp:
            case PlayerStateEnum.MoveDown:
                MoveNeck(neckOriginalPosition, neckTargetPosition, elapsedNormTime);
                MoveHead(headOriginalPosition, headTargetPosition, elapsedNormTime);
                MoveAtkTarget();
               break;

            case PlayerStateEnum.AttackF:
            case PlayerStateEnum.AttackB:
                //MoveAtkPosition(neckTargetPosition);
                MoveNeck(neckOriginalPosition, neckTargetPosition, elapsedNormTime);
                MoveHead(headOriginalPosition, headTargetPosition, elapsedNormTime);
                Rotate(elapsedNormTime);
                break;

            default:
                break;
        }

        elapsedTime += Time.deltaTime;
    }

    private void MoveAtkTarget()
    {
        Vector3 projectedOffset = transform.TransformDirection(neckAtkPositionWorldOffset);
        neckAtkTarget.position = neckBoneContainer.position + projectedOffset;
    }

    private void MoveNeck(Vector3 neckOriginalPosition, Vector3 neckTargetPosition, float elapsedNormTime)
    {
        neckBoneContainer.position = Vector3.Lerp(neckOriginalPosition, neckTargetPosition, elapsedNormTime);
    }

    private void MoveHead(Vector3 headOriginalLocalPosition, Vector3 headTargetLocalPosition, float elapsedNormTime)
    {
        headBoneContainer.localPosition = Vector3.Lerp(headOriginalLocalPosition, headTargetLocalPosition, elapsedNormTime);
    }

    private float CalculateNormalTime(float elapsedFlatTime)
    {
        float threshold = 0.01f;

        float elapsedNormTime = elapsedFlatTime / currentAnimationTimeDuration;
        if (Mathf.Abs(elapsedNormTime - 1f) < threshold || elapsedNormTime > 1f)
        {
            elapsedNormTime = 1f;
            CheckChangeNext();
        }

        if (elapsedNormTime < 0f)
        {
            Debug.LogWarning("Animation time was negative");
            elapsedNormTime = 0f;
        }

        return elapsedNormTime;
    }

    private void CheckChangeNext()
    {
        if(currentState is MoveUpState || currentState is MoveDownState || 
            currentState is AttackBState || currentState is AttackFState)
        {
            changeStateNext = true;
        }
    }

    private void MoveAtkPosition(Vector3 neckTargetPosition)
    {
        Vector3 newtarget = new Vector3(neckBoneContainer.position.x, neckTargetPosition.y, neckBoneContainer.position.z);
        neckBoneContainer.position = Vector3.MoveTowards(neckBoneContainer.position, newtarget, Time.fixedDeltaTime * 250f);
        //Debug.Log(newtarget);
    }

    private void Rotate(float elapsedNormTime)
    {
        neckBoneContainer.localRotation = Quaternion.Lerp(neckBoneOriginalLocalRotation, neckBoneTargetLocalRotation, elapsedNormTime);
        headBoneContainer.localRotation = Quaternion.Lerp(headBoneOriginalLocalRotation, headBoneTargetLocalRotation, elapsedNormTime);
    }

    private void OnEnterSpawn()
    {
        currentHP = maxHP;
        playerUIManager.ResetHP();
        neckTargetPosition = neckSpawnWorldPosition;
        neckOriginalPosition = neckSpawnWorldPosition;
        headTargetPosition = headSpawnLocalPosition;
        headOriginalPosition = headSpawnLocalPosition;

        neckBoneContainer.position = neckSpawnWorldPosition;
        headBoneContainer.localPosition = headSpawnLocalPosition;

        neckBoneContainer.localRotation = Quaternion.Euler(Vector3.zero);
        headBoneContainer.localRotation = Quaternion.Euler(Vector3.zero);

        stateManager.ResetStates();

        MoveAtkTarget();

        ChangeState(stateManager.GetState(PlayerStateEnum.Idle), 3f);
    }

    private void OnEnterIdle()
    {
        //Debug.Log("On Enter Idle called");
        FixLocalPosition();
        float elapsedNormTime = elapsedTime / currentAnimationTimeDuration;
        animationManager.PlayAnimation(stateManager.GetState(PlayerStateEnum.Idle), elapsedNormTime);
    }

    private void OnEnterMoveUp()
    {
        neckTargetPosition = neckTopPosition.position;
        headTargetPosition = headTopLocalPosition.localPosition;

        neckOriginalPosition = neckSpawnWorldPosition;
        headOriginalPosition = headSpawnLocalPosition;

        //Debug.Log("Enter move up void " + "elapsed time: " + elapsedTime);
        float elapsedNormTime = elapsedTime / currentAnimationTimeDuration;
        animationManager.PlayAnimation(stateManager.GetState(PlayerStateEnum.MoveUp), elapsedNormTime);

    }

    private void OnEnterMoveDown()
    {
        neckTargetPosition = neckSpawnWorldPosition;
        headTargetPosition = headSpawnLocalPosition;

        neckOriginalPosition = neckTopPosition.position;
        headOriginalPosition = headTopLocalPosition.localPosition;

        float elapsedNormTime = elapsedTime / currentAnimationTimeDuration;
        animationManager.PlayAnimation(stateManager.GetState(PlayerStateEnum.MoveDown), elapsedNormTime);
    }

    private void OnEnterAttackF()
    {

        playerSoundManager.PlayAttackSound();

        Vector3 projectedHeadOffset = headBoneContainer.TransformPoint(headAtkPositionLocalOffset);
        //Debug.Log(projectedHeadOffset);
        neckTargetPosition = neckAtkTarget.position;
        headTargetPosition = headBoneContainer.InverseTransformPoint(projectedHeadOffset);

        float lerp = 0f;
        if(previousState.PlayerState != PlayerStateEnum.Idle)
        {
            lerp = Mathf.Min(1f, previousState.AnimationExitTime / previousState.AnimationDuration);
            //Debug.Log("Lerp: " + lerp);
        }

        targetNeckRotation = Mathf.Lerp(0f, neckRotation, lerp);
        targetHeadRotation = Mathf.Lerp(0f, headRotation, lerp);
        //Debug.Log("Target neck Rotation: " + targetNeckRotation);
 
        neckBoneOriginalLocalRotation = neckBoneContainer.localRotation;
        neckBoneTargetLocalRotation = Quaternion.Euler(0f, 0f, targetNeckRotation);

        headBoneOriginalLocalRotation = headBoneContainer.localRotation;
        headBoneTargetLocalRotation = Quaternion.Euler(0f, 0f, targetHeadRotation);

        neckOriginalPosition = neckBoneContainer.position;
        headOriginalPosition = headBoneContainer.localPosition;

        //Debug.Log("Origin: " + neckOriginalPosition);
        //Debug.Log("Atk position: " + neckTargetPosition);
        float elapsedNormTime = elapsedTime / currentState.AnimationDuration;

        animationManager.PlayAnimation(stateManager.GetState(PlayerStateEnum.AttackF), elapsedNormTime);
    }

    private void OnEnterAttackB()
    {
        Vector3 neckAtkReturnPosition = neckOriginalPosition;
        Vector3 headAtkReturnPosition = headOriginalPosition;

        neckOriginalPosition = neckTargetPosition;
        headOriginalPosition = headTargetPosition;

        neckTargetPosition = neckAtkReturnPosition;
        headTargetPosition = headAtkReturnPosition;

        neckBoneOriginalLocalRotation = neckBoneContainer.localRotation;
        neckBoneTargetLocalRotation = Quaternion.Euler(Vector3.zero);

        headBoneOriginalLocalRotation = headBoneContainer.localRotation;
        headBoneTargetLocalRotation = Quaternion.Euler(Vector3.zero);

        float elapsedNormTime = 1f - elapsedTime / currentState.AnimationDuration;
        animationManager.PlayAnimation(stateManager.GetState(PlayerStateEnum.AttackB), elapsedNormTime);
    }

    private void OnEnterHurt()
    {
        playerSoundManager.PlayHurtSound();

        currentHP--;
        playerUIManager.ReduceHealth();

        animationManager.PlayAnimation(stateManager.GetState(PlayerStateEnum.Hurt), 0f);

        if(currentHP <= 0) 
        {
            matchController.OnDragonDead();
        }

        else
        {
            ChangeState(stateManager.ResumeState(previousState.PlayerState), hurtTime);
        }
    }

    private void OnEnterWin()
    {
        animationManager.PlayAnimation(stateManager.GetState(PlayerStateEnum.Win), 0f);

        if (isAI)
        {
            ai_controller.SetStandby();
        }
    }

    private void OnEnterLose()
    {
        animationManager.PlayAnimation(stateManager.GetState(PlayerStateEnum.Lose), 0f);

        if (isAI)
        {
            ai_controller.SetStandby();
        }

    }

    private void OnEnterLocked()
    {
        
    }

    #endregion

    #region Void Behaviour

    private void SetPlayerSkin()
    {
        dragonSkin = player == Player.Player1 ? SelectDragon.Player1Skin : SelectDragon.Player2Skin;
    }

    private void CalculateElapsedTime()
    {
        elapsedTime = currentState.AnimationExitTime;
    }

    private void FixLocalPosition()
    {
        neckBoneContainer.transform.localPosition = neckBoneOriginalLocalPosition;
        headBoneContainer.transform.localPosition = headBoneOriginalLocalPosition;
    }

    private float InverseAnimationNormTime(State state)
    {
        float inverseTime = state.AnimationDuration - elapsedTime;
        float inverseNormalized = inverseTime / state.AnimationDuration;
        return inverseNormalized;
    }

    private float InverseAnimationTime(State oldState, State mirrorState)
    {
        float inverseTime = oldState.AnimationDuration - elapsedTime;

        if(inverseTime < 0f)
        {
            return 0f;
        }

        inverseTime /= oldState.AnimationDuration;

        inverseTime *= mirrorState.AnimationDuration;

        return inverseTime;
    }

    #endregion
}
