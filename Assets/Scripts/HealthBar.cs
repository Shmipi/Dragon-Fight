using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Slider slider;

    private void Awake()
    {
        slider.minValue = 0;
    }

    public void SetMaxHealth(int health) {

        slider.maxValue = health;
        slider.value = health;

    }

    public void ReduceHealth() {

        slider.value -= 1;

    }


    public void ResetHP()
    {
        slider.value = slider.maxValue;
    }

}
