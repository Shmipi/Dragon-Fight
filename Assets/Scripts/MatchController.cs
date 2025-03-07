using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchController : MonoBehaviour
{
    private const string WIN = " Wins!";
    private const string READY_MSG = "Ready for battle!";
    private const string GO = "Go!";
    private const string PLAYER1 = "Player 1";
    private const string PLAYER2 = "Player 2";
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Text winText;
    [SerializeField] private Countdown countdown;

    [SerializeField] private GameObject restartButton;

    private PlayerBehaviour player1;
    [SerializeField] private GameObject player1UI;
    private PlayerUIManager player1UIManager;


    private PlayerBehaviour player2;
    [SerializeField] private GameObject player2UI;
    private PlayerUIManager player2UIManager;

    private Coroutine checkForWinner;

    //private AIMove ai_move;
    //private AIAttack ai_attack;
    private AIController ai_controller;

    private void Start()
    {
        player1UIManager = player1UI.GetComponent<PlayerUIManager>();
        player2UIManager = player2UI.GetComponent<PlayerUIManager>();

        StartMatch();
    }

    public void StartMatch()
    {
        StartCoroutine(MatchStartTimer());
    }

    private IEnumerator MatchStartTimer()
    {
        yield return WaitFor.Frames(5);
        countdown.gameObject.SetActive(true);
        countdown.StartCountDown();
        winPanel.SetActive(false);
        restartButton.SetActive(false);
        player1UI.SetActive(true);
        player2UI.SetActive(true);

        player1UIManager.ResetHP();
        player2UIManager.ResetHP();

        player1.ResetPlayer();
        player2.ResetPlayer();

        checkForWinner = null;

        if (!GameMode.IsPVP)
        {
            RestartAI();
        }
    }

    public void OnCountDownFinished()
    {
        countdown.gameObject.SetActive(false);
    }

    private void MatchFinished(Player? player) // Accepts null for no winner
    {
        winPanel.SetActive(true);

        if (player == null)
        {
            player1.Lose();
            player2.Lose();
            winText.text = "Draw! No Winner!";
        }
        else
        {
            switch (player)
            {
                case Player.Player1:
                    player1.Win();
                    player2.Lose();
                    winText.text = PLAYER1 + WIN;
                    break;

                case Player.Player2:
                    player2.Win();
                    player1.Lose();
                    winText.text = PLAYER2 + WIN;
                    break;
            }
        }

        player1UI.SetActive(false);
        player2UI.SetActive(false);
        restartButton.SetActive(true);
    }

    public void OnDragonDead()
    {
        if(checkForWinner == null)
        {
            checkForWinner = StartCoroutine(CheckForWinnerAfterDelay());
        }
    }

    private IEnumerator CheckForWinnerAfterDelay()
    {
        yield return WaitFor.Frames(5); // Wait 5 frames

        if (player1.GetIsDead() && player2.GetIsDead())
        {
            MatchFinished(null); 
        }

        else
        {
            // The surviving dragon is the winner
            PlayerBehaviour winner = (player1.GetIsDead()) ? player2 : player1;
            MatchFinished(winner.player);
        }
    }

    public void SetPlayers(PlayerBehaviour p1, PlayerBehaviour p2)
    {
        player1 = p1;
        player2 = p2;
    }

    private void RestartAI()
    {
        ai_controller.ResetDelay();
        //ai_attack.ResetDelay();
        //ai_move.ResetDelay();
    }

    public void SetAI(AIController ai_controller)
    {
        this.ai_controller = ai_controller;
    }

    public void SetAI(AIMove ai_move, AIAttack ai_attack)
    {
        //this.ai_move = ai_move;
        //this.ai_attack = ai_attack;
    }    
}
