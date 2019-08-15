using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIStreamVideo : MonoBehaviour {
    public RawImage rawImage;
    public VideoPlayer videoPlayer;

    // Use this for initialization
    void Start() {
        videoPlayer.Prepare();
        videoPlayer.isLooping = true;
        videoPlayer.prepareCompleted += OnPreparationFinished;


        //        videoPlayer.Prepare();
        //call back startcoroutine
    }

    void OnPreparationFinished(UnityEngine.Video.VideoPlayer vPlayer)
    {
        //vPlayer.Play();
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }

        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }
}
