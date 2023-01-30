using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuMotor : MonoBehaviour
{
    Game_Manager Gmanager;
    GameObject canvasObj;
    // Camara de Game Manager
    public Camera cam;
    void Start() { 
        //DontDestroyOnLoad(this.gameObject);
        canvasObj = GameObject.Find("UI Menu");
        Gmanager = FindObjectOfType<Game_Manager>();
    }
    void Update() {}

    public void Button (int value){
        switch (value)
        {
            case 0:
                // Iniciar la escena de la cinematica
                canvasObj.transform.GetChild(0).gameObject.SetActive(false);
                CinematicManager cinematic = FindObjectOfType<CinematicManager>();
                cinematic.myCamera = cam;
                cinematic.canPlay = true;
                break;
            case 1:
                // Salir de la aplicación
                Application.Quit();
                break;
        }
    }

    // llamar a Game Manager para cargar la siguiente escena MenuMotor/GameManager/SceneManager/SceneLoader
    public void CallGM_ToChangeScene()
    {
        Gmanager.Output_SceneManager_ChangeScene("Lobby");
    }

}
