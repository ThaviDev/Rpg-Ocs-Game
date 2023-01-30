using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Manager : MonoBehaviour
{
    public bool changeScene;
    public string sceneID;

    public Animator transition;
    public Game_Manager Gman;
    public float transitionTime = 1f;
    void Start()
    {
        Gman = FindObjectOfType<Game_Manager>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (changeScene == true)
        {
            changeScene = false;
            // Empieza a animarse la transición
            StartCoroutine(TransicionToLoad());
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("Scene Manager Triggered Escape Key");
            //changeScene = true;
            //sceneID = "Main Menu";
        }
    }

    IEnumerator TransicionToLoad()
    {
        transition.SetBool("Start", true);
        //Time.timeScale = 0f;
        yield return new WaitForSeconds(transitionTime);
        //Time.timeScale = 1f;
        Gman.Input_SceneLoaded();
        transition.SetBool("Start", false);
        Scene_Loader.LoadScene(sceneID);
        // Load Scene
    }
}
