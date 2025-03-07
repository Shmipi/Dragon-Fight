using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    private const int MAIN_MENU_INDEX = 0;
    
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(MAIN_MENU_INDEX);
    }
}
