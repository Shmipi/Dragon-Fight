using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour
{

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private bool onGoingGame;

    private void Awake()
    {
        StartCoroutine(StartGame());
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

}
