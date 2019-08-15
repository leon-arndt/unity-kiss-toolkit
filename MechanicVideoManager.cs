using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

/// <summary>
/// Scene component which works together with the MechanicVideoData scriptable object.
/// Uses the UIStreamVideo component to play videos.
/// This component also handles UI and Input.
/// This was done to prevent the playerController and UiController scripts from growing.
/// </summary>
public class MechanicVideoManager : MonoBehaviour {
    public bool tutorialVisible;

    [SerializeField]
    GameObject uiGroup;

    [SerializeField]
    Text titleText, descText;

    [SerializeField]
    UIStreamVideo uIStreamVideo;

    [SerializeField]
    Animation m_animation;

    [SerializeField]
    AnimationClip bouncyHideClip;

    private MechanicVideoData mechanicVideoData;
    private Translator translator;

    Player player;

    // Use this for initialization
    void Start () {
        //player = ReInput.players.SystemPlayer;
        player = ReInput.players.GetPlayer(0);
        uiGroup.SetActive(false);

        if (GameManager.instance.activeLevel.mechanicVideo != null)
        {
            mechanicVideoData = GameManager.instance.activeLevel.mechanicVideo;
            LoadVideo();
            ShowVideo();
            //open UI
        }
    }

    private void Update()
    {
        if (tutorialVisible)
        {
            if (player.GetButtonDown("Action"))
            {
                HideVideo();
            }
        }
    }

    //Load Mechanic Video Data
    private void LoadVideo()
    {
        uIStreamVideo.videoPlayer.clip = mechanicVideoData.videoClip;
    }

    public void ShowVideo()
    {
        translator = GameManager.instance.GetComponent<Translator>();
        titleText.text = translator.GetTranslation(mechanicVideoData.titleId);
        descText.text = translator.GetTranslation(mechanicVideoData.descId);

        uiGroup.SetActive(true);
        tutorialVisible = true;
    }

    public void HideVideo()
    {
        //uiGroup.SetActive(false);
        m_animation.Play(bouncyHideClip.name);

        tutorialVisible = false;
        Time.timeScale = 1f;
        GrayscaleEffect.Instance.FadeToAndFromGray(false);
    }
}
