using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScript : MonoBehaviour
{
    [SerializeField] private MatchController matchController;

    public void RestartGame()
    {
        matchController.StartMatch();
    }
}
