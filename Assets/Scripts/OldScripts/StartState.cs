using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : State
{
    public StartState(BattleSystem battleSystem) : base(battleSystem)
    {

    }

    public override IEnumerator Start()
    {
        BattleSystem.print("Ready for battle!");
        MonoBehaviour.print("3");
        yield return new WaitForSeconds(1f);
        BattleSystem.print("2");
        yield return new WaitForSeconds(1f);
        BattleSystem.print("1");
        yield return new WaitForSeconds(1f);
        BattleSystem.print("Go!");
        battleSystem.SetState(new StandByState(battleSystem));

    }
}
