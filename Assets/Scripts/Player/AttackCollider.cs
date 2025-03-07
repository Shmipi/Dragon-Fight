using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private Player player;
    private PlayerBehaviour playerBehaviour;

    public PlayerBehaviour PlayerBehaviour { get => playerBehaviour; set => playerBehaviour = value; }
    public bool HasBeenHurt => hasBeenHurt;

    private bool canTrigger = true;  
    private readonly float triggerCooldown = 0.5f;

    private bool hasBeenHurt = false;
    private Coroutine delayedForceReturnCoroutine;

    private void Start()
    {
        player = playerBehaviour.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canTrigger) return;

        if (collision.TryGetComponent(out NeckCollider neckCollider))
        {
            canTrigger = false;
            StartCoroutine(ResetTriggerCooldown());

            neckCollider.Hurt();
            string playerNumber = neckCollider.player.ToString();
            Debug.Log(player.ToString() + " atk hurting: " + playerNumber);

            if (delayedForceReturnCoroutine == null)
            {
                delayedForceReturnCoroutine = StartCoroutine(DelayedForceReturn());
            }

            return;
        }

        
        if(collision.TryGetComponent(out AttackCollider attackCollider))
        {
            canTrigger = false;
            StartCoroutine(ResetTriggerCooldown());
            Debug.Log(player + " Atk head with: " + attackCollider.PlayerBehaviour.player);
            playerBehaviour.ForceReturn();
            return;
        }
    }

    private IEnumerator ResetTriggerCooldown()
    {
        yield return new WaitForSeconds(triggerCooldown);
        canTrigger = true;
    }

    private IEnumerator DelayedForceReturn()
    {
        yield return WaitFor.Frames(1);

        if (!hasBeenHurt)
        {
            playerBehaviour.ForceReturn();
        }

        delayedForceReturnCoroutine = null;
    }

    public void SetHasBeenHurt()
    {
        hasBeenHurt = true;
        StartCoroutine(ResetHasBeenHurt());
    }

    private IEnumerator ResetHasBeenHurt()
    {
        yield return WaitFor.Frames(5);
        hasBeenHurt = false;
    }
}
