using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CinematicManager : MonoBehaviour
{
    public VideoPlayer myVideoPlayer;
    public Camera myCamera;
    public MenuMotor menuMotor;

    // Checa si el video se puede reproducir, a través del GameManager/MenuMotor
    public bool canPlay;
    void Start()
    {
        //myVideoPlayer = GetComponent<VideoPlayer>();
        myVideoPlayer = gameObject.transform.Find("Video Cinematic").GetComponent<VideoPlayer>();
        menuMotor = FindObjectOfType<MenuMotor>();
    }

    void Update()
    {
        if (canPlay)
        {
            myVideoPlayer.targetCamera = myCamera;
            StartCoroutine("VideoTimeAmount");
            canPlay = false;
        }
    }

    IEnumerator VideoTimeAmount()
    {
        myVideoPlayer.Play();
        yield return new WaitForSeconds((float)myVideoPlayer.length);
        myVideoPlayer.Pause();
        // Llama a Game Manager para cambiar la escena
        menuMotor.CallGM_ToChangeScene();
        //SceneManager.LoadScene(1, LoadSceneMode.Additive);
        //SceneManager.LoadScene(2, LoadSceneMode.Additive);
        //yield return new WaitForSeconds(0.1f);
        //SceneManager.UnloadSceneAsync("Main Menu");
    }
}
