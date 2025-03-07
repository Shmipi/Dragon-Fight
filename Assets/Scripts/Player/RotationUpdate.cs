using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationUpdate : MonoBehaviour
{
    public Player player;
    private Quaternion targetRotation;


    private void Start()
    {
        switch (player)
        {
            case Player.Player1:
                targetRotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case Player.Player2:
                targetRotation = Quaternion.Euler(0f, 0f, 180f);
                break;
        }
    }

    private void FixedUpdate()
    {
        transform.rotation = targetRotation;
    }
}
