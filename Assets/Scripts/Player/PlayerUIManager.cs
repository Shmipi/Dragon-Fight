using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    private PlayerBehaviour player;
    [SerializeField] private MoveButton moveButton;
    [SerializeField] private AttackButton attackButton;
    [SerializeField] private NewHealthbar healthbar;

    private void Start()
    {
        if(!GameMode.IsPVP && player.player == Player.Player2)
        {
            Button moveButton = this.moveButton.GetComponent<Button>();
            Button attackButton = this.attackButton.GetComponent<Button>();

            moveButton.interactable = false;
            attackButton.interactable = false;

            this.moveButton.GetComponent<Image>().raycastTarget = false;
            this.attackButton.GetComponent <Image>().raycastTarget = false;

        }
    }
        public PlayerBehaviour GetPlayer()
    {
        return player;
    }

    public void SetPlayer(PlayerBehaviour value)
    {
        player = value;
        moveButton.SetPlayer(value);
        attackButton.SetPlayer(value);
        value.PlayerUIManager = this;
    }

    public void ReduceHealth()
    {
        healthbar.ReduceHealth();
    }

    public void ResetHP()
    {
        healthbar.ResetHP();
    }
}
