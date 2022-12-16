using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuMotor : MonoBehaviour
{
    void Start() {}
    void Update() {}

    public void Button (int value){
        print(value);
        BtonFunction(value);
    }

    void BtonFunction(int value)
    {
        switch (value)
        {
            case 0:
                // Iniciar la escena de la cinematica
                this.transform.GetChild(0).gameObject.SetActive(false);
                CinematicManager cinematic = FindObjectOfType<CinematicManager>();
                cinematic.canPlay = true;
                break;
        }
    }

}
