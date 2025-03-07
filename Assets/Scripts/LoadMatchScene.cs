using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMatchScene : MonoBehaviour
{
    public void LoadNextScene()
    {
        int nextSceneIndex = GameMode.GetSceneIndex();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
