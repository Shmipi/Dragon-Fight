using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFState : State
{
    public AttackFState()
    {
        playerState = PlayerStateEnum.AttackF;
        animation = Animation.Attack;
        animationLabel = SpriteLabel.OpenMouth;
        animationDuration = 0.75f;
        animationLayer = 2;
        animationSpeed = 1f / animationDuration;
        animationID = Animator.StringToHash("AnimAttackLayerSpeed");
        animationHash = Animator.StringToHash(animation.ToString());
    }
}
