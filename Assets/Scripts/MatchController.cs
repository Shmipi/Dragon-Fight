using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchController : MonoBehaviour
{

    [SerializeField] private GameObject winPanel;
    [SerializeField] private Text winText;

    [SerializeField] private GameObject restartButton;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player1UI;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject player2UI;

    private void Awake()
    {     
        MatchStart();
    }

    public void MatchStart()
    {
        StartCoroutine(StartGame());
        winPanel.SetActive(false);
        restartButton.SetActive(false);
        player1UI.SetActive(true);
        player2UI.SetActive(true);

        try
        {
            player1.gameObject.GetComponent<PlayerBehaviour>().ResetPlayer();
            player2.gameObject.GetComponent<PlayerBehaviour>().ResetPlayer();
        }

        catch
        {

        }
        player1.gameObject.GetComponent<PlayerBehaviour>().healthBar.ResetHP();
        player2.gameObject.GetComponent<PlayerBehaviour>().healthBar.ResetHP();
    }

    public IEnumerator StartGame()
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

    public void MatchFinished(string dragon)
    {
        winPanel.SetActive(true);        
        if(dragon == "Player1")
        {
            player2.gameObject.GetComponent<PlayerBehaviour>().Win();
            winText.text = "Player 2" + " Wins!";

        }
        else
        {
            player1.gameObject.GetComponent<PlayerBehaviour>().Win();
            winText.text = "Player 1" + " Wins!";
        }
        player1UI.SetActive(false);
        player2UI.SetActive(false);
        restartButton.SetActive(true);
    }

    /*
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
    */

}
