using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHealthbar : MonoBehaviour
{
    [SerializeField] private GameObject[] healthBar;

    private int currentHealthIndex = 0;

    private void Awake()
    {
        for (int i = 0; i < healthBar.Length; i++)
        {
            healthBar[i].SetActive(true);
        }
    }

    public void ReduceHealth()
    {
        if(currentHealthIndex < healthBar.Length)
        {
            healthBar[currentHealthIndex++].SetActive(false);
        }
    }


    public void ResetHP()
    {
        for(int i = 0;i < healthBar.Length; i++)
        {
            healthBar[i].SetActive(true);
            currentHealthIndex = 0;
        }
    }
}
