using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelAnimation : MonoBehaviour
{

    [SerializeField] private Sprite frame1;
    [SerializeField] private Sprite frame2;
    [SerializeField] private Sprite frame3;

    [SerializeField] private float animateTime = 3f;

    private bool animate1;
    private bool animate2;
    private bool animate3;

    private void Awake()
    {
        animate1 = true;
        MyAnimator();
    }

    private void MyAnimator()
    {
        if (animate1 == true)
        {
            gameObject.GetComponent<Image>().sprite = frame1;
            animate1 = false;
            animate2 = true;
            Invoke("MyAnimator", animateTime);
        } else if (animate2 == true)
        {
            gameObject.GetComponent<Image>().sprite = frame2;
            animate2 = false;
            animate3 = true;
            Invoke("MyAnimator", animateTime);
        } else if (animate3 == true)
        {
            gameObject.GetComponent<Image>().sprite = frame3;
            animate3 = false;
            animate1 = true;
            Invoke("MyAnimator", animateTime);
        }

    }
}
