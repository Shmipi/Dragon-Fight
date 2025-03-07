using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public abstract class State
{
    protected PlayerStateEnum playerState;
    protected Animation animation;
    protected SpriteLabel animationLabel;
    protected float animationDuration;
    private float animationExitTime;
    protected int animationLayer;
    protected bool loopAnimation;
    protected float animationSpeed = 1f;
    protected int animationID = Animator.StringToHash("AnimMainLayerSpeed");
    protected int animationHash;

    public PlayerStateEnum PlayerState { get => playerState; }
    public Animation Animation { get => animation; }
    public SpriteLabel AnimationLabel { get => animationLabel; }
    public float AnimationDuration { get => animationDuration; }
    public int AnimationLayer { get => animationLayer;}
    public float AnimationExitTime { get => animationExitTime; set => SetAnimationExitTime(value); }
    public float AnimationSpeed { get => animationSpeed;}
    public int AnimationID { get => animationID;}
    public int AnimationHash { get => animationHash; set => animationHash = value; }

    public State()
    {

    }

    public override bool Equals(object obj)
    {
        if (obj is State otherState)
        {
            return this.playerState == otherState.playerState;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return playerState.GetHashCode();
    }

    private void SetAnimationExitTime(float value)
    {
        if(value < 0f)
        {
            animationExitTime = 0f;
        }

        if(loopAnimation)
        {
            value %= animationDuration;
            //Debug.Log("modulated animation: " + value);
            animationExitTime = value;
        }

        else
        {
            //Debug.Log (playerState.ToString() + " value was: " + value);
            value = Mathf.Min(animationDuration, value);
            //Debug.Log(playerState.ToString() + " min value: " + value);
            animationExitTime = value;
            //Debug.Log(playerState.ToString() + " exit time set to: " +  animationExitTime);
        }

    }

    public override string ToString()
    {
        return playerState.ToString();
    }
}
