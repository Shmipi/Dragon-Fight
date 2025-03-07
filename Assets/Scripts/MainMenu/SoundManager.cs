using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SoundManager : MonoBehaviour
{
    private const string gameVolume = "gameVolume";
    [SerializeField] private Slider volumeSlider; 

    
    void Start()
    {
        if (!PlayerPrefs.HasKey(gameVolume))
        {
            PlayerPrefs.SetFloat(gameVolume, 100);
            Load();
        }
        else
        {
            Load();
        }
    }

    //Changes the game volume
    public void ChangeVolume() 
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    //Loads the previous settings when you start the game 
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(gameVolume); 
    }

    //Saves the volume setting  
    private void Save()
    {
        PlayerPrefs.SetFloat(gameVolume, volumeSlider.value); 
    }
}
