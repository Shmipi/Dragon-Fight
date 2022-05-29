using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBackground : MonoBehaviour
{
    public SpriteRenderer GameBackground;

    public Sprite[] Sprites;

    public int x; 

    // Start is called before the first frame update
    void Start()
    {
        x = Random.Range(0, 3);

        GameBackground.sprite = Sprites[x];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
