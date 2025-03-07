using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerAnimationManager : MonoBehaviour
{
    private const string ANIM_MAIN_LAYER_SPEED = "AnimMainLayerSpeed";
    private const string ANIM_HURT_LAYER_SPEED = "AnimHurtLayerSpeed";
    private const string ANIM_ATK_LAYER_SPEED = "AnimAttackLayerSpeed";

    private State currentState = null;
    private Animator animator;
    private SpriteLabel currentLabel;
    private PlayerBehaviour playerBehaviour;
    public PlayerBehaviour PlayerBehaviour { get => playerBehaviour; set => playerBehaviour = value; }

    private string spriteSkin = "";
    public string SpriteSkin { get => spriteSkin; set => spriteSkin = value; }

    public StateManager GetStateManager()
    {
        return stateManager;
    }

    public void SetStateManager(StateManager value, State state)
    {
        stateManager = value;
        currentState = state;
    }

    private SpriteResolver spriteResolver;
    private StateManager stateManager;

    private int[] animationIDs = new int[3];

    // Start is called before the first frame update
    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
        animator = GetComponent<Animator>();

        SetAnimationIDs();
        //Restart();
    }

    public void PlayAnimation(State newPlayerState, float animationNormalizedTime)
    {
        if (currentState == newPlayerState) return;

        int layer = newPlayerState.AnimationLayer;
        SetAnimatorSpeed(newPlayerState);
        animator.Play(newPlayerState.AnimationHash, layer, animationNormalizedTime);
        currentState = newPlayerState;
        SetAnimationSprite();
    }

    private void SetAnimatorSpeed(State newPlayerState)
    {
        int id = newPlayerState.AnimationID;
        float speed = newPlayerState.AnimationSpeed;

        for(int i = 0; i < animationIDs.Length; i++)
        {
            animator.SetFloat(animationIDs[i], (id == animationIDs[i]) ? speed : 0f);
        }
    }

    private void SetAnimationIDs()
    {
        animationIDs[0] = Animator.StringToHash(ANIM_MAIN_LAYER_SPEED);
        animationIDs[1] = Animator.StringToHash(ANIM_HURT_LAYER_SPEED);
        animationIDs[2] = Animator.StringToHash(ANIM_ATK_LAYER_SPEED);
    }

    private void SetAnimationSprite()
    {
        SpriteLabel newLabel = currentState.AnimationLabel;
        if (newLabel != currentLabel)
        {
            currentLabel = newLabel;
            //Debug.Log("Sprite Resolver called");
            spriteResolver.SetCategoryAndLabel(spriteSkin, currentLabel.ToString());

        }
    }

    public float CurrentAnimationNormTime(int layer)
    {
        return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime;
    }

    public void ChangeState(PlayerStateEnum state)
    {
        playerBehaviour.ChangeState(stateManager.GetState(state));
    }

    public void Restart()
    {
        animator.Play("Standby", 1);
        animator.Play("Standby", 2);
        PlayAnimation(stateManager.GetState(PlayerStateEnum.Idle), 3f);
    }
}
