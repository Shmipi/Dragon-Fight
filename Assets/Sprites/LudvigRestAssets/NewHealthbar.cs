using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHealthbar : MonoBehaviour
{

    [SerializeField] private GameObject health1;
    [SerializeField] private GameObject health2;
    [SerializeField] private GameObject health3;
    [SerializeField] private GameObject health4;
    [SerializeField] private GameObject health5;

    private bool hp1;
    private bool hp2;
    private bool hp3;
    private bool hp4;
    private bool hp5;

    private void Awake()
    {
        hp1 = true;
        hp2 = true;
        hp3 = true;
        hp4 = true;
        hp5 = true;

        health1.SetActive(true);
        health2.SetActive(true);
        health3.SetActive(true);
        health4.SetActive(true);
        health5.SetActive(true);
    }

    public void ReduceHealth()
    {
        if (hp1 == true)
        {
            health1.SetActive(false);
            hp1 = false;
        } else if (hp2 == true)
        {
            health2.SetActive(false);
            hp2 = false;
        } else if (hp3 == true)
        {
            health3.SetActive(false);
            hp3 = false;
        } else if (hp4 == true)
        {
            health4.SetActive(false);
            hp4 = false;
        } else
        {
            health5.SetActive(false);
            hp5 = false;
        }
    }


    public void ResetHP()
    {
        health1.SetActive(true);
        health2.SetActive(true);
        health3.SetActive(true);
        health4.SetActive(true);
        health5.SetActive(true);

        hp1 = true;
        hp2 = true;
        hp3 = true;
        hp4 = true;
        hp5 = true;
    }
}
