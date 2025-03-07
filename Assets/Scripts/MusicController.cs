using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip menuMusicClip;
    [SerializeField] private AudioClip combatMusicClip;
    private AudioClip currentClip;
    private const int MAIN_MENU_INDEX = 0;
    private const int SELECT_PLAYER_INDEX = 1;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("1");
        if (PersistantSceneManager.GetMusicController() == null)
        {
            PersistantSceneManager.SetMusicController(gameObject);
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.Play();
            DontDestroyOnLoad(this);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }


    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentClip = SetMusicClip(scene.buildIndex);

        if(audioSource != null)
        {
            if(audioSource.clip != currentClip)
            {
                audioSource.clip = currentClip;
                audioSource.Play();
            }
        }
    }

    private AudioClip SetMusicClip(int currentSceneIndex)
    {

        if (currentSceneIndex == MAIN_MENU_INDEX || currentSceneIndex == SELECT_PLAYER_INDEX)
        {
            return menuMusicClip;
        }

        else
        {
            return combatMusicClip;
        }
    }
}
