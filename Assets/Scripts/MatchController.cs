using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchController : MonoBehaviour
{

    [SerializeField] private GameObject winPanel;
    [SerializeField] private Text winText;

    [SerializeField] private HealthTest player1Health;
    [SerializeField] private HealthTest player2Health;

    [SerializeField] private Button p1Attack;
    [SerializeField] private Button p1Move;
    [SerializeField] private Button p2Attack;
    [SerializeField] private Button p2Move;

    [SerializeField] private GameObject restartButton;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private bool onGoingGame;

    private void Awake()
    {
        MatchStart();
    }

    public void MatchStart()
    {
        StartCoroutine(StartGame());
        winPanel.SetActive(false);
        restartButton.SetActive(false);
        player1Health.ResetHealth();
        player2Health.ResetHealth();
        p1Attack.interactable = true;
        p1Move.interactable = true;
        p2Attack.interactable = true;
        p2Move.interactable = true;
    }

    private IEnumerator StartGame()
    {
        print("Ready for battle!");
        print("3");
        yield return new WaitForSeconds(1f);
        print("2");
        yield return new WaitForSeconds(1f);
        print("1");
        yield return new WaitForSeconds(1f);
        player1.gameObject.GetComponent<PlayerBehaviour>().StartSpawn();
        player2.gameObject.GetComponent<PlayerBehaviour>().StartSpawn();
        print("Go!");
       
    }

    private void Update()
    {
        if (player1Health.currentHealth < 1)
        {

            winPanel.SetActive(true);
            winText.text = "GREEN DRAGON WINS!";
            p1Attack.interactable = false;
            p1Move.interactable = false;
            p2Attack.interactable = false;
            p2Move.interactable = false;
            restartButton.SetActive(true);
        }
        else if (player2Health.currentHealth < 1)
        {
            winPanel.SetActive(true);
            winText.text = "RED DRAGON WINS!";
            p1Attack.interactable = false;
            p1Move.interactable = false;
            p2Attack.interactable = false;
            p2Move.interactable = false;
            restartButton.SetActive(true);
        }
    }

}
