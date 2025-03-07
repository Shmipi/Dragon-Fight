using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBackgroundCanvas : MonoBehaviour
{
    private Image gameBackground;

    [SerializeField] Sprite[] sprites;

    void Start()
    {
        gameBackground = GetComponent<Image>();
        int index = Random.Range(0, sprites.Length);

        gameBackground.sprite = sprites[index];
    }

}
