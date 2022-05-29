using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundFXController
{
    /*
    public enum Sound //Holds the referance to the audioclips through GameAssets
    {
        BackgroundMusik,
        PlayerMove,
        PlayerAttack,
    }

    private static Dictionary<Sound, float> soundTimerDictionary; //Timmer for making sound play under certain time
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    private static void Initialize() // used to initialize Dictionary. Should be called in "Awake" when game starts through a GameHandler
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerMove] = 0f;
    }

    public static void PlaySound(Sound sound) //choses wich sound should be played
    {
        if (CanPlaySound(sound)){
            if (oneShotGameObject == null){
                oneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>(); //this komponent plays the Audio
            }

            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));  // funktion that choses how the sound should be played
        }
    }


    private static bool CanPlaySound(Sound sound) //checks if sound chould be played normal or with delay
    {
        switch (sound)
        {
            default:
                return true; // most sounds will be played normaly, thus only specifik sounds will be fals
            case Sound.PlayerMove: // can use for several sounds such as PlayerAttack etc..
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = .05f; // time that is delayed
                    if(lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else { return false;}
                }
                else { return true;}
        }
    }


    //finds the right audioclip to be played by the PlaySound class
    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)  //cycles through the array
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;  
            } 
        }
        Debug.LogError("Sound " + sound + " not found");
        return null;
    }  
    
    //To use insert: SoundFXController.PlaySound(SoundFXController.Sound.NameOfSound);

    */
}