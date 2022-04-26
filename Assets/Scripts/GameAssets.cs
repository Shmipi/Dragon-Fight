using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    //This class makes it possible to referance Assets through code
    //The sound asset has to be in a folder called ***"Resources"*** 
    //Just attachthe code to a GameObject that is a prefsb and drag said Asset into the open field.

    private static GameAssets _i;

    public static GameAssets i {
        get {
            if (_i == null) 
                _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i; 
        }
    }

    //Insert what type of Asset you whant to referance in Unity
    // example: public Sprite nameOfSprite;

    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]   
    public class SoundAudioClip //Stores Audio referances.
    {
        public SoundFXController.Sound sound;
        public AudioClip audioClip;

    }

}
