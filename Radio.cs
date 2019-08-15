using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the radio which showcases Leon Arndt's and Wieland Müller's music.
/// </summary>
public class Radio : Interactable {
    private bool currentlyPlayingMusic;

    public override void Interact(Transform transform)
    {
        if (!currentlyPlayingMusic)
        {
            currentlyPlayingMusic = true;
            AudioManager.instance.StopAll();
            AudioManager.instance.PlayEvent("Play_radio_music");
        }
        else
        {
            currentlyPlayingMusic = false;
            AudioManager.instance.StopAll();
            AudioManager.instance.PlayEvent("Play_station_music");
        }
    }
}
