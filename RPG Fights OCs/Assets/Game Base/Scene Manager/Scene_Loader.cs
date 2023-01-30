using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Scene_Loader
{
    // Este carga una escena sencilla, como una escena de combate
    public static void LoadScene (string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    /*
    // Este carga un nuevo mapa para las escenas de exploracion
    public static void LoadMap (string mapToLoad, string mapToUnload)
    {
        SceneManager.LoadSceneAsync(mapToLoad, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(mapToUnload);
    }

    // Este carga de una escena sencilla a una de exploración para añadir la base de la exploración
    public static void LoadExplorationScene (string mapToLoad)
    {
        // Carga la base de la exploración (SceneIndex : 1)
        SceneManager.LoadSceneAsync(1);
        SceneManager.LoadSceneAsync (mapToLoad, LoadSceneMode.Additive);
    }
    */
}
