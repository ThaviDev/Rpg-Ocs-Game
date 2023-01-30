using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    // VARIABLES
    Scene myScene;
    bool changeScene;
    int currentScene;

    // CODIGOS
    Scene_Manager sceneMan;
    Camera myCamera;
    PauseManager pauseManager;
    MyDialogueManager myDialogueManager;

    MenuMotor menuMotor;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        // Encontrar todos los objetos de la base del juego
        sceneMan = FindObjectOfType<Scene_Manager>();
        myCamera = Camera.main;
        pauseManager = FindObjectOfType<PauseManager>();

        // Destruye la escena con todos los objetos y códigos indestructibles para no molestar con una escena demás que no tiene nada
        SceneManager.UnloadSceneAsync("Game Base");

        // Obtener la escena activa en ese momento
        myScene = SceneManager.GetActiveScene();
        print("El nombre de esta escena es: " + myScene.name);

        CheckScene();
    }

    // Esta función sirve para estar checando que escena está cargada, y una vez que sabe eso, que códigos necesita encontrar, además de que otras funciones puede acceder
    void CheckScene()
    {
        myScene = SceneManager.GetActiveScene();
        switch (myScene.name)
        {
            case "Main Menu":
                menuMotor = FindObjectOfType<MenuMotor>();
                menuMotor.cam = myCamera;
                break;
        }
    }
    // Envía codigo a Scene Manager para cambiar la escena
    public void Output_SceneManager_ChangeScene(string sceneName)
    {
        // Pasar el nombre de la escena que cambiará y que se pueda cambiar
        sceneMan.sceneID = sceneName;
        sceneMan.changeScene = true;
    }

    public void Input_SceneLoaded()
    {
        print("MUEVO LA CAMARA");
        myCamera.GetComponent<CameraManager_OW>().canFollowPlayer = false;
        myCamera.gameObject.transform.position = new Vector3(0f, 0f, -10f);
        // Checar en que escena estamos
        CheckScene();
    }

    public void Input_BattleManager(bool isVictory, 
        bool isDefeat)
    {

    }


}
