using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PersistantSceneManager
{
    private static GameObject musicController;

    public static GameObject GetMusicController()
    {
        return musicController;
    }

    public static void SetMusicController(GameObject value)
    {
        if(value != null || musicController == null)
        {
            musicController = value;
        }
    }
}
