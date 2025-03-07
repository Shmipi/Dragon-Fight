using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_PlayLevel : MonoBehaviour
{
    private const int SELECT_PLAYER_SCENE_INDEX = 1;

    public void PlayLevelOne()
    {
        GameMode.IsPVP = true;
        SceneManager.LoadScene(SELECT_PLAYER_SCENE_INDEX);
    }
    public void PlayLevelTwo()
    {
        GameMode.IsPVP = false;
        SceneManager.LoadScene(SELECT_PLAYER_SCENE_INDEX);
    }
}
