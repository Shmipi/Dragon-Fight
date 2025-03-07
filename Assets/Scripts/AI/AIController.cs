using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private enum AI_Action
    {
        Standby, HoldButton, ReleaseButton, Attack,
    }

    private AI_Action currentAction;

    private const float INITIAL_DELAY = 3f;
    

    private PlayerBehaviour playerBehaviour;
    public PlayerBehaviour PlayerBehaviour { get => playerBehaviour; set => playerBehaviour = value; }

    private float stateTimer; // General timer for state transitions

    private void Start()
    {
        stateTimer = INITIAL_DELAY;
    }

    private void Update()
    {
        if (stateTimer > 0f)
        {
            stateTimer -= Time.deltaTime;
            HandleCurrentAction();
        }

        else
        {
            SelectRandomAction();
        }
        
    }

    private void HandleCurrentAction()
    {
        switch (currentAction)
        {
            case AI_Action.HoldButton:
                playerBehaviour.MoveOnHoldButton();
                break;

            case AI_Action.ReleaseButton:
                playerBehaviour.MoveOnReleaseButton();
                break;

            case AI_Action.Attack:
                playerBehaviour.AttackButton();
                break;
        }
    }


    private void SelectRandomAction()
    {
        float randomValue = Random.value; 

        if (randomValue < 0.35f) 
        {
            TransitionToState(AI_Action.HoldButton, Random.Range(0.15f, 0.25f));
        }
        else if (randomValue < 0.90f) 
        {
            TransitionToState(AI_Action.ReleaseButton, Random.Range(0.1f, 0.15f));
        }
        else
        {
            TransitionToState(AI_Action.Attack, 1.25f);
        }
    }

    private void TransitionToState(AI_Action action, float delay)
    {
        currentAction = action;
        stateTimer = delay;
    }

    public void ResetDelay()
    {
        stateTimer = INITIAL_DELAY;
    }

    public void SetStandby()
    {
        TransitionToState(AI_Action.Standby, float.MaxValue);
    }

    public void ForceRelease()
    {
        TransitionToState(AI_Action.ReleaseButton, 0.2f);
    }
}
