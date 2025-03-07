using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMode
{
    private static bool isPVP = true;

    public static bool IsPVP { get => isPVP; set => isPVP = value; }

    private const int PVP_SCENE_INDEX = 2;
    private const int AI_SCENE_INDEX = 3;

    public static int GetSceneIndex()
    {
        return isPVP ? PVP_SCENE_INDEX : AI_SCENE_INDEX;
    }
}
