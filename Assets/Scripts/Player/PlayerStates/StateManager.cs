using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    private Dictionary<PlayerStateEnum, State> stateDictionary = new Dictionary<PlayerStateEnum, State>();

    public StateManager()
    {
        // Initialize and store states in the dictionary
        stateDictionary[PlayerStateEnum.Spawn] = new SpawnState();
        stateDictionary[PlayerStateEnum.Idle] = new IdleState();
        stateDictionary[PlayerStateEnum.MoveUp] = new MoveUpState();
        stateDictionary[PlayerStateEnum.MoveDown] = new MoveDownState();
        stateDictionary[PlayerStateEnum.AttackF] = new AttackFState();
        stateDictionary[PlayerStateEnum.AttackB] = new AttackBState();
        stateDictionary[PlayerStateEnum.Hurt] = new HurtState();
        stateDictionary[PlayerStateEnum.Win] = new WinState();
        stateDictionary[PlayerStateEnum.Lose] = new LoseState();
    }

    public State ResumeState(PlayerStateEnum playerState)
    {
        switch (playerState)
        {
            case PlayerStateEnum.MoveUp:
            case PlayerStateEnum.MoveDown:
                return GetState(PlayerStateEnum.MoveDown);

            case PlayerStateEnum.AttackF:
            case PlayerStateEnum.AttackB:
                return GetState(PlayerStateEnum.AttackB);
        }

        return GetState(PlayerStateEnum.Idle);
    }

    public State MirrorState(PlayerStateEnum playerState)
    {
        switch (playerState)
        {
            case PlayerStateEnum.MoveUp:
                return GetState(PlayerStateEnum.MoveDown);

            case PlayerStateEnum.MoveDown:
                return GetState(PlayerStateEnum.MoveUp);

            case PlayerStateEnum.AttackF:
                return GetState(PlayerStateEnum.AttackB);

            case PlayerStateEnum.AttackB:
                return GetState(PlayerStateEnum.AttackF);
        }

        return null;
    }

    public State GetState(PlayerStateEnum playerState)
    {
        if (!stateDictionary.TryGetValue(playerState, out State state))
        {
            throw new Exception($"State {playerState} not found in StateManager.");
        }

        return state;
    }

    public void ResetStates()
    {
        foreach (State state in stateDictionary.Values)
        {
            state.AnimationExitTime = 0f;
        }
    }
}
