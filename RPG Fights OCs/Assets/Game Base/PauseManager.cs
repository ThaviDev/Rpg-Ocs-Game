using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    bool isPaused;

    CanvasGroup myCanvasGroup;
    Game_Manager gMan;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        // Encontrar Game Manager
        gMan = FindObjectOfType<Game_Manager>();

        // Encontrar El grupo de canvas
        myCanvasGroup = this.transform.Find("Pause Canvas").transform.Find("Panel").GetComponent<CanvasGroup>();

        myCanvasGroup.alpha = 0.0f;
        myCanvasGroup.interactable = false;
        myCanvasGroup.blocksRaycasts = false;
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("Pause Manager Escape Key Pressed");
            TriggerActivePause();
        }
        print(isPaused);
    }

    void TriggerActivePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            myCanvasGroup.alpha = 1.0f;
            myCanvasGroup.interactable = true;
            myCanvasGroup.blocksRaycasts = true;
            Time.timeScale = 0.0f;
        } else
        {
            myCanvasGroup.alpha = 0.0f;
            myCanvasGroup.interactable = false;
            myCanvasGroup.blocksRaycasts = false;
            Time.timeScale = 1.0f;
        }
    }

    public void Button (int value)
    {
        switch (value)
        {
            case 0:
                // Renaudar
                TriggerActivePause();
                break;
            case 1:
                // Opciones
                // Cambiar panel a de opciones
                break;
            case 2:
                // Salir al menú principal
                // Llamar a gMan
                break;
        }
    }

}