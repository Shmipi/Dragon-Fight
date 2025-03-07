using System;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPlayers : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject redDragonPrefab;
    [SerializeField] private GameObject greenDragonPrefab;
    [SerializeField] private GameObject brownDragonPrefab;
    [SerializeField] private GameObject greyDragonPrefab;

    [Header("Player Transform")]
    [SerializeField] private Transform player1Transform;
    [SerializeField] private Transform player2Transform;

    [Header("Player UI")]
    [SerializeField] private PlayerUIManager player1UIManager;
    [SerializeField] private PlayerUIManager player2UIManager;

    [Header("Match Controller")]
    [SerializeField] private MatchController matchController;

    [Header("AI Controller")]
    [SerializeField] private GameObject ai_controller_obj;

    private readonly Dictionary<DragonSkin, GameObject> dragons = new Dictionary<DragonSkin, GameObject>(4);

    private readonly Player player1 = Player.Player1;
    private readonly Player player2 = Player.Player2;

    private void Awake()
    {
        InitializeList();
        InstantiatePlayers();
    }

    private void InitializeList()
    {
        dragons[DragonSkin.RedMain] = redDragonPrefab;
        dragons[DragonSkin.GreenMain] = greenDragonPrefab;
        dragons[DragonSkin.BrownMain] = brownDragonPrefab;
        dragons[DragonSkin.GreyMain] = greyDragonPrefab;
    }

    private void InstantiatePlayers()
    {
        DragonSkin player1DragonSkin = SelectDragon.Player1Skin;
        DragonSkin player2DragonSkin = SelectDragon.Player2Skin;

        PlayerBehaviour p1 = InitializePlayer(player1DragonSkin, player1Transform, player1UIManager, player1);
        PlayerBehaviour p2 = InitializePlayer(player2DragonSkin, player2Transform, player2UIManager, player2);

        matchController.SetPlayers(p1, p2);

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            GameMode.IsPVP = false;
        }

        if (!GameMode.IsPVP)
        {
            InstantiateAI(p2);
        }
    }

    private PlayerBehaviour InitializePlayer(DragonSkin playerDragonSkin, Transform transform, PlayerUIManager playerUI, Player player)
    {
        GameObject playerObj = Instantiate(dragons[playerDragonSkin], transform);
        PlayerBehaviour playerBehaviour = playerObj.GetComponent<PlayerBehaviour>();
        playerBehaviour.player = player;
        playerBehaviour.MatchController = matchController;
        playerBehaviour.PlayerUIManager = playerUI;
        playerUI.SetPlayer(playerBehaviour);
        return playerBehaviour;
    }

    private void InstantiateAI(PlayerBehaviour p2)
    {

        /*
        AIAttack ai_attack = ai_controller_obj.GetComponent<AIAttack>();
        ai_attack.PlayerBehaviour = p2;

        AIMove ai_move = ai_controller_obj.GetComponent<AIMove>();
        ai_move.PlayerBehaviour = p2;

        matchController.SetAI(ai_move, ai_attack);
        */

        GameObject ai_controller_obj = Instantiate(this.ai_controller_obj, player2Transform);
        AIController ai_controller = ai_controller_obj.GetComponent<AIController>();

        ai_controller.PlayerBehaviour = p2;
        p2.SetAi_controller(ai_controller);

        matchController.SetAI(ai_controller);

    }
}

