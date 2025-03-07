using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClickSound : MonoBehaviour
{
    private AudioSource audioSource;
    private const float minRndPitch = 0.95f;
    private const float maxRndPitch = 1.05f;
    private const float minRndVolume = 0.785f;
    private const float maxRndVoume = 0.825f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        audioSource.pitch = Random.Range(minRndPitch, maxRndPitch); 
        audioSource.volume = Random.Range(minRndVolume, maxRndVoume);
        audioSource.Play();
    }
}
