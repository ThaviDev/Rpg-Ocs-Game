using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScOw_Motor : MonoBehaviour
{
    public int OwScene; // la escena actual en la que 
    public GameObject interactableObjs; // Objetos interactuables en la escena: Personajes y Localizaciones
    //public GameObject exit;
    public LocationCol exitCol = null;
    public PlayerMotor_OW player;
    public Camera cam;

    private bool canExit;
    void Start()
    {
        canExit = true;
        GameObject locations;
        GameObject exit;
        interactableObjs = GameObject.Find("Interactable Objects");
        print(interactableObjs);
        locations = interactableObjs.gameObject.transform.Find("Locations").gameObject;

        player = FindObjectOfType<PlayerMotor_OW>();
        cam = FindObjectOfType<Camera>();

        try
        {
            exit = locations.gameObject.transform.Find("Exit").gameObject;
        } catch
        {
            canExit = false;
            print("There is no exit");
        }
        if (canExit)
        {
            exit = locations.gameObject.transform.Find("Exit").gameObject;
            exitCol = exit.GetComponent<LocationCol>();
        }
    }


    void Update()
    {

        if (exitCol.coliderCheck == true)
        {
            player.transform.position = new Vector2(3, 1);
            cam.transform.position = new Vector2(3, 1);
            SceneManager.LoadScene(3, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(2);
            exitCol.coliderCheck = false;
        }

    }
}
