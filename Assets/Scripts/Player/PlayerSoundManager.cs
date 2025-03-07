using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{

    private AudioSource audioSource;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip hurtClip;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();  
        player = GetComponent<PlayerBehaviour>().player;
        PanSource();
    }

    private void PanSource()
    {
        switch (player)
        {
            case Player.Player1:
                audioSource.panStereo = -0.75f;
                break;
            case Player.Player2:
                audioSource.panStereo = 0.75f;
                break;
        }
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackClip);
    }

    public void PlayHurtSound()
    {
        audioSource.PlayOneShot(hurtClip);
    }
}
