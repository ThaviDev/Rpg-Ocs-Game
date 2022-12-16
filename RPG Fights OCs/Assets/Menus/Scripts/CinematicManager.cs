using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class CinematicManager : MonoBehaviour
{
    public VideoPlayer myVideoPlayer;
    // Checa si el video se puede reproducir, a través del Menu Motor
    public bool canPlay;
    // Start is called before the first frame update
    void Start()
    {
        myVideoPlayer = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlay)
        {
            StartCoroutine("VideoTimeAmount");
            canPlay = false;
        }
    }

    IEnumerator VideoTimeAmount()
    {
        myVideoPlayer.Play();
        yield return new WaitForSeconds((float)myVideoPlayer.length);
        myVideoPlayer.Pause();
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        yield return new WaitForSeconds(0.1f);
        SceneManager.UnloadSceneAsync("Main Menu");
    }
}
