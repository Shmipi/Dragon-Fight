using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{

    [SerializeField] private GameObject winPanel;
    [SerializeField] private Text winText;

    [SerializeField] private HealthTest player1;
    [SerializeField] private HealthTest player2;

    [SerializeField] private Button p1Attack;
    [SerializeField] private Button p1Move;
    [SerializeField] private Button p2Attack;
    [SerializeField] private Button p2Move;

    // Start is called before the first frame update
    void Start()
    {
        winPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(player1.currentHealth < 1) {

            winPanel.SetActive(true);
            winText.text = "GREEN DRAGON WINS!";
            p1Attack.interactable = false;
            p1Move.interactable = false;
            p2Attack.interactable = false;
            p2Move.interactable = false;
        } else if (player2.currentHealth < 1) {
            winPanel.SetActive(true);
            winText.text = "RED DRAGON WINS!";
            p1Attack.interactable = false;
            p1Move.interactable = false;
            p2Attack.interactable = false;
            p2Move.interactable = false;
        }
    }
}
