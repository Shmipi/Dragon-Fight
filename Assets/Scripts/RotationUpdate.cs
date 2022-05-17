using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationUpdate : MonoBehaviour
{

    private void Awake()
    {
        if(gameObject.tag == "Player1"){
            gameObject.transform.eulerAngles = new Vector3(0f,0f,0f);
        }
        else if(gameObject.tag == "Player2")
        {
            gameObject.transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
    }

    private void Update()
    {
        if (gameObject.tag == "Player1")
        {
            gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (gameObject.tag == "Player2")
        {
            gameObject.transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
    }
}
