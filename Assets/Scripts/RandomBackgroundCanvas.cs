using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBackgroundCanvas : MonoBehaviour
{
    public Image gameBackground;

    public Sprite[] Sprites;

    public int x;

    void Start()
    {
        x = Random.Range(0, 3);

        gameBackground.sprite = Sprites[x];
    }

}
